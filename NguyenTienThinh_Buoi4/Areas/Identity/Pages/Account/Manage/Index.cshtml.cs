using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CamZone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CamZone.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string AvatarUrl { get; set; }
        public List<Order> UserOrders { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public IFormFile AvatarFile { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            public string FullName { get; set; }
            public string Address { get; set; }
            public int Age { get; set; }
            public string GioiTinh { get; set; }
            public string NgheNghiep { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            FullName = user.FullName;
            Email = await _userManager.GetEmailAsync(user);
            Address = user.Address;
            Age = user.Age;
            AvatarUrl = user.AvatarUrl ?? "/images/default-avatar.png";

            UserOrders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            Input = new InputModel
            {
                FullName = user.FullName,
                Address = user.Address,
                Age = user.Age,
                GioiTinh = user.GioiTinh,
                NgheNghiep = user.NgheNghiep,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Cập nhật toàn bộ fields của user
            user.FullName = Input.FullName;
            user.Address = Input.Address;
            user.Age = Input.Age;
            user.GioiTinh = Input.GioiTinh;
            user.NgheNghiep = Input.NgheNghiep;

            // Xử lý góc hiển thị ảnh đại diện (avatar position)
            var avatarPos = Request.Form["AvatarPosition"].ToString();
            if (!string.IsNullOrEmpty(avatarPos) && !string.IsNullOrEmpty(user.AvatarUrl))
            {
                var basePath = user.AvatarUrl.Split('?')[0];
                user.AvatarUrl = $"{basePath}?pos={avatarPos}";
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Lỗi khi cập nhật hồ sơ.";
                return RedirectToPage();
            }

            // Cập nhật PhoneNumber nếu thay đổi
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Lỗi khi cập nhật số điện thoại.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Hồ sơ đã được cập nhật thành công!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUploadAvatarAsync()
        {
            if (AvatarFile == null || AvatarFile.Length == 0)
            {
                StatusMessage = "Vui lòng chọn ảnh để upload.";
                return RedirectToPage();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = System.IO.Path.GetExtension(AvatarFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                StatusMessage = "Định dạng ảnh không được hỗ trợ. Vui lòng sử dụng JPG, PNG hoặc GIF.";
                await LoadAsync(user);
                return Page();
            }

            // Max file size: 5MB
            if (AvatarFile.Length > 5 * 1024 * 1024)
            {
                StatusMessage = "Kích thước ảnh không được vượt quá 5MB.";
                await LoadAsync(user);
                return Page();
            }

            try
            {
                // Create uploads directory if it doesn't exist
                var uploadsDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // Generate unique filename
                var fileName = $"{user.Id}_{DateTime.UtcNow.Ticks}{extension}";
                var filePath = System.IO.Path.Combine(uploadsDir, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                // Update user avatar URL với góc hiển thị mặc định là center
                user.AvatarUrl = $"/uploads/avatars/{fileName}?pos=center";
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    StatusMessage = "Lỗi khi cập nhật ảnh đại diện.";
                    await LoadAsync(user);
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Ảnh đại diện đã được cập nhật thành công!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi: {ex.Message}";
                await LoadAsync(user);
                return Page();
            }
        }
    }
}
