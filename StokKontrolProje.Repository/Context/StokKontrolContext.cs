using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProje.Repository.Context
{
    public class StokKontrolContext :  DbContext
    {

        public StokKontrolContext(DbContextOptions<StokKontrolContext> options) :base(options)
        {
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-GQND9IH;Database=StokKontrolDB;Uid=sa;Pwd=1234;");
        }

        public DbSet<Category>Categories { get; set; }
        public DbSet<Order>Orders { get; set; }
        public DbSet<OrderDetails>OrderDetails { get; set; }
        public DbSet<Product>Products { get; set; }
        public DbSet<Supplier>Suppliers { get; set; }
        public DbSet<User>Users { get; set; }
        
    }
}
