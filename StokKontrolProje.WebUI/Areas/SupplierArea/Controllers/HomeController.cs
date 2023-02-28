using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;

namespace StokKontrolProje.WebUI.Areas.SupplierArea.Controllers
{

    [Area("SupplierArea"),Authorize(Roles = "Supplier")]
    public class HomeController : Controller
    {
        string uri = "https://localhost:7121";

        public async Task<IActionResult> Index()
        {
            // Tüm tedarikciler gelsin istiyoruz.

            List<Supplier> tedarikciler = new List<Supplier>();

            using (var client = new HttpClient())
            {
                using (var cevap = await client.GetAsync($"{uri}/api/Supplier/TumtedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap)!;
                }
            }

            return View(tedarikciler);
        }

        public async Task<IActionResult> FilteredList()
        {

            List<Product> tedarikciUrunleri = new List<Product>();

            using (var client = new HttpClient())
            {
                using (var cevap = await client.GetAsync($"{uri}/api/Product/TedarikciyeGoreUrunleriGetir/{HttpContext.User.FindFirst("CompanyID")?.Value.ToString()}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    tedarikciUrunleri = JsonConvert.DeserializeObject<List<Product>>(apiCevap)!;
                }
            }

            return View(tedarikciUrunleri);
        }
    }
}
