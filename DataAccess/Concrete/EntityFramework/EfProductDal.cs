using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : IProductDal
    {
        public void Add(Product entity)
        {
            //IDisposable pattern implementation of c#
            // using bloğu northwind context'in işi bittikten sonra bellekten hemen silinmesini sağlar Performans için yapılır
            using (NorthwindContext context = new NorthwindContext())
            {
                var addedEntity = context.Entry(entity); // eklenecek ürünün referansını(adres) oluşturduk. Mesela 101 nolu adres gibi.
                addedEntity.State = EntityState.Added; // state durum demek, yani referansı oluştuktan sonra eklemesini söyledik
                context.SaveChanges(); // değişiklikleri kaydet dedik.
            }
        }

        public void Delete(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var deletedEntity = context.Entry(entity); 
                deletedEntity.State = EntityState.Deleted; 
                context.SaveChanges(); 
            }
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context.Set<Product>().SingleOrDefault(filter);
            }
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                // Ternary operator
                // filtre verilmemişse bütün veriyi getir. Filtre verilmişse verilen filtreye göre getir dedik.
                return filter == null ? context.Set<Product>().ToList() : context.Set<Product>().Where(filter).ToList(); 
            }
        }

        public void Update(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
