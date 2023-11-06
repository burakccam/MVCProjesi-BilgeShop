using System.ComponentModel.DataAnnotations;

namespace BilgeShop.WebUI.Areas.Admin.Models
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }//Düzenleme işlemleri yapacağımız için aldık.

        [Display(Name ="Ad")]
        [Required(ErrorMessage ="Kategori adını getirmek zorunludur.")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        //? ile validation sırasında zorunlu olmadığını belirtiyorum
        //? yazılmayan her property,defaul olarak required'dır.
    }
}
