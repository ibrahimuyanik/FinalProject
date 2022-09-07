using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message): base(success, message)
        {
            Data = data;
            // mesajlı hali --> base class'a durumu, mesajı ve datayı yolla dedik
        }
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
            // mesajsız hali --> sadece datayı yolla dedik
        }

        public T Data { get; }
    }
}
/* Result class'ında durum ve mesaj sistemini yamıştık
 * Hem mesaj hem durum bilgisi verebilirdik
 * Sadece durum bilgisi verebilirdik.
 * Ama burada base class'a yani Result'ın constructor'ına yolladığımız için
 * Data yı iki durumda da eşitlememiz lazım
 * Çünkü Result'ın constructor'ında data yok
 * 
 * Özetle şunu yaptık
 * Sen durum bilgisi ve mesajı Result class'ında yaptığımız gibi yap ama
 *  her iki durumda da ek olarak datayı da gönder dedik
 * 
 * 
 * 
 */