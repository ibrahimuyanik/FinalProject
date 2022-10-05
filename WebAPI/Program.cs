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



var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // API'ye JWT kullanýlacaðýný söyledik
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


//builder.Services.AddSingleton<IProductService, ProductManager>(); // bizim yerimize new'ledi açýklamasý aþaðýda
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

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();


/*
 * 
 * 
 * !!!DÜZELTME!!!
 * ARTIK BU ÝÞLEMLERÝ AUTOFAC ÝLE YAPICAZ 12. GÜN DEÐÝÞTÝ
 * 
 *Biz önceden constructor bloðuna baðýmlýlýklarý enjekte etmiþtik
 *Mesela Manager class'larýna ilgili entity'nin Dal interface'ini enjekte etmiþtik
 *IoC Container bunu bizim yerimize yapýyor
 *16. satýrda dedik ki IProductService'e ihtiyacýn olursa ProductManager'ý new'le dedik.
 *
 *
 *IoC container bizim yerimize bellekte instance'lar oluþturur ve ihtiyaç halinde onu ilgili kod bloðunda çalýþtýrýr
 *Yani bizim yerimize productmanager class'ýný new'ler ve productmanager class'ýna ihtiyaç duyulan bir yere onun instance'ýný gönderir.
 *Biz burada API katmanýnda Controller yani bize istek yapýldýðý zaman çalýþan yapýya gönderdik
 *
 *
 *Mesela ürünleri listeleme isteði yapýldýðýnda ProductController class'ýnda Get() metodunun içinde 
 *ürünleri listeleriz ama ürünleri listeleyebilmek için ProductManager class'ýna new'lenmiþ halde ihtiyacýmýz var
 *Çünkü mew'lenmemiþ halini anlamýyor hata verir.
 *
 *
 *
 *Önceden constructor'a gönderirken new'lemeden gönderiyorduk orada hata vermiyor ama API üzerinde new'lememiz lazým.
 *
 * AddSingleton() metoduna bak
 * 
 * 
 * Ürünleri listelemek istiyoruz baðýmlýlýklarýmýz þöyle
 * 
 *  1 --> Business katmanýndaki interface(çünkü iþ kurallarýnda sonra CRUD yapýlýr kurallardan geçmezse yapma diyorduk)
 *  2 --> Business'ýn da baðýmlý olduðu DataAccess katmanýndaki interface
 *  Önceden þöyle yapýyoduk ProductManager(EfProductDal()) --> þeklinde git Ef ile ekle veya Memory ile ekle diyoduk
 *  Burda da listelerken Ef ile listele dememiz lazým 
 *  Aslýnda bize lazým olan ProductManager ve EfProductDal ama bu class'larý vermeyiz bunlarýn interface'ini veririz
 *  Zaten interface'ler bunlarýn referansýný tutuyordu.
 * 
 * 
 * 
 * 
 */