using asp_net_3.Data;
using asp_net_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminCartsController : Controller {
        private readonly ApplicationDbContext _context;

        public AdminCartsController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            List<Cart> carts = await _context.Carts
                .Include(x => x.User)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return View(carts);
        }

        public IActionResult Create() {
            LoadUsers();
            return View(new Cart());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cart cart) {
            if (!ModelState.IsValid) {
                LoadUsers(cart.UserId);
                return View(cart);
            }

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            Cart? cart = await _context.Carts.FindAsync(id);
            if (cart == null)
                return NotFound();

            LoadUsers(cart.UserId);
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cart cart) {
            if (id != cart.Id)
                return NotFound();

            if (!ModelState.IsValid) {
                LoadUsers(cart.UserId);
                return View(cart);
            }

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            Cart? cart = await _context.Carts
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cart == null)
                return NotFound();

            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            Cart? cart = await _context.Carts.FindAsync(id);
            if (cart == null)
                return NotFound();

            _context.Carts.Remove(cart);

            try {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } catch (DbUpdateException) {
                TempData["DeleteError"] = "Не удалось удалить запись: есть связанные данные в базе.";
                return RedirectToAction("Delete", new { id });
            }
        }

        private void LoadUsers(int selectedId = 0) {
            ViewBag.Users = new SelectList(_context.Users.OrderBy(x => x.Login).ToList(), "Id", "Login", selectedId);
        }
    }
}
