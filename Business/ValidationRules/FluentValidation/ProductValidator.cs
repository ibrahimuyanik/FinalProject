using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator: AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName).NotEmpty(); // burada ürünün ismi boş bırakılamaz dedik
            RuleFor(p => p.ProductName).MinimumLength(2); // burada ürünün ismi minimum 2 karakter olmalıdır dedik.
            RuleFor(p => p.UnitPrice).NotEmpty();
            RuleFor(p => p.UnitPrice).GreaterThan(0); // burada ürün fiyatı 0'dan büyük olmalı dedik
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1); // 1 nolu kategorideki ürünlerin fiyatı en az 10 olmalıdır dedik.
            RuleFor(p => p.ProductName).Must(StartWidthA).WithMessage("Ürünler A harfi ile başlamalı"); 
        }

        private bool StartWidthA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}


/* Fluent Validation ile doğrulama işlemleri yapılır. 
 * Her bir entiti (ürün, kategori) için böyle class oluşturulur ve AbstractValidator<Entity> şeklinde inherit edilir.
 * AbstractValidator class'ı fluentvalidation ile gelir ve generic'tir yani hangi entiti için kural yazıldığını belirtmemiz lazım
 * Doğrulama kurallarını constructor bloğunun içine yazarız.
 *Ayrıca DTO'lar içinde validation yazabiliriz onlar için de ayrı class'lar oluşturulur
 *
 * RuleFor metodu --> kural koymak için yazılır içine lambda ifaleri ile yazarız
 * Must() --> uymalı demek yani metodun içine yazdığımız şarta uymalı anlamına gelir. Regex formatında da yazılabilir.
 * WidthMessage("Hata mesajı yaz") --> metodu ise hata alırsak bize mesaj döner eklediğimiz ürün ekleme şartlarına uymazsa mesaj verdirebiliriz.
 * FluentValidation'da hataları türkçe olarak gösterme özelliği var ama biz de hata mesajı verebiliriz
 *
 *
 *
 *
 */