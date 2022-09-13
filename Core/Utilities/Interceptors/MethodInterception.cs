using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        // invocation bizim metodlarımız Add, Update gibi. 
        // Biz metodumuzu parametre olarak veriyoruz
        // İlgili şarta göre 
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }
        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    OnSuccess(invocation);
                }
            }
            OnAfter(invocation);
        }
    }


}
/* Bütün aspect servisleri MethodInterception inherit eder
 * Çünkü bütün seçenekleri burada yazdık
 * Loglama yapılmak istenirse burdaki şartlara göre
 * Caching yapılacaksa yine buraki şartlara göre
 * Validation yapılacaksa yine aynı
 * Şartlarımızda metoddan önce, sonra, hata durumunda ve başarılı olma durumunda
 *
 * Mesela eklenen ürünleri kimin eklediği, ne zaman eklediği gibi bilgileri loglamak isteyebiliriz
 *
 *
 */