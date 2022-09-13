using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)  // FluentValidation ile yazılmış olan validator tipinde parametre yolladık
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType)) // Burda kontrolünü yaptık gerçekten bir validation class'ımı değil mi diye
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil"); // değilse hata mesajı 
            }

            _validatorType = validatorType; // validator ise kendi değişkenimize atadık.
        }
        protected override void OnBefore(IInvocation invocation)  // Metod çalışmadan önce validation yapmak için bu metod
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType); // Validator'un çalışma zamanında instance'ını oluştur dedik
            var entityType = _validatorType.BaseType.GetGenericArguments()[0]; // Validator'un hangi tiple çalıştığına bak ve ilk tipi getir dedik (ürün , kategori için validation yazılır.)
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType); // Business'da CRUD işlemleri yapılırken verdiğimiz parametreleri bir dizi haline getirdik. Ürün eklerken ürün adı, fiyatı gibi bilgiler
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity); // Burada da her bir yollanan değerin validation kurallarına uyup uymadığını kontrol ettik
            }
        }
    }
}
