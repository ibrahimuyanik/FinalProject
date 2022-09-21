using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            // 1.Kural Bir kategoride en fazla 10 ürün olabilir.
            //var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count; // Gönderdiğimiz ürünün kategorisindeki ürünlerin sayısını bulduk
            //if (result >= 10)
            //{
            //    return new ErrorResult(Messages.ProductCountOfCategoryError);
            //}

            // Bu yanlış yazım şeklidir çünkü aynı kural güncelleme yaparkende kullanılacak kuralımız bir kategoride en fazla 10 ürün olabilirdi
            // Bunu metod haline getirip yazmalıyız bu sayede update'in içinde de metodu kullanabiliriz Böylelikle kendimiz tekrar etmekten kurtulduk

            //2. Kural Bütün ürünlerin ismi farklı olmalı

            IResult result =  BusinessRules.Run( // run metodu bize bir sonuç döndüreceği için onu değişkene atadık. Hangi kuralın metodu hata döndürürse result değişkeninin değeri o parametre olur.
                CheckIfProductCountOfCategoryCorrect(product.CategoryId), //kural 1
                CheckIfProductNameExists(product.ProductName), // kural 2
                CheckIfCategoryLimitExceded() // kural 3
                );

            if (result != null)
            {
                return result; // Run metodu null değilse hata var anlamına gelir hatayı döndür dedik hata dönüşü olarak da yolladığımız hangi iş kuralı metodu hatalı ise onu gönderecek.            }
            }

            //Hata yoksa yani iş kurallarından geçtiyse ekle dedik.
            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            // İş kodları olduğunu düşünelim
            // Listeleme için yetkisi var mı gibi şartları kontrol edebiliriz

            if (DateTime.Now.Hour == 05)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max) // belli bir fiyat aralığına göre ürünleri getirir.
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            throw new NotImplementedException();
        }


        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {

            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count; // Gönderdiğimiz ürünün kategorisindeki ürünlerin sayısını bulduk
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any(); // Any metodu şarta uyan veri varsa true yoksa false döner

            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();

        }
        private IResult CheckIfCategoryLimitExceded() // 3. kural eğer 15'den fazla kategori varsa yeni ürün eklenemez
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();
        }
    }
}
