using asp_net_3.Data;
using asp_net_3.Models;
using asp_net_3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminProductsController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly AdminDeleteService _adminDeleteService;

        public AdminProductsController(ApplicationDbContext context, AdminDeleteService adminDeleteService) {
            _context = context;
            _adminDeleteService = adminDeleteService;
        }

        public async Task<IActionResult> Index() {
            List<Product> products = await _context.Products
                .Include(x => x.Category)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return View(products);
        }

        public IActionResult Create() {
            LoadCategories();
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product) {
            if (!ModelState.IsValid) {
                LoadCategories(product.CategoryId);
                return View(product);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            LoadCategories(product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product) {
            if (id != product.Id)
                return NotFound();

            if (!ModelState.IsValid) {
                LoadCategories(product.CategoryId);
                return View(product);
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            Product? product = await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            bool deleted = await _adminDeleteService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }

        private void LoadCategories(int selectedId = 0) {
            ViewBag.Categories = new SelectList(_context.Categories.OrderBy(x => x.Name).ToList(), "Id", "Name", selectedId);
        }
    }
}
