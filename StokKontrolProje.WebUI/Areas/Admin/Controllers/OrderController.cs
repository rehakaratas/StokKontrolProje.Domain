using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;

namespace StokKontrolProje.WebUI.Areas.Admin.Controllers
{

    [Area("Admin"), Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {



        string uri = "https://localhost:7121";


        public async Task<IActionResult> Index()
        {

            List<Order> siparisler = new List<Order>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/TumSiparisleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap);

                }
            }

            return View(siparisler);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisOnayla/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CancelOrder(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisReddet/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Details(int id)
        {
            List<OrderDetails> siparis = new List<OrderDetails>();

            using (var client = new HttpClient())
            {
                using (var cevap = await client.GetAsync($"{uri}/api/Order/DetaylariGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    siparis = JsonConvert.DeserializeObject<List<OrderDetails>>(apiCevap)!;
                }
            }

            return View(siparis);
        }
    }
}
