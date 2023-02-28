using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.Service.Abstract;

namespace StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult TumTedarikcileriGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifTedarikcileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyegoreTedarikcileriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult TedarikciEkle(Supplier supplier)
        {
            _service.Add(supplier);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyegoreTedarikcileriGetir", new { id = supplier.ID }, supplier);
        }

        [HttpPut("{id}")]
        public IActionResult TedarikciGuncelle(int id, Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return BadRequest();
            }
            if (!TedarikciVarMi(id))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _service.Update(supplier);
                    return Ok(supplier);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
            }
            
            //return NoContent();
        }

        private bool TedarikciVarMi(int id)
        {
            return _service.Any(sup => sup.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult TedarikciSil(int id)
        {
            var supplier = _service.GetById(id);

            if (supplier == null)
            {
                return NotFound();
            }

            try
            {
                _service.Remove(supplier);
                return Ok("Tedarikci Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult TedarikciAktiflestir(int id)
        {
            var supplier = _service.GetById(id);

            if (supplier == null)
            {
                return NotFound();
            }

            try
            {
                _service.Activate(id);
                //return Ok("Tedarikci Aktifleştirildi");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
