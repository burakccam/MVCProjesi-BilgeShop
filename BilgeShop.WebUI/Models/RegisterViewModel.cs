using System.ComponentModel.DataAnnotations;

namespace BilgeShop.WebUI.Models
{
    public class RegisterViewModel//Models klasörü view ve controller arasında veri taşıdığı için klasörünü buraya açtık.
    {
        [Display(Name ="Ad")] //Display kullaabilmemiz için label asp-for'da nereye ait olduğunu belirtmemiz gerekiyordu.
        [MaxLength(25,ErrorMessage ="Ad en fazla 25 karakter uzunluğunda")]
        [Required(ErrorMessage ="Ad alanı boş bırakılamaz")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")] //Display kullaabilmemiz için label asp-for'da nereye ait olduğunu belirtmemiz gerekiyordu.
        [MaxLength(25,ErrorMessage ="Soyad en fazla 25 karakter uzunluğunda olabilir")]
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz")]

        public string LastName { get; set; }

        [Display(Name = "Eposta")] //Display kullaabilmemiz için label asp-for'da nereye ait olduğunu belirtmemiz gerekiyordu.
        [Required(ErrorMessage = "Email alanı boş bırakılamaz")]

        public string Email { get; set; }

        [Display(Name = "Şifre")] //Display kullaabilmemiz için label asp-for'da nereye ait olduğunu belirtmemiz gerekiyordu.
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]

        public string Password { get; set; }

        [Display(Name = "ŞifreTekrarı")] //Display kullaabilmemiz için label asp-for'da nereye ait olduğunu belirtmemiz gerekiyordu.
        [Required(ErrorMessage = "Şifre tekrarı alanı boş bırakılamaz")]
        [Compare(nameof(Password),ErrorMessage ="Şifreler eşleşmiyor.")]
        public string PasswordConfirm { get; set; }
    }
}
