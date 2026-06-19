using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CamZone.Models;

namespace CamZone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportController : Controller
    {
        //Kết nối đến csdl
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Hàm lấy thống kê theo khoảng thời gian, Chỉ có quyền admin mới vào được
        /// </summary>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <returns></returns>
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        {
            //Nếu không có ngày thì mặc định lấy ngày đầu tiên của tháng
            if (!fromDate.HasValue)
            {
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            //Nếu không có ngày thì mặc định lấy hiện tại
            if (!toDate.HasValue)
            {
                toDate = DateTime.Now;
            }

            //Gửi dữ liệu qua view và cho phép user chỉnh sửa thông tin này
            ViewBag.FromDate = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.Value.ToString("yyyy-MM-dd");

            //Thực hiện câu truy vấn lấy về danh sách đơn hàng theo khoảng thời gian
            var orders = _context.Orders.Where(x => x.OrderDate >= fromDate && x.OrderDate <= toDate);

            //Tổng doanh thu
            ViewBag.TotalRevenue = await orders.SumAsync(x => (decimal?)x.TotalPrice) ?? 0;
            //Tổng đơn hàng
            ViewBag.TotalOrders = await orders.CountAsync();
            //Tổng khách hàng
            ViewBag.TotalCustomers = await orders.Select(x => x.UserId).Distinct().CountAsync();

            //Lấy về danh sách id đơn hàng
            var orderIds = await orders.Select(x => x.Id).ToListAsync();
            //Lấy về chi tiết danh sách đơn hàng dựa trên danh sách id đơn hàng ở trên
            var details = _context.OrderDetails.Where(x => orderIds.Contains(x.OrderId));

            //Tổng số lượng sản phẩm đã bán
            ViewBag.TotalQuantity = await details.SumAsync(x => (int?)x.Quantity) ?? 0;
            //Tổng mặt hàng đã bán
            ViewBag.TotalProducts = await details.Select(x => x.ProductId).Distinct().CountAsync();

            // =========================
            // Top 5 sản phẩm bán chạy
            // =========================
            ViewBag.TopProducts = await
            (
                from od in _context.OrderDetails
                join p in _context.Products on od.ProductId equals p.Id
                where orderIds.Contains(od.OrderId)
                group od by new { p.Id, p.Name }
                into g
                orderby g.Sum(x => x.Quantity) descending
                select new
                {
                    g.Key.Name,
                    Quantity = g.Sum(x => x.Quantity)
                }
            ).Take(5).ToListAsync();

            // =========================
            // Top 5 sản phẩm bán tệ
            // =========================
            ViewBag.WorstProducts = await
            (
                from od in _context.OrderDetails
                join p in _context.Products on od.ProductId equals p.Id
                where orderIds.Contains(od.OrderId)
                group od by new { p.Id, p.Name }
                into g
                orderby g.Sum(x => x.Quantity)
                select new
                {
                    g.Key.Name,
                    Quantity = g.Sum(x => x.Quantity)
                }
            ).Take(5).ToListAsync();

            // =========================
            // Top 5 Category bán chạy
            // =========================

            ViewBag.TopCategories = await
            (
                from od in _context.OrderDetails
                join p in _context.Products on od.ProductId equals p.Id
                join c in _context.Categories on p.CategoryId equals c.Id
                where orderIds.Contains(od.OrderId)
                group od by new { c.Id, c.Name }
                into g
                orderby g.Sum(x => x.Quantity) descending
                select new
                {
                    g.Key.Name,
                    Quantity = g.Sum(x => x.Quantity)
                }
            ).Take(5).ToListAsync();

            // =========================
            // Top 5 Category bán tệ
            // =========================

            ViewBag.WorstCategories = await
            (
                from od in _context.OrderDetails
                join p in _context.Products on od.ProductId equals p.Id
                join c in _context.Categories on p.CategoryId equals c.Id
                where orderIds.Contains(od.OrderId)
                group od by new { c.Id, c.Name }
                into g
                orderby g.Sum(x => x.Quantity)
                select new
                {
                    g.Key.Name,
                    Quantity = g.Sum(x => x.Quantity)
                }
            ).Take(5).ToListAsync();

            // =========================
            // Top 5 khách hàng mua nhiều nhất
            // =========================
            ViewBag.TopCustomers = await
            (
                from o in _context.Orders
                join u in _context.Users on o.UserId equals u.Id
                where o.OrderDate >= fromDate && o.OrderDate <= toDate
                group o by new { u.Id, u.FullName, u.UserName }
                into g
                orderby g.Sum(x => x.TotalPrice) descending
                select new
                {
                    Customer =
                        g.Key.FullName ??
                        g.Key.UserName,
                    Revenue =
                        g.Sum(x => x.TotalPrice),
                    Orders =
                        g.Count()
                }
            ).Take(5).ToListAsync();

            return View();
        }
    }
}
