using BilgeShop.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Services
{
    public interface IProductService
    {
        void AddProduct(AddProductDto addProductDto);

        List<ListProductDto> GetProducts();
        
        List<ListProductDto> GetProductsByCategoryId(int? categoryId);//Tüm ürünlere tıklayabilme ihtimali için soru işareti koyduk.= null olabilir productviewcompenent kısmında yaptık.

        UpdateProductDto GetProductById(int id);//Categor^y'nin getcategorysi ile aynı mantık.

        void UpdateProduct(UpdateProductDto updateProductDto);

        void DeleteProduct(int id);

       

    }
}
