using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.Service.Abstract;

namespace StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {

       
        private readonly IGenericService<OrderDetails> _odService;

        public OrderDetailsController( IGenericService<OrderDetails> odService)
        {

           
            _odService = odService;
        }

       

        [HttpGet]
        public IActionResult TumDetaylariGetir()
        {
            return Ok(_odService.GetAll(t0 => t0.Order, t1 => t1.Product));
        }


        [HttpGet("{id}")]
        public IActionResult DetaylariGetir(int id)
        {
            return Ok(_odService.GetAll(x => x.OrderID == id, t0 => t0.Product));
        }
    }
}
