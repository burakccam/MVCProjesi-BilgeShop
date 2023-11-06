using BilgeShop.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Services
{
    public interface ICategoryService
    {
        bool AddCategory(AddCategoryDto addCategoryDto);//Daha önce eklenmiş mi diye kontrol edip ekleyeceğiz.

        List<ListCategoryDto> GetCategories();

        void UpdateCategory(UpdateCategoryDto updateCategoryDto);

        UpdateCategoryDto GetCategory(int id);
        void DeleteCategory(int id);
    }
}
