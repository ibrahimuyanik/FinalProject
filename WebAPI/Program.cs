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


//builder.Services.AddSingleton<IProductService, ProductManager>(); // bizim yerimize new'ledi a��klamas� a�a��da
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