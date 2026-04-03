using asp_net_3.Data;
using asp_net_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminCartItemsController : Controller {
        private readonly ApplicationDbContext _context;

        public AdminCartItemsController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            List<CartItem> items = await _context.CartItems
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return View(items);
        }

        public IActionResult Create() {
            LoadData();
            return View(new CartItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CartItem cartItem) {
            if (cartItem.Quantity < 1)
                ModelState.AddModelError("Quantity", "Количество должно быть не меньше 1");

            if (!ModelState.IsValid) {
                LoadData(cartItem.CartId, cartItem.ProductId);
                return View(cartItem);
            }

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            CartItem? cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
                return NotFound();

            LoadData(cartItem.CartId, cartItem.ProductId);
            return View(cartItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CartItem cartItem) {
            if (id != cartItem.Id)
                return NotFound();

            if (cartItem.Quantity < 1)
                ModelState.AddModelError("Quantity", "Количество должно быть не меньше 1");

            if (!ModelState.IsValid) {
                LoadData(cartItem.CartId, cartItem.ProductId);
                return View(cartItem);
            }

            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            CartItem? cartItem = await _context.CartItems
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cartItem == null)
                return NotFound();

            return View(cartItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            CartItem? cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
                return NotFound();

            _context.CartItems.Remove(cartItem);

            try {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } catch (DbUpdateException) {
                TempData["DeleteError"] = "Не удалось удалить запись: есть связанные данные в базе.";
                return RedirectToAction("Delete", new { id });
            }
        }

        private void LoadData(int selectedCartId = 0, int selectedProductId = 0) {
            ViewBag.Carts = new SelectList(_context.Carts.OrderBy(x => x.Id).ToList(), "Id", "Id", selectedCartId);
            ViewBag.Products = new SelectList(_context.Products.OrderBy(x => x.Name).ToList(), "Id", "Name", selectedProductId);
        }
    }
}
