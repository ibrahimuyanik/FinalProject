using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    // İşlem başarılı ise bu class'dan yapılacak
    public class SuccessResult: Result
    {
        public SuccessResult(string message) : base(true, message)
        {
            // :base(true, message) --> Result class'ında iki durum vardı ya hem mesaj hem durum bilgi vardı burda mesajlı halini yaptık
            // :base --> demek Result class'ının constructor'ına gönder demek
        }
        public SuccessResult() : base(true)
        {
            // :base(true) --> mesajsız hali sadece işlem başarılı demek
        }
    }
}
/*
 *  :base() --> bunun anlamı base olan class'a gönder demek
 * 
 *
 */ 