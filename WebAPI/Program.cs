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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // API'ye JWT kullan�laca��n� s�yledik
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


//builder.Services.AddSingleton<IProductService, ProductManager>(); // bizim yerimize new'ledi a��klamas� a�a��da
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
 * !!!D�ZELTME!!!
 * ARTIK BU ��LEMLER� AUTOFAC �LE YAPICAZ 12. G�N DE���T�
 * 
 *Biz �nceden constructor blo�una ba��ml�l�klar� enjekte etmi�tik
 *Mesela Manager class'lar�na ilgili entity'nin Dal interface'ini enjekte etmi�tik
 *IoC Container bunu bizim yerimize yap�yor
 *16. sat�rda dedik ki IProductService'e ihtiyac�n olursa ProductManager'� new'le dedik.
 *
 *
 *IoC container bizim yerimize bellekte instance'lar olu�turur ve ihtiya� halinde onu ilgili kod blo�unda �al��t�r�r
 *Yani bizim yerimize productmanager class'�n� new'ler ve productmanager class'�na ihtiya� duyulan bir yere onun instance'�n� g�nderir.
 *Biz burada API katman�nda Controller yani bize istek yap�ld��� zaman �al��an yap�ya g�nderdik
 *
 *
 *Mesela �r�nleri listeleme iste�i yap�ld���nda ProductController class'�nda Get() metodunun i�inde 
 *�r�nleri listeleriz ama �r�nleri listeleyebilmek i�in ProductManager class'�na new'lenmi� halde ihtiyac�m�z var
 *��nk� mew'lenmemi� halini anlam�yor hata verir.
 *
 *
 *
 *�nceden constructor'a g�nderirken new'lemeden g�nderiyorduk orada hata vermiyor ama API �zerinde new'lememiz laz�m.
 *
 * AddSingleton() metoduna bak
 * 
 * 
 * �r�nleri listelemek istiyoruz ba��ml�l�klar�m�z ��yle
 * 
 *  1 --> Business katman�ndaki interface(��nk� i� kurallar�nda sonra CRUD yap�l�r kurallardan ge�mezse yapma diyorduk)
 *  2 --> Business'�n da ba��ml� oldu�u DataAccess katman�ndaki interface
 *  �nceden ��yle yap�yoduk ProductManager(EfProductDal()) --> �eklinde git Ef ile ekle veya Memory ile ekle diyoduk
 *  Burda da listelerken Ef ile listele dememiz laz�m 
 *  Asl�nda bize laz�m olan ProductManager ve EfProductDal ama bu class'lar� vermeyiz bunlar�n interface'ini veririz
 *  Zaten interface'ler bunlar�n referans�n� tutuyordu.
 * 
 * 
 * 
 * 
 */