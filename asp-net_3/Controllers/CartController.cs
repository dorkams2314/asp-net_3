using asp_net_3.Data;
using asp_net_3.Models;
using asp_net_3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers {
    public class CartController : Controller {
        private readonly ApplicationDbContext _context;
        private const int DemoUserId = 2;

        public CartController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            Cart cart = await GetOrCreateCartAsync();

            List<CartItem> cartItems = await _context.CartItems
                .Where(item => item.CartId == cart.Id)
                .Include(item => item.Product)
                .ThenInclude(product => product!.Category)
                .OrderBy(item => item.Id)
                .ToListAsync();

            CartViewModel model = new CartViewModel();

            foreach (CartItem item in cartItems) {
                CartItemViewModel cartItemViewModel = new CartItemViewModel();
                cartItemViewModel.Id = item.Id;
                cartItemViewModel.ProductId = item.ProductId;
                cartItemViewModel.Quantity = item.Quantity;

                if (item.Product != null) {
                    cartItemViewModel.ProductName = item.Product.Name;
                    cartItemViewModel.Description = item.Product.Description;
                    cartItemViewModel.Price = item.Product.Price;

                    if (item.Product.Category != null)
                        cartItemViewModel.CategoryName = item.Product.Category.Name;
                }

                model.Items.Add(cartItemViewModel);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId) {
            Product? product = await _context.Products.FindAsync(productId);

            if (product == null)
                return NotFound();

            Cart cart = await GetOrCreateCartAsync();

            CartItem? cartItem = await _context.CartItems
                .FirstOrDefaultAsync(item => item.CartId == cart.Id && item.ProductId == productId);

            if (cartItem == null) {
                cartItem = new CartItem();
                cartItem.CartId = cart.Id;
                cartItem.ProductId = productId;
                cartItem.Quantity = 1;

                _context.CartItems.Add(cartItem);
            } else {
                cartItem.Quantity = cartItem.Quantity + 1;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = "Товар добавлен в корзину";

            return RedirectToAction("Index", "Store");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity) {
            CartItem? cartItem = await _context.CartItems.FindAsync(itemId);

            if (cartItem == null)
                return NotFound();

            if (quantity < 1) // проверочка на меньше 1
                quantity = 1;

            cartItem.Quantity = quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int itemId) {
            CartItem? cartItem = await _context.CartItems.FindAsync(itemId);

            if (cartItem == null)
                return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private async Task<Cart> GetOrCreateCartAsync() {
            Cart? cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == DemoUserId);

            if (cart == null) {
                cart = new Cart();
                cart.UserId = DemoUserId;

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}
