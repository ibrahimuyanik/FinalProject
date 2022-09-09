using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Autofac;
using Business.DependencyResolvers.Autofac;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddSingleton<IProductService, ProductManager>(); // bizim yerimize new'ledi açýklamasý aþaðýda
//builder.Services.AddSingleton<IProductDal, EfProductDal>(); 



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

app.UseAuthorization();

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