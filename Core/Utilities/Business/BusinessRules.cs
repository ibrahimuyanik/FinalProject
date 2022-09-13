using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    // İş kurallarını merkezi bir yerden yönetmek için burayı yazdık
    // Run metoduna iş kurallarını yazdığımız fonk. yollayacağız hepsi IResult döndüğü için Run metodu şart doğruysa çalışacak
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic; // bütün iş kurallarını içeren metodları gez ve kurala uymayan olursa hata döndür dedik
                }
            }
            return null;
        }
    }
}
/* Biz burada kuraldan geçmeyen metodları Business'a bildirdik geçen kurallar için bişey yapmasını söylemedik
 * 
 * Business'da eğer run metodu null dönmüşse kurallara uyulduğu anlamına gelecek
 * 
 */