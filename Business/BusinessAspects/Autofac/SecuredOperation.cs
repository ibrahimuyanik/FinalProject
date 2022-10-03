using Business.Constants;
using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        // Yetkisi varmı yokmu diye kontrol eden aspect (attribute)

        private string[] _roles; // attribute'a verilecek parametreleri yani claim'leri atadığımız değişken (admin, editör gibi)
        private IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(','); // Split() --> belirtilen karaktere göre ifadeleri ayırıp bir array'in içine atar burada , ile ayırdık

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>(); // instance'ını oluşturdu.

        }

        protected override void OnBefore(IInvocation invocation) // metod çalışmadan önce bu aspect'i çalıştır demek
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))  // bu kod bloğunda kullanıcının token'ında bizim belirtmiş olduğumuz claim'ler varmı yokmu diye kontrol ettik
                {                               // claim'ler varsa devam et dedik yoksa çalışmayı durduracak ve yetkiniz yok hatası verecek
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied); // yetki yok hatası ver dedik
        }
    }
}
