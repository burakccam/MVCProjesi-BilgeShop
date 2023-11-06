using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.ViewComponents
{
    //CategoriesViewCompenent bir controller gibi  düşünülebilir.
    public class CategoriesViewComponent: ViewComponent //Açıcacığım componentler burdan miras almalı.
    {
        //--Business'taki metotları kullancağımız için dependecy Injection yaptık.
        private readonly ICategoryService _categorysService;
        //Default view'ini besleyen burdaki actionlar ve componentler.
        public CategoriesViewComponent(ICategoryService categoryService)
        {
            _categorysService = categoryService;
        }

        //Invoke bir action gibi düşünülebilir.
        public IViewComponentResult Invoke()
        {
            var categoryDtos=_categorysService.GetCategories();
            var viewModel = categoryDtos.Select(x => new CategoryViewModel()
            {
                Id=x.Id,
                Name=x.Name,
            }).ToList(); //Otomatik olarak eşleştiği için Default view'ini parantez içine yazmamıza gerek yok.
            return View(viewModel);//Burdaki View'in açılacağı yeri shared 'a Components klasörü  açıypruz.Ardından içine burdaki class'ın adında bir klasör daha açıp içine de Default adında bir view açıyoruz.Açtığımız bütün yapıları Compenentle alakalı doğru isimlendirmede açmalıyız.
        }
    }
}
