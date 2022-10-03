using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    public class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
/*
 * Bu metod API katmanındaki appsettings.json dosyasının içindeki SecurityKey değerini ikilik sisteme çevirmek için ve simetrik anahtar haline getirmek için
 * Böyle yapmamızın sebebi asp.net core securityKey değerlerini byte[] şeklinde istediği için yani string ifadelerle token oluşturamıyor
 * 
 * SymmetricSecurityKey --> byte[] haline dönüştürülen değeri simetrik anahtar haline getirir.
 * 
 * 
 */