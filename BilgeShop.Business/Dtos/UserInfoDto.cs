using BilgeShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Dtos
{
    public class UserInfoDto  //WebUI'dan login verilerini business'a gönderdik.Business katmanında,veriler eşleşişrse business kısmında giriş yapan kullanıcının verilerini bana yollayacak (Bu verileri sayfada göstererek gömülü Id sayesinde giriş yapan kullanıcıya ait işlemleri yapacağız.) Dto'yu oluşturdum.(Business'tan WebUI 'a transfer sağlayacak. LoginDto ve Data verileri eşleşirse UserInfoDto verileri tutacağımız yapı.
    {
        public int Id { get; set; }  //Gömülü Id üzerinden kullanıcı işlemlerini yapacağım.

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserTypeEnum  UserType { get; set; }
    }
}
