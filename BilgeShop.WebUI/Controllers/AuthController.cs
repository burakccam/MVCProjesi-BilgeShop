using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BilgeShop.WebUI.Controllers
{
    //Authentication-Authorization işlemleri yapılacak bu sayfada.
    //(Kimlik doğrulama-Yetkilendirme=
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Route("KayitOl")] //Bu View'a istek atıldığında Url kısmında gözükücek kısım. [HttpPost] kısmında da bu route'ı belirtmemiz gerekiyor. 
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("KayitOl")]
        public IActionResult Register(RegisterViewModel formData)
        {
            if(!ModelState.IsValid) //Gönderdiğim veri uygunmu koşullara eğer uygun değilse forma tekrar hataları verip gönderir.
            {
                return View(formData);//FormDatayı geri göndererek,formda doldurulmuş yerleri sıfırlamayı sağlıyoruz.
            }

            var addUserDto = new AddUserDto()
            {
                Email = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Password = formData.Password,   
            };

            //Buraya kadar olan süreçte kullanıcıdan gelen veriyi kontrol ettik.Yukardaki if(ModelState.IsValid kısmında onayladık.Ve gönderdik.)
            var result = _userService.AddUser(addUserDto);
            if (result.IsSucced)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View(formData);
            }
            //Kayıt işlemi tamamladndıktan sonra ana sayfaya gönderelim.
          
        }
        //Await keyword kullanılıyorsa->Yani action içerisindeki asenkron bir işlem yapılacaksa,ilgili action async Task <..> tanımlanmalıdır.
        public async Task<IActionResult> login(LoginViewModel formData)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");//Model istediğim gibi değilse AnaSayfaya yönlendir.
            }
            var loginDto = new LoginDto()
            {
                Email=formData.Email,      //Gelen veriler validation'a uygunsa buraya geldi ve Dto sayesinde business katmanına aktardım verileri.
                Password=formData.Password
            };

            var userInfo=_userService.LoginUser(loginDto);//Metodun dönüş tipini aktardık buraya.İki ihtimal var.Kişinin girdiği bilgilere göre ya null geldi.Ya da kişinin çektiğimiz bilgileri geldi.
            if(userInfo is null)
            {  //Eğer UserManager kısmında LoginUser metodunda Null olarak bir şey döndüyse  AnaSayfa'da tekrar buraya yönlendiriyorum.
                return RedirectToAction("Index", "Home");
            }

            //Buraya kadar gelebildiyse kodlar,demek ki kişinin formdan gönderdiği email ve şifre ile DB üzerindeki kayıt eşleşmiş.Gerekli bilgileri alıp veritabanından buraya kadar getirmişiz.(userInfo içerisinde). Artık oturum açılabilir.
            //Oturumda her tutacağım veriye "Claim"adını vereceğiz.
            //Bütün verilerin listesi->List<Claim>

            //ClaimList chrome içine açılıyor.Chrome içine bir şey oluşturdugumuz için asenkronizasyon kullandık.s
            var claims= new List<Claim>(); //Bu claim listesinde giriş yapan(verileri eşleşen) kullancıların verileri tutacağız.
            claims.Add(new Claim("id",userInfo.Id.ToString())); //Giriş yapan kullanıcıların idlerini "id" adlı bir klasörde-->1.parametre //Neyi tutacağız-->Giriş yapan kullancının verisi(id)
            claims.Add(new Claim("email", userInfo.Email));
            claims.Add(new Claim("firstName", userInfo.FirstName));
            claims.Add(new Claim("lastName", userInfo.LastName));
            claims.Add(new Claim("userType", userInfo.UserType.ToString()));//String olmayan verileri stringe çevirip ekliyoruz.


            /*Yetkilendirme işlemi için  özel olarak bir claim daha açılması gerekiyor.*/
            claims.Add(new Claim(ClaimTypes.Role,userInfo.UserType.ToString()));  //Yetkilendirme için userType yetkilendirmesi(rollendirme) için ayrı bir veri daha ekledik claime.

            var claimIdentity= new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);//ctrl. using ekliyoruz.//Oturum açma parçalarından bir tanesi.

            var autProperties = new AuthenticationProperties()
            {
             AllowRefresh = true,//Yenilenebilir bir oturum .Sayfayı yenilediğimizde oturum açık kalsın.Bizim sitemiz için true olabilir.
             ExpiresUtc =  new DateTimeOffset(DateTime.Now.AddHours(48)) //Oturum açtıktan sonra 48 saat sonra otomatik olarark oturum düşücek.
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), autProperties);
            //await asenkronize(Eşzamansız) yapıların birbirini bekleyerek çalışmalarını sağlıyor.(Proje-Browser)
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Oturumu kapatır.

            return RedirectToAction("Index", "Home");
        }
    }
}
