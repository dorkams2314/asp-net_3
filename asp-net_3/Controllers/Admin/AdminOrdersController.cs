using asp_net_3.Data;
using asp_net_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminOrdersController : Controller {
        private readonly ApplicationDbContext _context;

        public AdminOrdersController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            List<Order> orders = await _context.Orders
                .Include(x => x.User)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return View(orders);
        }

        public IActionResult Create() {
            LoadUsers();
            return View(new Order());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order) {
            if (!ModelState.IsValid) {
                LoadUsers(order.UserId);
                return View(order);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            Order? order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            LoadUsers(order.UserId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order) {
            if (id != order.Id)
                return NotFound();

            if (!ModelState.IsValid) {
                LoadUsers(order.UserId);
                return View(order);
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            Order? order = await _context.Orders
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            Order? order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);

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
