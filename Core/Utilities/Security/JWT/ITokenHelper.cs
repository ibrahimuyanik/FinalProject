using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims); // Kullanıcı için Token oluşturacak metod
    }
}
/* Interface oluşturmamızın sebebi belki JWT yerine başka bir teknoloji kullanarak token oluşturmak istersek diye
 * veya unit test yazılacaksa bu interface'i kullanarak token oluşturup test yapılabilir
 * 
 * CreateToken metodu bize token oluşturacak metoddur.
 * Parametre olarak kullanıcı bilgisi ve kullanıcının rollerini alarak token oluşturulur
 * 
 * Özetle bu metod sisteme giriş yapan bir kullanıcının bilgilerine ve claim'lerine bakarak ona bir token üretecek
 * biz bu token bilgisini, token bilgilerini tuttuğumuz AccessToken class'ına atayacağız.
 * 
 * 
 * 
 */