using StokKontrolProje.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProje.Domain.Entities
{
    public class User : BaseEntity
    {

        public User()
        {
            Orders=new List<Order>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoURL { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }

        [ForeignKey("Supplier")]
        public int? CompanyID { get; set; }

        public virtual List<Order> Orders { get; set; }
        public virtual Supplier Supplier { get; set; }

    }
}
