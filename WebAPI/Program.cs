using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Autofac.Core;
using Core.Extensions;
using Core.DependencyResolvers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors();

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // API'ye JWT kullanılacağını söyledik
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });

builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule()
});





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddSingleton<IProductService, ProductManager>(); // bizim yerimize new'ledi açıklaması aşağıda
//builder.Services.AddSingleton<IProductDal, EfProductDal>(); 
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//ServiceTool.Create(Services);





// Autofac implementasyonu
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacBusinessModule());
    });





var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader()); // bu adresten gelen tüm istekleri kabul et dedik

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*
 * 
 * 
 * !!!DÜZELTME!!!
 * ARTIK BU İŞLEMLERİ AUTOFAC İLE YAPICAZ 12. GÜN DEĞİŞTİ
 * 
 *Biz önceden constructor bloğuna bağımlılıkları enjekte etmiştik
 *Mesela Manager class'larına ilgili entity'nin Dal interface'ini enjekte etmiştik
 *IoC Container bunu bizim yerimize yapıyor
 *16. satırda dedik ki IProductService'e ihtiyacın olursa ProductManager'ı new'le dedik.
 *
 *
 *IoC container bizim yerimize bellekte instance'lar oluşturur ve ihtiyaç halinde onu ilgili kod bloğunda çalıştırır
 *Yani bizim yerimize productmanager class'ını new'ler ve productmanager class'ına ihtiyaç duyulan bir yere onun instance'ını gönderir.
 *Biz burada API katmanında Controller yani bize istek yapıldığı zaman çalışan yapıya gönderdik
 *
 *
 *Mesela ürünleri listeleme isteği yapıldığında ProductController class'ında Get() metodunun içinde 
 *ürünleri listeleriz ama ürünleri listeleyebilmek için ProductManager class'ına new'lenmiş halde ihtiyacımız var
 *Çünkü mew'lenmemiş halini anlamıyor hata verir.
 *
 *
 *
 *Önceden constructor'a gönderirken new'lemeden gönderiyorduk orada hata vermiyor ama API üzerinde new'lememiz lazım.
 *
 * AddSingleton() metoduna bak
 * 
 * 
 * Ürünleri listelemek istiyoruz bağımlılıklarımız şöyle
 * 
 *  1 --> Business katmanındaki interface(çünkü iş kurallarında sonra CRUD yapılır kurallardan geçmezse yapma diyorduk)
 *  2 --> Business'ın da bağımlı olduğu DataAccess katmanındaki interface
 *  Önceden şöyle yapıyoduk ProductManager(EfProductDal()) --> şeklinde git Ef ile ekle veya Memory ile ekle diyoduk
 *  Burda da listelerken Ef ile listele dememiz lazım 
 *  Aslında bize lazım olan ProductManager ve EfProductDal ama bu class'ları vermeyiz bunların interface'ini veririz
 *  Zaten interface'ler bunların referansını tutuyordu.
 * 
 * 
 * 
 * 
 */