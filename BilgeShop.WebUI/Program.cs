using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BilgeShopContext>(options=>options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof (SqlRepository<>));//IRepository tipinde bir newleme yap�ld��nda (Dependency Injection ile) -SqlRepository kopyas� olu�tur.
//AddScoped->Her istek i�in yeni bir kopya
//Her interface kullan�m�nda buraya gelip AddScoped a�aca��z.
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie//Authentocation i�lemleriyle alakal� yaz�lmas� gereken bir yer.
    (options =>
    {
        options.LoginPath = new PathString("/");// --> / koymam�z default olarak home'un indexine y�nlendiriyor.
        options.LogoutPath = new PathString("/");
        options.AccessDeniedPath = new PathString("/");
        //Giri�-��k��-eri�im reddi durumlar�nda anasayfaya y�nlendiriyor.
    });

builder.Services.AddDataProtection();

var app = builder.Build();

app.UseStaticFiles();//wwwroot kulland���m�z i�in ekledik.
app.UseAuthentication();  
app.UseAuthorization();
//Auth i�lemleri yap�yorsan,�stteki 2 sat�r yaz�lmal�.Yoksa hata vermez fakat oturum a�maz,yetkilendirme sorgulayamaz.

//area route-->default route'un �ncesinde yaz�lmal�.Program ilk olarak y�netim paneli var m� diye bakt�.Yoksa a�a��daki route �al��t�.
 app.MapControllerRoute(
    name:"areas",
    pattern:"{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"

    );

app.MapDefaultControllerRoute();


app.Run();
