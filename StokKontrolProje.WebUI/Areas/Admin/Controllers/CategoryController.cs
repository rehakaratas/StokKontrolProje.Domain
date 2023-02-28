using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;
using System.Text;

namespace StokKontrolProje.WebUI.Areas.Admin.Controllers
{

    [Area("Admin"), Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {

        string uri = "https://localhost:7121";


        public async Task<IActionResult> Index()
        {

            List<Category> kategoriler=new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/TumKategorileriGetir"))
                {
                    string apiCevap=await cevap.Content.ReadAsStringAsync();
                    kategoriler=JsonConvert.DeserializeObject<List<Category>>(apiCevap);

                }
            }

            return View(kategoriler);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateCategory(int id)
        {
                      
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/KategoriAktiflestir/{id}"))
                {
                   

                }
            }
           
            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> DeleteCategory(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Category/KategoriSil/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            category.IsActive = true;

            using(var httpClient=new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Category/KategoriEkle",content))
                {
                   

                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    

                }
            }

            return RedirectToAction("Index");
        }

        static Category updatedCategory;

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
           
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/IdyegoreKategorileriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedCategory = JsonConvert.DeserializeObject<Category>(apiCevap);

                }
            }

            return View(updatedCategory);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category guncelCategory)
        {
           
            using (var httpClient = new HttpClient())
            {
                guncelCategory.AddedDate = updatedCategory.AddedDate;
                guncelCategory.IsActive = true;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelCategory), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Category/KategoriGuncelle/{guncelCategory.ID}", content))
                {

                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

    }
}
