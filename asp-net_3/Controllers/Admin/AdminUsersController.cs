using asp_net_3.Data;
using asp_net_3.Models;
using asp_net_3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers.Admin {
    public class AdminUsersController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly AdminDeleteService _adminDeleteService;

        public AdminUsersController(ApplicationDbContext context, AdminDeleteService adminDeleteService) {
            _context = context;
            _adminDeleteService = adminDeleteService;
        }

        public async Task<IActionResult> Index() {
            List<User> users = await _context.Users
                .Include(x => x.Role)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return View(users);
        }

        public IActionResult Create() {
            LoadRoles();
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user) {
            if (string.IsNullOrWhiteSpace(user.Login))
                ModelState.AddModelError("Login", "Введите логин");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                ModelState.AddModelError("PasswordHash", "Введите пароль");

            if (!ModelState.IsValid) {
                LoadRoles(user.RoleId);
                return View(user);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            User? user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            LoadRoles(user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user) {
            if (id != user.Id)
                return NotFound();

            if (string.IsNullOrWhiteSpace(user.Login))
                ModelState.AddModelError("Login", "Введите логин");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                ModelState.AddModelError("PasswordHash", "Введите пароль");

            if (!ModelState.IsValid) {
                LoadRoles(user.RoleId);
                return View(user);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            User? user = await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Carts)
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound();

            SetDeleteInfo(user);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            bool deleted = await _adminDeleteService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }

        private void LoadRoles(int selectedId = 0) {
            ViewBag.Roles = new SelectList(_context.Roles.OrderBy(x => x.Name).ToList(), "Id", "Name", selectedId);
        }

        private void SetDeleteInfo(User user) {
            ViewBag.CartCount = user.Carts.Count;
            ViewBag.OrderCount = user.Orders.Count;
        }
    }
}
