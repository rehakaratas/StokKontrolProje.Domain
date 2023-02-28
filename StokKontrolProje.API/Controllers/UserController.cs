using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Domain.Entities;
using StokKontrolProje.Service.Abstract;

namespace StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult TumKullanicilariGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifKullanicilariGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyegoreKullanicilariGetir(int id)
        {
            return Ok(_service.GetById(id));
        }
        [HttpPost]
        public IActionResult KullaniciEkle(User user)
        {
            _service.Add(user);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyegoreKullanicilariGetir", new { id = user.ID }, user);
        }

        [HttpPut("{id}")]
        public IActionResult KullaniciGuncelle(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }
            if (!KullaniciVarMi(id))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    _service.Update(user);
                    return Ok(User);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
            }

            //return NoContent();
        }

        private bool KullaniciVarMi(int id)
        {
            return _service.Any(user => user.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult KullaniciSil(int id)
        {
            var user = _service.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _service.Remove(user);
                return Ok("Kullanici Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult KullaniciAktiflestir(int id)
        {
            var user = _service.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _service.Activate(id);
                //return Ok("Kullanici Aktifleştirildi");
                return Ok(_service.GetById(id));
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult Login(string email,string password)
        {
            if (_service.Any(user => user.Email == email && user.Password == password))
            {
                User loggedUser = _service.GetByDefault(user => user.Email == email && user.Password == password);
                return Ok(loggedUser);
            }

            return NotFound();
        }
    }
}
