using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.CCS;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule: Module
    {
        protected override void Load(ContainerBuilder builder) // uygulama çalıştığında burası çalışacak
        {
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance(); // biri senden IProductService isterse ona ProductManager ver dedik.
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector() // burda yukarıdaki new'lenen class'ların aspect'leri varmı diye kontrol ettiriyoruz
                }).SingleInstance();



        }
    }
}


/*
 *  Önceden API katmanında Ioc container ile yaptığımız instance oluşturmalarını artık autofac ile yapıcaz
 *  Artık API katmanında değil business'da yapıcaz çünkü başka api'ler veya service'lerinde instance oluşturmaya ihtiyacı olduğu için
 *  API'de yaparsak sadece o API'de çalışır ama Business'da yaparsak her API'de veya service'de çalışır.
 *  
 *  
 *  builder.RegisterType API katmanındaki Singleton metoduna karşılık gelir. Yani interface'in hangi class'a karşılık geleceğini burada belirtiriz.
 *  Örnek IProductService --> ProductManager' a karşılık gelir gibi.
 *  
 *  
 * 
 */