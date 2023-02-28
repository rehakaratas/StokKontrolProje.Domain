using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StokKontrolProje.WebUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin"), Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
