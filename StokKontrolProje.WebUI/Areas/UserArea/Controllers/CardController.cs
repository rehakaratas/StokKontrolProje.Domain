using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProje.Domain.Entities;
using System.Text;

namespace StokKontrolProje.WebUI.Areas.UserArea.Controllers
{
    [Area("UserArea"), Authorize(Roles = "User")]
    public class CardController : Controller
    {
        public IActionResult Index()
        {
            return View(addedToCard);
        }


        static CardController()
        {
            addedToCard = new List<Product>();
        }
        string uri = "https://localhost:7121";

        static Product product;
        static List<Product> addedToCard;

        [HttpGet]
        public async Task<IActionResult> AddToCard(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/Product/IdyegoreUrunleriGetir/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    product = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }

            addedToCard.Add(product);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> CompleteOrder()
        {
            int[] productIDs = new int[0];
            short[] quantities = new short[0];
            for (int i = 0; i < addedToCard.Count; i++)
            {
                Array.Resize(ref productIDs, productIDs.Length);
                Array.Resize(ref quantities, quantities.Length);
                productIDs[i] = addedToCard[i].ID;
                quantities[i] = 1;
            }


            string gidecekProductIDs = "";

            foreach (var item in productIDs)
            {
                gidecekProductIDs += "&productIDs=" + item;
            }

            string gidecekQuantities = "";

            foreach (var item in quantities)
            {
                gidecekQuantities += "&quantities=" + item;
            }

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PostAsync($"{uri}/api/Order/SiparisEkle?userID={HttpContext.User.FindFirst("ID").Value}{gidecekProductIDs}{gidecekQuantities}", content))
                {


                    string apiCevap = await cevap.Content.ReadAsStringAsync();


                }
            }

            return RedirectToAction("Index");
        }
    }
}
