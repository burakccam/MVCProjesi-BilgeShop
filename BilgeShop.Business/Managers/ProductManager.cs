using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class ProductManager : IProductService
    {

        private readonly IRepository<ProductEntity> _productRepository;
        public ProductManager(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(AddProductDto addProductDto)
        {
            var entity = new ProductEntity()
            {
                Name = addProductDto.Name,
                Description = addProductDto.Description,
                UnitInStock = addProductDto.UnitInStock,
                UnitPrice = addProductDto.UnitPrice,
                CategoryId = addProductDto.CategoryId,
                ImagePath = addProductDto.ImagePath,


            };
            _productRepository.Add(entity);//Bunu ilk yazdık buraya gelince Repository'deki metotları kullancağız.

        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);

        }

        public UpdateProductDto GetProductById(int id)
        {
            var entity = _productRepository.GetById(id);

            var updateProductDto = new UpdateProductDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UnitInStock = entity.UnitInStock,
                UnitPrice = entity.UnitPrice,
                CategoryId = entity.CategoryId,
                ImagePath = entity.ImagePath,
            };
            return updateProductDto;
        }

        public List<ListProductDto> GetProducts()
        {
            var productEntities = _productRepository.GetAll().OrderBy(x => x.Category.Name).ThenBy(x => x.Name);
            //Önce kendi içinde kategorileri sırala.Ardından kategorideki ürünleri de sırala.Liste çevirmedik henüz.Tablodan çektiğim product listesini Dto'ya çevirip WewbUI'a gönderirken Listeye çevireceğiz.

            var productDtoList = productEntities.Select(x => new ListProductDto()//Tablodan çektiğim her biri veri için yeni bir ListProductDto oluşturup listeledim.
            {

                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,//Bağlı olduğu tablodan çektik.
                ImagePath = x.ImagePath,

            }).ToList();

            return productDtoList;

        }

        public List<ListProductDto> GetProductsByCategoryId(int? categoryId)
        {
            if (categoryId.HasValue)//is not null//Kategorilerden bir tanesine tıkladdıysak.
            {
                var productEntitites = _productRepository.GetAll(x => x.CategoryId == categoryId).OrderBy(x => x.Name);
                //Gönderdiğim categoryId ile CategoryId verisi eşleşenleri isimlerine göre sıralayarek getir.

                var productDtos = productEntitites.Select(x => new ListProductDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UnitInStock = x.UnitInStock,
                    UnitPrice = x.UnitPrice,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    ImagePath = x.ImagePath,

                }).ToList();
                return productDtos;
            }
            else
            {
                return GetProducts();//CategoryId ile gönderilmezse yapılacak işlemler GetProducts() ile aynı olduğu için direkt o metoda yönlendiriyorum.
            }
        }

        public void UpdateProduct(UpdateProductDto updateProductDto)
        {
            var entity = _productRepository.GetById(updateProductDto.Id);
            //Id ile eşleşen nesnenin verisini çekiyorum.
            entity.Name = updateProductDto.Name;
            entity.Description = updateProductDto.Description;
            entity.UnitPrice = updateProductDto.UnitPrice;
            entity.UnitInStock = updateProductDto.UnitInStock;
            entity.CategoryId = updateProductDto.CategoryId;
            if (updateProductDto.ImagePath != "")
            {
                entity.ImagePath = updateProductDto.ImagePath;
            }
            // Bu If'i yazmazsam, updateProductDto ile View'den gelen null olan ImagePath bilgisi, veritabanındaki görsel adresinin üzerine yazılır. Böylelikle elimizde olan görseli kaybetmiş oluruz. ISTERSEN IF'I YORUM SATIRI YAPIP BIR DENE !
            //entity.ImagePath = updateProductDto.ImagePath; -->Bunun yerine yukarda if'li koşulu yazdık.Düzenle de boş resim gönderdiğimizde hata vermemesi için
            _productRepository.Update(entity);
        }
    }
}
