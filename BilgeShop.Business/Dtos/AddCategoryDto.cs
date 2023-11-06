using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Dtos
{
    public class AddCategoryDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        //Id'ye gerek yok.Çünkü yeni eklenecek bir verinn Id'si 0 olarak gelir ilk başta.
    }
}
