using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProje.Domain.Entities
{
    public class Supplier:BaseEntity
    {


        public Supplier()
        {
            Products=new List<Product>();
        }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        public virtual List<Product> Products { get; set; }


    }
}
