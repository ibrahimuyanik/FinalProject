using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    // Context : DB tabloları ile proje class'larını bağlama
    // DB'deki products tablosu ile kendi yazdığımız product class'ını bağlamak gibi diğer entiti'ler için de yapılacak
    public class NorthwindContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = Northwind; Trusted_Connection = true"); // DB bağlantısı kurduk
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }  // ----> Burda DB tablolarını kendi class'larımızla ilişkilendirdik.
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
