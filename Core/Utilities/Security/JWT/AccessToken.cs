using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
/*
 * Kullanıcı bir işlem yapacağı zaman token bilgisini göndererek işlem yapabilir.
 * 
 * Kullanıcının istek yaparken  göndereceği token bilgisini ve token'ın bitiş süresini burada tutarız
 *
 */