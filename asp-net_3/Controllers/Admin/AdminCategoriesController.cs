using asp_net_3.Data;
using asp_net_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminCategoriesController : Controller {
        private readonly ApplicationDbContext _context;

        public AdminCategoriesController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            return View(await _context.Categories.OrderBy(x => x.Id).ToListAsync());
        }

        public IActionResult Create() {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category) {
            if (string.IsNullOrWhiteSpace(category.Name))
                ModelState.AddModelError("Name", "Введите название категории");

            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category) {
            if (id != category.Id)
                return NotFound();

            if (string.IsNullOrWhiteSpace(category.Name))
                ModelState.AddModelError("Name", "Введите название категории");

            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            Category? category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);

            try {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } catch (DbUpdateException) {
                TempData["DeleteError"] = "Не удалось удалить запись: есть связанные данные в базе.";
                return RedirectToAction("Delete", new { id });
            }
        }
    }
}
