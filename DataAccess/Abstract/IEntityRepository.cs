using Entities.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    // where T: class ---> referans tip olabilir demek
    // IEntity --> Ientity olabilir veya onu implemente eden bir class olabilir demek
    // new() ---> new'lenebilir olmalı demek
    // Bu sayede sadece varlıklarımızı buraya yazabiliriz yani sistemi korumuş olduk
    // yanlışlıkla buraya başka bir class yazılamaz.
    // Mantıken öyle olmalı çünkü biz varlıklarımızı yani ürün, kategori gibi class'ları ekleriz, sileriz, güncelleriz
    public interface IEntityRepository<T> where T : class, IEntity, new()  //generic constraint yani generic kısıtlaması 
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
// Expression<Func<T, bool>> ---> Bu ifade ile metoda parametre olarak linq sorgusu yazabiliriz demek.