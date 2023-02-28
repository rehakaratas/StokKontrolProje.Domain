using Microsoft.AspNetCore.Mvc;

namespace StokKontrolProje.WebUI.Areas.UserArea.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
