using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProje.Domain.Entities
{
    public class Product:BaseEntity
    {
        public Product()
        {
            OrderDetails = new List<OrderDetails>();
        }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short? Stock { get; set; }
        public DateTime? ExpireDate { get; set; }


        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public virtual Category? Category { get; set; }


        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }

        public virtual Supplier? Supplier { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }

    }
}
