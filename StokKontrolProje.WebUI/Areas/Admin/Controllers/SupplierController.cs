using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;
using System.Text;

namespace StokKontrolProje.WebUI.Areas.Admin.Controllers
{

    [Area("Admin"), Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        string uri = "https://localhost:7121";


        public async Task<IActionResult> Index()
        {

            List<Supplier> tedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TumTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);

                }
            }

            return View(tedarikciler);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateSupplier(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TedarikciAktiflestir/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteSupplier(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Supplier/TedarikciSil/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            supplier.IsActive = true;

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Supplier/TedarikciEkle", content))
                {


                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }

        static Supplier updatedSupplier;

        [HttpGet]
        public async Task<IActionResult> UpdateSupplier(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/IdyegoreTedarikcileriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedSupplier = JsonConvert.DeserializeObject<Supplier>(apiCevap);

                }
            }

            return View(updatedSupplier);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Supplier guncelSupplier)
        {

            using (var httpClient = new HttpClient())
            {
                guncelSupplier.AddedDate = updatedSupplier.AddedDate;
                guncelSupplier.IsActive = true;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelSupplier), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Supplier/TedarikciGuncelle/{guncelSupplier.ID}", content))
                {

                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
