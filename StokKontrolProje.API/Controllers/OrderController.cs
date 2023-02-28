using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.Domain.Enums;
using StokKontrolProje.Service.Abstract;

namespace StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
       
        private readonly IGenericService<User> _userService;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<Order> _orderService;
        private readonly IGenericService<OrderDetails> _odService;

        public OrderController(IGenericService<User> userService, IGenericService<Product> productService, IGenericService<Order> orderService, IGenericService<OrderDetails> odService)
        {
           
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
            _odService = odService;
        }


        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(_orderService.GetAll(t0=>t0.OrderDetails,t1=>t1.User));
        }

        [HttpGet]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(_orderService.GetAll(t0 => t0.OrderDetails, t1 => t1.User));
        }

        [HttpGet("{id}")]
        public IActionResult IdyegoreSiparisleriGetir(int id)
        {
            return Ok(_orderService.GetById(id,t0 => t0.OrderDetails, t1 => t1.User));
        }

        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Pending));
        }
        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Confirmed));
        }

        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Cancelled));
        }


        [HttpPost]
        public IActionResult SiparisEkle(int userID, [FromQuery] int[] productIDs, [FromQuery] short[]quantities)
        {
            Order yeniSiparis=new Order();
            yeniSiparis.UserId=userID;
            yeniSiparis.Status = Status.Pending;
            yeniSiparis.IsActive = true;
            _orderService.Add(yeniSiparis);

            for (int i = 0; i < productIDs.Length; i++)
            {
                OrderDetails yeniDetay = new OrderDetails();
                yeniDetay.OrderID = yeniSiparis.ID;
                yeniDetay.ProductID=productIDs[i];
                yeniDetay.Quantity=quantities[i];
                yeniDetay.UnitPrice = _productService.GetById(productIDs[i]).UnitPrice;
                yeniDetay.IsActive=true;

                _odService.Add(yeniDetay);
            }
            return CreatedAtAction("IdyegoreSiparisleriGetir", new { id = yeniSiparis.ID }, yeniSiparis);
        }

        [HttpGet("{id}")]
        public IActionResult SiparisOnayla(int id)
        {
            Order confirmedOrder = _orderService.GetById(id);
            if (confirmedOrder==null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetails> detaylar = _odService.GetDefault(x => x.OrderID == confirmedOrder.ID).ToList();

                foreach (OrderDetails item in detaylar)
                {
                    Product siparistekiUrun = _productService.GetById(item.ProductID);
                    siparistekiUrun.Stock -= item.Quantity;
                    _productService.Update(siparistekiUrun);

                    item.IsActive = false;
                    _odService.Update(item);

                }
                confirmedOrder.Status = Status.Confirmed;
                confirmedOrder.IsActive = false;
                _orderService.Update(confirmedOrder);

                return Ok(confirmedOrder);
            }
        }



        [HttpGet("{id}")]
        public IActionResult SiparisReddet(int id)
        {
            Order reddedilenSiparis = _orderService.GetById(id);
            if (reddedilenSiparis == null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetails> detaylar = _odService.GetDefault(x => x.OrderID == reddedilenSiparis.ID).ToList();

                foreach (OrderDetails item in detaylar)
                {
                  
                    item.IsActive = false;
                    _odService.Update(item);

                }
                reddedilenSiparis.Status = Status.Cancelled;
                reddedilenSiparis.IsActive = false;
                _orderService.Update(reddedilenSiparis);

                return Ok(reddedilenSiparis);
            }
        }

       

    }
}
