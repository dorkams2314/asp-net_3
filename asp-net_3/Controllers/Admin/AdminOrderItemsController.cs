using asp_net_3.Data;
using asp_net_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminOrderItemsController : Controller {
        private readonly ApplicationDbContext _context;

        public AdminOrderItemsController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            List<OrderItem> items = await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Product)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return View(items);
        }

        public IActionResult Create() {
            LoadData();
            return View(new OrderItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItem orderItem) {
            if (orderItem.Quantity < 1)
                ModelState.AddModelError("Quantity", "Количество должно быть не меньше 1");

            if (orderItem.Price <= 0)
                ModelState.AddModelError("Price", "Цена должна быть больше нуля");

            if (!ModelState.IsValid) {
                LoadData(orderItem.OrderId, orderItem.ProductId);
                return View(orderItem);
            }

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            OrderItem? orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
                return NotFound();

            LoadData(orderItem.OrderId, orderItem.ProductId);
            return View(orderItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderItem orderItem) {
            if (id != orderItem.Id)
                return NotFound();

            if (orderItem.Quantity < 1)
                ModelState.AddModelError("Quantity", "Количество должно быть не меньше 1");

            if (orderItem.Price <= 0)
                ModelState.AddModelError("Price", "Цена должна быть больше нуля");

            if (!ModelState.IsValid) {
                LoadData(orderItem.OrderId, orderItem.ProductId);
                return View(orderItem);
            }

            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            OrderItem? orderItem = await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (orderItem == null)
                return NotFound();

            return View(orderItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            OrderItem? orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
                return NotFound();

            _context.OrderItems.Remove(orderItem);

            try {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } catch (DbUpdateException) {
                TempData["DeleteError"] = "Не удалось удалить запись: есть связанные данные в базе.";
                return RedirectToAction("Delete", new { id });
            }
        }

        private void LoadData(int selectedOrderId = 0, int selectedProductId = 0) {
            ViewBag.Orders = new SelectList(_context.Orders.OrderBy(x => x.Id).ToList(), "Id", "Id", selectedOrderId);
            ViewBag.Products = new SelectList(_context.Products.OrderBy(x => x.Name).ToList(), "Id", "Name", selectedProductId);
        }
    }
}
