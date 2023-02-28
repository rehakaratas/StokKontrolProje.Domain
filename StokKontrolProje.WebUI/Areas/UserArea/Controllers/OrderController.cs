using Microsoft.AspNetCore.Mvc;

namespace StokKontrolProje.WebUI.Areas.UserArea.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
