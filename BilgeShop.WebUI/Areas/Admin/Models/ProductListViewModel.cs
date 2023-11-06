namespace BilgeShop.WebUI.Areas.Admin.Models
{
    public class ProductListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal UnitInStock { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }//Add'den farklı olarak bunu görmek istediğimiz için ekranda ekledik.Add kısmında işler farklıydı bundan

        public string ImagePath { get; set; }
    }
}
