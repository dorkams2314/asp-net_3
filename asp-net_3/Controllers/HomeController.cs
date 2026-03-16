using asp_net_3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace asp_net_3.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return RedirectToAction("Index", "Store");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
