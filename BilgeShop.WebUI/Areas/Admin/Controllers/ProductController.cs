using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]//progra.cs'teki  area:exists kısmı ile eşleşir.
    [Authorize(Roles = "Admin")]// Bu kısım Claimlerdeki  
                                //claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString())); 
                                //kısmı ile eşleşiyor.
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly ICategoryService _categoryService;//Ürünleri güncellerken ya da oluştururken Kategori tablosundaki kategorileri kullanacağımız için kategorileri çağırmak için categoryservice'deki metotlara ihtiyaç duyacağız.
        private readonly IWebHostEnvironment _environment;//wwwroot yolunu yakalamak için kullanacağız.(Her bilgisayarda farklı olanı yakalamak için,çekmek için)
        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;

        }

        public IActionResult List()
        {
            var productDtoList=_productService.GetProducts();

            var viewModel = productDtoList.Select(x => new ProductListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock,
                ImagePath = x.ImagePath,
            }).ToList();

            //En son Listeleme kısmına göndereceğimiz viewmodeli(listeyi) gönderdim.List View'inde de @model List<ProductListViewModel> bu tanımlamayı yapabiliriz.
            return View(viewModel);
        }

        public IActionResult New()
        {
            ViewBag.Categories = _categoryService.GetCategories();//Select kısmına göndereceğimiz verileri çekti.
            return View("Form", new ProductFormViewModel());//İlk olarak bunu yazdık.Hem güncelleme hem ekleme aynı formdan olacağı için ilk bunu yazdık.Sonra da form view'i oluşturduk.//EKLEME VE GÜNCELLEME AYNI FORM ÜZERİNDEN YAPILACAKSA ,BOŞ MODEL İLE AÇMAYI UNUTMA-YOKSA FORMDAN ID-->NULL GÖNDERİLİYOR SANIP VALIDATION'A TAKILIR.
        }

        [HttpGet]
        public IActionResult Update(int id)//Öncelikle bu metodun içini doldurmadan servicedeki metodu oluşturduk ve manager kısmında tablodan çektiğimiz veriyi geri gönderdik buraya.
        {
            var updateProductDto=_productService.GetProductById(id);//Tablodaki veriyi business'ta çekip dtoyla buraya göndermiştik.Onu çektik.

            var viewModel = new ProductFormViewModel()//Forma dolu veri göndereceğim için viewmodel oluşturup verileri gönderdim
            {
                Id=updateProductDto.Id,
                Name=updateProductDto.Name,
                Description=updateProductDto.Description,
                CategoryId=updateProductDto.CategoryId,
                UnitInStock=Convert.ToInt32(updateProductDto.UnitInStock),
                UnitPrice=updateProductDto.UnitPrice,
            };

            ViewBag.ImagePath=updateProductDto.ImagePath;

            ViewBag.Categories = _categoryService.GetCategories();//Kategorileri birdaha çektik.Formda kategorileri çekme listesi var.Oranın dolu gelmesi için göndermem lazım.
            return View("Form", viewModel);
        }
        public IActionResult Save(ProductFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Categories = _categoryService.GetCategories();//Select kısmına göndereceğimiz verileri çekti.
                return View("Form", formData);//Yukardaki satırı yazmamızın sebebi View her açıldığında eğer yeni kategori eklenmişsse onunda çekilmesini istemek.
            }
            var newFileName = "";
            if (formData.File is not null)//Resim ekleme kısmı boş değil ise dedik.Yani resim eklemek istiyorsa.!!!!formData.File dedik önemli!!
            {
                var allowedFileTypes = new string[] { "image/jpeg", "image,jpg", "image,pnp", "image/jfif" };//izin verdiğim dosya tiplerini liste oluşturup onda depoladım.
                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif" };//İzin verdiğim dosya uzantılarını listeye attım.
                var fileContentType = formData.File.ContentType;//dosyanın içeriğini çektim. image/jpg
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formData.File.FileName);// Dosyanın uzantısız halini hazır olarak path kütüphanesinde çeken bir metotlar birlikte çektim.profilfoto.jpeg-- > Sadece profilfoto yazısını çektim.
                var fileExtension = Path.GetExtension(formData.File.FileName);//Sadece uzantıyı çektik bu sefer.


                if (!allowedFileExtensions.Contains(fileExtension) || !allowedFileTypes.Contains(fileContentType))//Burda izin verdiğim kısıtlamalardan bir tanesini  bile içermiyorsa if'in içine gir  dedik.
                {
                    ViewBag.ErrorMessage = "Eklenmek istenen dosya uygun formatta değil.";
                    ViewBag.Categories = _categoryService.GetCategories();//Eğer resim doğru formatta değilse form sayfasına girdiği eski bilgilerle beraber tekrar gönderdik.
                    return View("Form", formData);
                }//Burdan sonra wwwroot oluşturup ilgili dependecy Injection'u yaptık.

                newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid() + fileExtension;
                // Aynı isimde iki dosya yüklendiğinde hata vermesin diye, birbiriyle asla eşleşmeyecek şekilde her dosya adına unique bir metin ilavesi (guid) yapılır.
                //Burdan sonra wwwroot oluşturup ilgili dependecy Injection'u yaptık.
                /* ********************************/
                //AŞAĞIDAKİ KISIMDA DEPENDENCY INJECTİON SAYESİNDE KULLANDIĞIMIZ METOTLARLA RESİMİ ÇEKTİK VE YÜKLENMESİ GEREKEN YERE BELİRTTİK.

                var folderPath = Path.Combine("images", "products");
                //images/products

                var wwwrootFolderPath = Path.Combine(_environment.WebRootPath, folderPath);
                //.../wwwroot/images/products
                //_environment.WebRootPath-->www.root'a kadar olan kısım.-->Root dahil

                var wwwrootFilePath = Path.Combine(wwwrootFolderPath, newFileName);
                // .../wwwroot/images/products/urunGorseli---1325321331

                Directory.CreateDirectory(wwwrootFolderPath);// Eğer images/product klaasörleri yoksa oluştur.Var mı yok mu diye bakıyor metot.

                using (var filestream = new FileStream(wwwrootFilePath, FileMode.Create))
                {
                    /*using (var filestream = new FileStream(wwwrootFilePath, FileMode.Create))
                     Bu satırda, wwwrootFilePath'i içeren dosyayı oluşturarak bir FileStream nesnesi oluşturulur. Bu dosya, yüklenen verileri almak için kullanılacaktır.*/

                    formData.File.CopyTo(filestream);
                    //asıl dosya kopyalamanın yapıldığı kısım
                    

                    /*  formData.File.CopyTo(filestream);
                      Bu satırda, formData adlı bir nesnenin içinde bulunan File özelliği (muhtemelen bir IFormFile türünde)oluşturulan filestream'e kopyalanır. Bu işlem dosyanın sunucuya yüklenmesini gerçekleştirir.*/   /*  BU KISIMDA YÜKLENEN DOSYALARI VERİ TABANINDA TUTMAK İÇİN YAZDIĞIMIZ KODLAR.*/ 
                }

            }

            if (formData.Id==0)//EKLEME -->Ekleme için AddProoductDto açacağız. //Id'ye bakarak ekleme ya da güncelleme olduğunu anlamamız ortak formdan kaynaklı.
            {
                var addProductDto = new AddProductDto()
                {
                    Name= formData.Name.Trim(),
                    Description=formData.Description,//Boş gelme ihtimali olduğu için trimlemedik
                    UnitPrice=formData.UnitPrice,//Viewmodel'deki ve AddProductDto kısmındaki decimallerin ikisinede soru işareti gelmesi ya da gelmemesi gerekir.Hataya sebep verir.
                    UnitInStock=formData.UnitInStock,
                    CategoryId=formData.CategoryId,
                    ImagePath=newFileName
                };
                _productService.AddProduct(addProductDto);//Eklemesi için Service'deki metoda yolladım.Her türlü ekleme yapacağımız için direkt list'e döndük.Metodun dönüş tipine bakmadık.(Aynı isimde üründen var mı vs diye)
                return RedirectToAction("List");
            }
            else//GÜNCELLEME
            {
                var updateProductDto = new UpdateProductDto()
                {
                    Id= formData.Id,
                    Name= formData.Name,
                    Description=formData.Description,
                    UnitPrice=formData.UnitPrice,
                    UnitInStock=formData.UnitInStock,
                    CategoryId=formData.CategoryId,
                    ImagePath = newFileName
                };
                _productService.UpdateProduct(updateProductDto);
                return RedirectToAction("List");
            }
           

        }

        
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("List");
        }
    }
}
