using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public IViewComponentResult Invoke(int? categoryId=null) //Tüm ürünler'e tıklandığında null olabilir demiştik.Null olabilir diyerek hatayı engelledik.
        {
            var productDtos=_productService.GetProductsByCategoryId(categoryId);

            var viewModel = productDtos.Select(x => new ProductViewModel()
            {   Id = x.Id, 
                Name = x.Name,
                UnitsInStock= Convert.ToInt32(x.UnitInStock),
                UnitPrice = x.UnitPrice,
                ImagePath = x.ImagePath,    
                CategoryId=x.CategoryId,
                CategoryName=x.CategoryName,


            }).ToList();

            return View(viewModel);
        }
    }
}
