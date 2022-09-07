using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        // iki parametreli constructor
        public Result(bool success, string message):this(success) // --> burdaki success parametresini aşağıdaki constructor bloğuna gönder dedik
        {
            Message = message;
        }
        // tek parametreli constructor
        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
/*
 * Burada 2 tane constructor bloğu oluşturduk 
 * İlki işlem doğru mu ve işlem mesajı parametrelerini almak zorunda
 * İkinci ise sadece işlem doğru mu değil mi sonucunu döner
 * Biz sadece işlem sonucunu dönmek isteyebiliriz veya hem işlem sonucu hem de mesaj birlikte vermek de isteyebiliriz
 * İlk constructor da sadece mesaj işlemini eşitledik
 * İkincide constructor da sadece işlem sonucunu eşitledik
 * İlk constructor da metodun sonuna :this(success) yazdık bunun anlamı şudur:
 * Parametresi success olan constructor bloğunu da çalıştır demek
 * Çünkü mantıken işlem sonucu verilmek zorunda 
 * 
 * 
 * Böyle yazmasaydık şöyle yazmamız gerekecekti
 * İlk constructor da hem mesajı hem de işlem sonucunu metodun içinde eşitleyecektik
 * İkinci constructor da sadece işlem sonucunu eşitleyecektik
 * Bu durumda kendimizi tekrar etmiş olurduk çünkü iki constructor bloğunda da işlem sonucunu eşitleyecektik
 * 
 * Özetle burda şunu söyledik;
 * Sen istersen mesaj vererek işlem sonucunu döndürebilirsin hem de mesaj vermeden de işlem sonucunu döndürebilirsin
 * Ama iki şartta da success olan constructor bloğu çalışacak
 * Yani kendimizi tekrar etmemiş olduk iki defa işlem sonucunu eşitlemedik.
 * 
 * 
 * 
 *
 *
 */