using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //Null olamayan bir değer için (örnek ->decimal), null olabilir configuration'u yazıyorsanız (IsRequıred(false)) - Mutlaka değeri ? ile tanımlamalısınız.Bu değer null olabilir demiş oluyoruz orada.

        //null olabilen(string) bir değer için IsRequired(false) yazılırsa  ? koymaya gerek yoktur.->Örnek Description
        //stack'te tutulan değerler null değer alamaz.(int bool datetime vs vs).Heap'te tutlan veriler(refereance type) ise null değer alabilir.
        public decimal? UnitPrice { get; set; }
        public decimal UnitInStock { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }  //Foreign Key  category ve product tabloları arasında.



        //RELATİONAL PROPERTY
        public CategoryEntity Category { get; set; }
    }
    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(50);

            builder.Property(x=>x.Description).
                IsRequired(false);

            builder.Property(x=>x.UnitPrice)
                .IsRequired(false);

            builder.Property(x=>x.ImagePath)
                .IsRequired(false);
            base.Configure(builder);
        }
    }
}
