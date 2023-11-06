using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BilgeShop.WebUI.Areas.Admin.Models
{
    public class ProductFormViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Ürün girmek zorunludur.")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Ürün Fiyatı")]
        public decimal? UnitPrice { get; set; }
        [Display(Name = "Stok Miktarı")]
        public int UnitInStock { get; set; }
       

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = " Bir kategori seçmesssk zorunludur.")]
        public int CategoryId { get; set; }//Biz kategori Id üzerinden işlemleri yapacağız kullanıcının görmek istediğini display name kısmında ayarlayacağız
        [Display(Name = "Ürün Görseli")]
        public IFormFile? File { get; set; }//Herhangi bir dosya taşırsak bunun tipi IFormFile olur.


    }
}
