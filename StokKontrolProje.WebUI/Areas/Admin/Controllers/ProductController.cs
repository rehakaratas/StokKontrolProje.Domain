using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;
using System.Text;

namespace StokKontrolProje.WebUI.Areas.Admin.Controllers
{

    [Area("Admin"), Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        string uri = "https://localhost:7121";


        public async Task<IActionResult> Index()
        {

            List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/TumUrunleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);

                }
            }

            return View(urunler);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateProduct(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/UrunAktiflestir/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteProduct(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Product/UrunSil/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }
        static List<Category> aktifCategories;
        static List<Supplier> aktifSuppliers;


        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifCategories = JsonConvert.DeserializeObject<List<Category>>(apiCevap);

                }

                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifSuppliers = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);

                }
            }

            ViewBag.AktifCategories = new SelectList(aktifCategories, "ID", "CategoryName");
            ViewBag.AktifSuppliers = new SelectList(aktifSuppliers, "ID", "SupplierName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            product.IsActive = true;

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Product/UrunEkle", content))
                {


                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }
        
        static Product updatedProduct;

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/IdyegoreUrunleriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedProduct = JsonConvert.DeserializeObject<Product>(apiCevap);

                }

                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifCategories = JsonConvert.DeserializeObject<List<Category>>(apiCevap);

                }

                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifSuppliers = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);

                }
            }

            ViewBag.AktifCategories = new SelectList(aktifCategories, "ID", "CategoryName");
            ViewBag.AktifSuppliers = new SelectList(aktifSuppliers, "ID", "SupplierName");

            return View(updatedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product guncelProduct)
        {

            using (var httpClient = new HttpClient())
            {
                guncelProduct.AddedDate = updatedProduct.AddedDate;
                guncelProduct.IsActive = updatedProduct.IsActive;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelProduct), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Product/UrunGuncelle/{guncelProduct.ID}", content))
                {

                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
