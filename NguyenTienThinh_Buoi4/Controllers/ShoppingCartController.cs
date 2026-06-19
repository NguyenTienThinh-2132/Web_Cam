using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CamZone.Models;
using CamZone.Repositories;
using System.Text.Json;

namespace CamZone.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string CartCookieName = "Cart";
        private const int CartCookieDays = 30;

        public ShoppingCartController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        private ShoppingCart GetCart()
        {
            var user = _userManager.GetUserAsync(User).Result;
            string cartKey = user?.Id ?? "GuestCart";

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>($"Cart_{cartKey}");
            if (cart != null) return cart;

            if (Request.Cookies.TryGetValue(CartCookieName, out var cartJson))
            {
                try
                {
                    cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson);
                    if (cart != null)
                    {
                        HttpContext.Session.SetObjectAsJson($"Cart_{cartKey}", cart);
                        return cart;
                    }
                }
                catch { }
            }
            return new ShoppingCart();
        }

        private void SaveCart(ShoppingCart cart)
        {
            var user = _userManager.GetUserAsync(User).Result;
            string cartKey = user?.Id ?? "GuestCart";

            HttpContext.Session.SetObjectAsJson($"Cart_{cartKey}", cart);

            // Chỉ lưu cookie cho guest (user chưa đăng nhập)
            if (user == null)
            {
                var cartJson = JsonSerializer.Serialize(cart);
                var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(CartCookieDays),
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                };
                Response.Cookies.Append(CartCookieName, cartJson, cookieOptions);
            }
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            Response.Cookies.Delete(CartCookieName);
        }

        /// <summary>
        /// View giỏ hàng
        /// </summary>
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        /// <summary>
        /// Lấy thông tin đầy đủ của 1 sản phẩm thông qua ID
        /// </summary>
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }

        /// <summary>
        /// Add sản phẩm vô giỏ hàng. Nếu là AJAX thì trả JSON, không thì redirect.
        /// </summary>
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await GetProductFromDatabase(productId);

            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            };

            var cart = GetCart();
            cart.AddItem(cartItem);
            SaveCart(cart);

            // AJAX request → trả JSON, không redirect
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                var count = cart.Items.Sum(i => i.Quantity);
                return Json(new { success = true, count, message = $"Đã thêm \"{product.Name}\" vào giỏ hàng!" });
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Remove sản phẩm khỏi giỏ hàng
        /// </summary>
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Trả về số lượng sản phẩm trong giỏ hàng (dùng cho AJAX)
        /// </summary>
        public IActionResult GetCartItemCount()
        {
            var cart = GetCart();
            var count = cart.Items.Sum(i => i.Quantity);
            return Json(new { count });
        }

        /// <summary>
        /// Checkout giỏ hàng -- hiển thị để người dùng nhập thêm thông tin
        /// </summary>
        [Authorize]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index");
            return View(new Order());
        }

        /// <summary>
        /// Tiến hành lưu giỏ hàng xuống cơ sở dữ liệu
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = GetCart();
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            ClearCart();

            return View("OrderCompleted", order.Id);
        }
    }
}
