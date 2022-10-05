using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key); // cache'den bir key değerine karşılık gelen veriyi getirir.
        object Get(string key);// yukarıdakinin farklı bir yazım şekli ikiside aynı işi yapar.
        void Add(string key, object value, int duration); // cache'e veri ekleyecek metod parametreleri sırayla verinin cache'deki karşılığı, veri, verinin tutulma süresi
        bool IsAdd(string key); // Verilen key değeri cache'de varmı yokmu diye kontrol eden metod 
        void Remove(string key); // cache'den verileri silmek için
        void RemoveByPattern(string pattern); // Parametre almayan metodlardan gelen verileri silmek için mesela getall gibi. parametre olarak Regex kullanılacak
    }
}

/* Biz projemizde .Net'in içinde var olan cache yapısını kullanıcaz
 * ama yarın başka bir cache yapısını kullanmak istersek diye interface'ini oluşturduk.
 * Veriler cache'de key, value şeklinde tutulur.
 * Örnek --> product.getall, productlistesi gibi.
 * product.getall --> key değeridir
 * productlistesi --> key değerinin karşılığıdır yani birisi product.getall isterse ona productlistesi verileri iletilir.
 *
 *
 */