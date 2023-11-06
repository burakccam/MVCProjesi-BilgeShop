using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]//progra.cs'teki  area:exists kısmı ile eşleşir.
    [Authorize(Roles ="Admin")]// Bu kısım Claimlerdeki  
    //claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString())); 
    //kısmı ile eşleşiyor.
  
    //Yukarıda yazdığım authorize sayesinde,yetkisi admin olmayan kişiler buraya istek atmaya çalıştığında accessDenied veriyoruz.Ve istediğimiz yere yönlendiriyoruz.(Program cs kısmında belirttik bunu)BURASI ÖNEMLİ!!!
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
