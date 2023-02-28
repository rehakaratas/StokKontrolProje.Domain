using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.Service.Abstract;

namespace StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult TumUrunleriGetir()
        {
            return Ok(_service.GetAll(t0=>t0.Category,t1=>t1.Supplier));
        }

        [HttpGet]
        public IActionResult AktifUrunleriGetir()
        {
            return Ok(_service.GetActive(t0 => t0.Category, t1 => t1.Supplier));
        }

        [HttpGet("{id}")]
        public IActionResult IdyegoreUrunleriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult UrunEkle(Product product)
        {
            _service.Add(product);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyegoreUrunleriGetir", new { id = product.ID }, product);
        }


        [HttpGet("{id}")]
        public IActionResult TedarikciyeGoreUrunleriGetir(int id)
        {
            return Ok(_service.GetAll(x => x.SupplierID == id, t0 => t0.Supplier!, t1 => t1.Category!));
        }

        [HttpPost("{id}")]
        public IActionResult UrunGuncelle(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }
            if (!UrunVarMi(id))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _service.Update(product);
                    return Ok(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
            }

            //return NoContent();
        }

        private bool UrunVarMi(int id)
        {
            return _service.Any(pro => pro.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            var product = _service.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _service.Remove(product);
                return Ok("Urun Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult UrunAktiflestir(int id)
        {
            var product = _service.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _service.Activate(id);
                //return Ok("Urun Aktifleştirildi");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

    }
}
