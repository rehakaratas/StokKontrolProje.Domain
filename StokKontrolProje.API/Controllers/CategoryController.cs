using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.Service.Abstract;

namespace StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> _service;
        public CategoryController(IGenericService<Category> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumKategorileriGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifKategorileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyegoreKategorileriGetir(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult KategoriEkle(Category category)
        {
            _service.Add(category);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyegoreKategorileriGetir", new { id = category.ID }, category);
        }

        [HttpPut("{id}")]
        public IActionResult KategoriGuncelle(int id, Category category)
        {
            if (id!=category.ID)
            {
                return BadRequest();
            }

            try
            {
                _service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!KategoriVarMi(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }

        private bool KategoriVarMi(int id)
        {
            return _service.Any(cat => cat.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult KategoriSil(int id)
        {
            var category=_service.GetById(id);

            if (category==null)
            {
                return NotFound();
            }

            try
            {
                _service.Remove(category);
                return Ok("Kategori Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult KategoriAktiflestir(int id)
        {
            var category = _service.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            try
            {
                _service.Activate(id);
                //return Ok("Kategori Aktifleştirildi");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
