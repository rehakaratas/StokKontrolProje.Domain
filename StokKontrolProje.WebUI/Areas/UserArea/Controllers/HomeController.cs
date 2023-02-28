using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;

namespace StokKontrolProje.WebUI.Areas.UserArea.Controllers
{

    [Area("UserArea"), Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        string uri = "https://localhost:7121";

        static List<Product> activeProducts;

        public async Task<IActionResult> Index()
        {
            using (var httpClient=new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Product/AktifUrunleriGetir"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    activeProducts = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }


            return View(activeProducts);
        }
    }
}
