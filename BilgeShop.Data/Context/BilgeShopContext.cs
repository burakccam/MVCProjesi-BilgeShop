using BilgeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Context
{
    public class BilgeShopContext:DbContext
    {
        public BilgeShopContext(DbContextOptions<BilgeShopContext>options):base(options)//Bağlantı için açtık.3 büyükler.
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//FLUENT API->C# tarafındaki  classların sql tablolarına dönüştürürken özelliklerine yaptığım biçimlendirmeler.
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UserEntity> Users =>Set<UserEntity>();//Prop tab tab şeklinde yap.Public Unutma.
        public DbSet<ProductEntity> Products =>Set<ProductEntity>();

        public DbSet<CategoryEntity> Categories =>Set<CategoryEntity>();
    }
}
