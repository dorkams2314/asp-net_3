using Microsoft.AspNetCore.Mvc;

namespace asp_net_3.Controllers.Admin {
    public class AdminController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
