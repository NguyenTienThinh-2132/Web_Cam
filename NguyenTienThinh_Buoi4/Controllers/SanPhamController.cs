using Microsoft.AspNetCore.Mvc;
using CamZone.Repositories;

namespace CamZone.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SanPhamController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm - hỗ trợ lọc theo danh mục
        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = await _productRepository.GetAllAsync();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
                ViewBag.SelectedCategoryId = categoryId.Value;
            }

            return View(products);
        }

        // Lấy danh sách danh mục dưới dạng JSON (cho AJAX)
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Json(categories.Select(c => new { id = c.Id, name = c.Name }));
        }
    }
}
