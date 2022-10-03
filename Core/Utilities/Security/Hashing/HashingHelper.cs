using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        // Gönderilen password'u hash'leme ve salt'lama işlemlerini yapacak olan metod
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) // gönderilen parolaları hash ve salt işlemlerini yapacak metod
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) // hash ve saltlamayı yapacak olan algoritma .NET'in kendi class'ı
            {
                passwordSalt = hmac.Key; // her kullanıcı için bir değer oluşturur
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        //Password hash'ini doğrulayacak metod
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            
        }
    }
}


/*
 * Encoding.UTF8.GetBytes ---> string bir ifadenin ikilik sistemdeki karşılığını verir
 * ComputeHash --> metodu ikilik sistemde verilen değerleri hash'leyebildiği için password'u ikilik sistemdeki karşılığına çevirerek gönderdik.
 *
 * VerifyPasswordHash --> metodu ile sisteme giriş yapmak isteyen kullanıcının
 * girdiği parolayı hash'ledik ve sistemde kayıtlı olan diğer hash'ler ile karşılaştırdık
 * Eşleşirse true döner eşleşmezse false döner
 *
 * CreatePasswordHash metodundaki out ile yazılmış parametreler passwordHash ve passwordSalt değerleri
 * dışarıdan ulaşılacak değerlerdir yani bu metodu çalıştırırken sadece string bir password yollayacağız
 * Yolladığımız password'ün hash ve salt değerlerini out keyword'ü sayesinde dışarıdan erişip kullanabilecez.
 *
 * passwordSalt = hmac.Key; --> burda salt değeri olarak kullandığımız algoritmanın Key değerini salt değerimiz yaptık.
 * 
 * HMACSHA512 algoritmasının Key değeri bizim sistemimizdeki kullanıcıların salt değeri olmuş oldu.
 * Algoritma her çalıştırıldığında farklı bir Key değeri oluşturur bu sayede her kullanıcının salt değeri farklı olmuş olur
 *
 * VerifyPasswordHash metodunda algoritmaya parametre olarak salt değerini verdik. Bunun anlamı
 * Verdiğimiz salt değerine göre password'ü hash'le ve hash değeri ile karşılaştır dedik.
 *
 *
 *
 *
 */