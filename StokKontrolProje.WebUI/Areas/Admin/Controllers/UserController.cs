using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.WebUI.Models;
using System.Text;

namespace StokKontrolProje.WebUI.Areas.Admin.Controllers
{

    [Area("Admin"), Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public UserController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        string uri = "https://localhost:7121";


        public async Task<IActionResult> Index()
        {

            List<User> kullanicilar = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/TumKullanicilariGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);

                }
            }

            return View(kullanicilar);
        }
        [HttpGet]
        public async Task<IActionResult> ActivateUser(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/KullaniciAktiflestir/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteUser(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/User/KullaniciSil/{id}"))
                {


                }
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user,List<IFormFile> files)
        {
            user.IsActive = true;
            user.Password = "123456A.";
            string imgPath = Upload.ImageUpload(files,_environment,out bool imgResult);

            if (imgResult)
            {
                user.PhotoURL = imgPath;
            }
            else
            {
                ViewBag.Message = "Resimyüklenemedi";
                return View();
            }

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/User/KullaniciEkle", content))
                {


                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }

        static User updatedUser;
        

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/IdyegoreKullanicilariGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedUser = JsonConvert.DeserializeObject<User>(apiCevap);

                }
            }

            return View(updatedUser);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User guncelUser,List<IFormFile> files)
        {
            if (files.Count==0)
            {
                guncelUser.PhotoURL=updatedUser.PhotoURL;
            }
            else
            {
                string returnedMessage = Upload.ImageUpload(files, _environment, out bool imgResult);

                if (imgResult)
                {
                    guncelUser.PhotoURL = returnedMessage;
                }
                else
                {
                    ViewBag.Message=returnedMessage;
                    return View();
                }
            }
            using (var httpClient = new HttpClient())
            {
                guncelUser.AddedDate = updatedUser.AddedDate;
                guncelUser.IsActive = updatedUser.IsActive;
                guncelUser.Password=updatedUser.Password;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelUser), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{uri}/api/User/KullaniciGuncelle/{guncelUser.ID}", content))
                {

                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }




    }
}
