using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.Domain.Enums;
using StokKontrolProje.WebUI.Models;
using System.Security.Claims;

namespace StokKontrolProje.WebUI.Controllers
{
    public class AccountController : Controller
    {

        string uri = "https://localhost:7121";

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {

            User logged = new User();
           
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/Login?email={dto.Email}&password={dto.Password}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    logged = JsonConvert.DeserializeObject<User>(apiCevap);

                }
            }

            if (logged!=null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("ID",logged.ID.ToString()),
                    new Claim("CompanyID",logged.CompanyID.ToString()),
                    new Claim(ClaimTypes.Name,logged.FirstName),
                    new Claim(ClaimTypes.Surname,logged.LastName),
                    new Claim(ClaimTypes.Email,logged.Email),
                    new Claim(ClaimTypes.Role,logged.Role.ToString()),
                    new Claim("Image",value:logged.PhotoURL),
                };


                var userIdentity=new ClaimsIdentity(claims,"login");
                ClaimsPrincipal principal=new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
            }


            else
            {
                return View(dto);
            }


            switch (logged.Role)
            {
                case UserRole.Admin:
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                case UserRole.Supplier:
                    return RedirectToAction("Index", "Home", new { Area = "SupplierArea" });
                case UserRole.User:
                    return RedirectToAction("Index", "Home", new { Area = "UserArea" });
                default:
                    return View(dto);
            }

           
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account", new { Area = "" });
        }
    }
}
