using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CamZone.Models;

namespace CamZone.Areas.Admin.Controllers
{

    //Chú ý phần vùng cho controller
    [Area("Admin")]
    //Muốn vào thì phải login
    [Authorize]
    public class DashboardController : Controller
    {
        //Kết nối đến csdl
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            // Thống kê tổng số khách hàng (khác nhau)
            var totalCustomers = await _context.Orders.Select(
                o => o.UserId).Distinct().CountAsync();

            // Tổng số lượng đơn hàng
            var totalOrders = await _context.Orders.CountAsync();

            // Tổng doanh số
            var totalRevenue = await _context.Orders.SumAsync(
                o => o.TotalPrice);

            //Đưa dữ liệu qua view thông qua biến ViewBag
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }
        //Trả về data để vẽ chart
        [HttpGet]
        public async Task<JsonResult> GetChartData()
        {
            //mặc định lấy cố định 30 ngày tính từ thời điểm hiện tại
            var today = DateTime.UtcNow;
            var last30Days = today.AddDays(-30); //lấy trong vòng 30 ngày

            //thực hiện query để lấy data về
            var ordersData = await _context.Orders
                .Where(o => o.OrderDate >= last30Days && o.OrderDate <= today)
                .GroupBy(o => o.OrderDate.Date) //group by theo ngày 
                .Select(g => new
                {
                    Date = g.Key, // Để nguyên dạng DateTime
                    Orders = g.Count(), //count số lượng đơn trong ngày
                    Revenue = g.Sum(o => o.TotalPrice) //tính tổng doanh thu trong ngày
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            // Chuyển đổi DateTime thành chuỗi sau khi lấy từ database
            var result = ordersData.Select(d => new
            {
                Date = d.Date.ToString("yyyy-MM-dd"),
                Orders = d.Orders,
                Revenue = d.Revenue
            }).ToList();

            return Json(result);
        }
    }
}
