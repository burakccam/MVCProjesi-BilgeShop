using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]//progra.cs'teki  area:exists kısmı ile eşleşir.
    [Authorize(Roles = "Admin")]// Bu kısım Claimlerdeki  
    //claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString())); 
    //kısmı ile eşleşiyor.
    public class CategoryController : Controller
    {//Controller Servicedeki metotları kullanacağı için burda dependency Injection kullandık.
        private readonly ICategoryService _categoryService;

        public CategoryController( ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult List()
        {
            var categoryDtoList = _categoryService.GetCategories();
            var viewModel=categoryDtoList.Select(x=> new CategoryListViewModel()
            {
                Id=x.Id,
                Name=x.Name,

            }).ToList();
            return View(viewModel);
        }

        [HttpGet]//link/url'den tetiklenir.
        public IActionResult New()
        {
            return View("Form",new CategoryFormViewModel());
        }//Eğer ekleme ve güncelleme için aynı formu kullanacaksanız.Formu ekleme kısmında boş bir model ile açtım.

        [HttpPost]//formdan tetiklenir
        public IActionResult Save(CategoryFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", formData);
            }

            if (formData.Id == 0)//Ekleme işlemi //Dışarıdan gelen veriyi ilk olarak 0 olarak algılar sonra ekler ve sql ıd'sini atar.
            {
                var addCategoryDto = new AddCategoryDto()
                {
                    Name = formData.Name.Trim(),
                    Description = formData.Description
                };//Business katmanına managera gönderdik veriyi.
                //Gönderdiğimizde metodun sonucunu aşağıdaki değişkene aktardık.
                var result=_categoryService.AddCategory(addCategoryDto);//Formdan aldığım veriyi service kısmına gönderdim.
                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu isimde bir kategori zaten mevcut.";
                    return View("Form", formData);//Form actionuna formData gönderdik.

                }
               
            }

            else//Güncelleme işlemi
            {
                var updateCategoryDto = new UpdateCategoryDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description
                };
                _categoryService.UpdateCategory(updateCategoryDto);
                return RedirectToAction("List");
            }





        }

        [HttpGet]
         public IActionResult Update(int id)
        {
            var entity=_categoryService.GetCategory(id);

            var viewModel = new CategoryFormViewModel()
            {
                Name=entity.Name,
                Description=entity.Description,
                Id=entity.Id
            };
            return View("Form",viewModel);
        }

        
        public IActionResult Delete(int id)
        {
            _categoryService.DeleteCategory(id);
            return RedirectToAction("List");
        }
    }
}
