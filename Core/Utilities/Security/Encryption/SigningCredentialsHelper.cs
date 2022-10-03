using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        }
    }
}

/* 
 * Burada Token'ların yönetiminin nasıl yapılacağını belirttik 
 * Yani securityKey değerini verdik ve hangi algoritmayı kullanması gerektiğini belirttik
 * securityKey API katmanında yazdığımız değer
 * 
 * Token  oluştururken API katmanındaki securityKey değerinin byte[] halini kullanarak ve
 * HmacSha512Signature algoritmasını kullanarak token oluşturulacağını belirttik
 * 
 * 
 * 
 */
