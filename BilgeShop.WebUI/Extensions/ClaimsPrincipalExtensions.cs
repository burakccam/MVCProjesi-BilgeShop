using System.Security.Claims;

namespace BilgeShop.WebUI.Extensions
{
    //Cookie'de tutulan verilerin kontrollerini yapmak için yazılacak metotları bu class içinde topluyorum.
    public static class ClaimsPrincipalExtensions//UserExtensions  Extension(sonradan eklemeli metot)(Static demek newlemeye ihtiyaç duymuyoruz.)Burdaki static class'ın interfaceden farkı metotun tüm detaylarını içeriğini vs burda dolduruyoruz.
    {
        //this-->Bu sayede artık metot sondan çağırılır.
        //User.IsLogged() tarzında
        public static bool IsLogged(  this ClaimsPrincipal user)//Layouttaki User(.netten gelen) parametre olarak burdaki userla eşleşiyor.
        {
            if (user.Claims.FirstOrDefault(x=>x.Type=="id")!=null)
                return true;       
            else
                return false;
            
        }
        public static string GetUserFirstName(this ClaimsPrincipal user)//Layouttaki User(.netten gelen) parametre olarak burdaki userla eşleşiyor.
        {
            return user.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value;//.value diyerek değerine ulaştık.String bir ifade olduğu için soru işareti koyduk.Null olabilir dedik yoksa hata alırdık.
        }

        public static string GetUserLastName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value;
        }
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            if (user.Claims.FirstOrDefault(x => x.Type == "userType")?.Value == "Admin")
                return true;
            else
                return false;
        }
    }
}
