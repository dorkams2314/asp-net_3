using asp_net_3.Data;
using asp_net_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Controllers {
    public class StoreController : Controller {
        private readonly ApplicationDbContext _context;

        public StoreController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            List<Product> products = await _context.Products
                .Include(product => product.Category)
                .OrderBy(product => product.Name)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Details(int id) {
            Product? product = await _context.Products
                .Include(product => product.Category)
                .FirstOrDefaultAsync(product => product.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
