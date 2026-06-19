using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CamZone.Models;

namespace CamZone.Areas.Admin.Controllers
{

    //khu vực admin
    [Area("Admin")]
    //Muốn vào là phải login
    [Authorize]
    public class UserRoleController : Controller
    {
        //Kết nối đến csdl
        private readonly ApplicationDbContext _context;

        public UserRoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        //chỉ cấp quyền cho admin
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            //Query lấy thông tin user và role quyền
            var users = await (
                from u in _context.Users
                join ur in _context.UserRoles on u.Id equals ur.UserId into userRoleGroup
                from ur in userRoleGroup.DefaultIfEmpty()
                join r in _context.Roles on ur.RoleId equals r.Id into roleGroup
                from r in roleGroup.DefaultIfEmpty()
                select new
                {
                    UserId = u.Id,
                    u.UserName,
                    u.FullName,
                    u.Email,
                    RoleId = r != null ? r.Id : "",
                    RoleName = r != null ? r.Name : ""
                }
            ).ToListAsync();

            //trả về view để xử lý
            ViewBag.Users = users;
            //lấy danh sách role quyền
            ViewBag.Roles = await _context.Roles.ToListAsync();

            return View();
        }
        /// <summary>
        /// Cập nhật thông tin role quyền cho user
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<IActionResult> UpdateRoles(string[] userIds, string[] roleIds)
        {
            //Thực hiện update cho từng user từ list
            for (int i = 0; i < userIds.Length; i++)
            {
                var userId = userIds[i];
                var roleId = roleIds[i];

                var oldRole = await _context.UserRoles
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                //Loại bỏ role quyền củ
                if (oldRole.Any())
                {
                    _context.UserRoles.RemoveRange(oldRole);
                }

                //add role quyền mới
                _context.UserRoles.Add(
                    new IdentityUserRole<string>
                    {
                        UserId = userId,
                        RoleId = roleId
                    });
            }

            //Lưu lại thay đổi trong csdl
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật quyền thành công";

            return RedirectToAction(nameof(Index));
        }
    }
}
