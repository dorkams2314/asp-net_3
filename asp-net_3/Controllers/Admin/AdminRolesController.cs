using asp_net_3.Data;
using asp_net_3.Models;
using asp_net_3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminRolesController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly AdminDeleteService _adminDeleteService;

        public AdminRolesController(ApplicationDbContext context, AdminDeleteService adminDeleteService) {
            _context = context;
            _adminDeleteService = adminDeleteService;
        }

        public async Task<IActionResult> Index() {
            return View(await _context.Roles.OrderBy(x => x.Id).ToListAsync());
        }

        public IActionResult Create() {
            return View(new Role());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role) {
            if (string.IsNullOrWhiteSpace(role.Name))
                ModelState.AddModelError("Name", "Введите название роли");

            if (!ModelState.IsValid)
                return View(role);

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            Role? role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Role role) {
            if (id != role.Id)
                return NotFound();

            if (string.IsNullOrWhiteSpace(role.Name))
                ModelState.AddModelError("Name", "Введите название роли");

            if (!ModelState.IsValid)
                return View(role);

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            Role? role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
                return NotFound();

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            bool deleted = await _adminDeleteService.DeleteRoleAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
