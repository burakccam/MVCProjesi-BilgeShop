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

builder.Services.AddScoped(typeof(IRepository<>), typeof (SqlRepository<>));//IRepository tipinde bir newleme yapýldðýnda (Dependency Injection ile) -SqlRepository kopyasý oluþtur.
//AddScoped->Her istek için yeni bir kopya
//Her interface kullanýmýnda buraya gelip AddScoped açacaðýz.
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie//Authentocation iþlemleriyle alakalý yazýlmasý gereken bir yer.
    (options =>
    {
        options.LoginPath = new PathString("/");// --> / koymamýz default olarak home'un indexine yönlendiriyor.
        options.LogoutPath = new PathString("/");
        options.AccessDeniedPath = new PathString("/");
        //Giriþ-çýkýþ-eriþim reddi durumlarýnda anasayfaya yönlendiriyor.
    });

builder.Services.AddDataProtection();

var app = builder.Build();

app.UseStaticFiles();//wwwroot kullandýðýmýz için ekledik.
app.UseAuthentication();  
app.UseAuthorization();
//Auth iþlemleri yapýyorsan,üstteki 2 satýr yazýlmalý.Yoksa hata vermez fakat oturum açmaz,yetkilendirme sorgulayamaz.

//area route-->default route'un öncesinde yazýlmalý.Program ilk olarak yönetim paneli var mý diye baktý.Yoksa aþaðýdaki route çalýþtý.
 app.MapControllerRoute(
    name:"areas",
    pattern:"{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"

    );

app.MapDefaultControllerRoute();


app.Run();
