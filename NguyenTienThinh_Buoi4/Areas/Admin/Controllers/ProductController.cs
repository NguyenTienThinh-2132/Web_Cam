using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CamZone.Models;
using CamZone.Repositories;

namespace CamZone.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            await SetCategoryViewBag();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(
            Product product,
            IFormFile? imageUrl,
            List<IFormFile>? imageUrls)
        {
            if (!ModelState.IsValid)
            {
                await SetCategoryViewBag(product.CategoryId);
                return View(product);
            }
            if (imageUrl != null)
                product.ImageUrl = await SaveImage(imageUrl);

            if (imageUrls != null && imageUrls.Any())
            {
                product.Images = new List<ProductImage>();
                foreach (var file in imageUrls)
                {
                    var imagePath = await SaveImage(file);
                    product.Images.Add(new ProductImage { Url = imagePath });
                }
            }
            await _productRepository.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            await SetCategoryViewBag(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            Product product,
            IFormFile? imageUrl,
            List<IFormFile>? imageUrls)
        {
            if (!ModelState.IsValid)
            {
                await SetCategoryViewBag(product.CategoryId);
                return View(product);
            }
            var existingProduct = await _productRepository.GetByIdAsync(product.Id);
            if (existingProduct == null) return NotFound();

            if (imageUrl != null)
                product.ImageUrl = await SaveImage(imageUrl);
            else
                product.ImageUrl = existingProduct.ImageUrl;

            if (imageUrls != null && imageUrls.Any())
            {
                product.Images = new List<ProductImage>();
                foreach (var file in imageUrls)
                {
                    var imagePath = await SaveImage(file);
                    product.Images.Add(new ProductImage { Url = imagePath });
                }
            }
            else
            {
                product.Images = existingProduct.Images;
            }
            await _productRepository.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task SetCategoryViewBag(int? selectedCategoryId = null)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine(directoryPath, fileName);
            using var fileStream = new FileStream(savePath, FileMode.Create);
            await image.CopyToAsync(fileStream);
            return "/images/" + fileName;
        }
    }
}