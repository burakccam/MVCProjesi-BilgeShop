using BilgeShop.Data.Context;
using BilgeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Repositories
{
    public class SqlRepository<TEntity> : IRepository<TEntity> //Generic bir yapıdan miras alıcağı için kendisininde generic olmasını sağladık.
        //-->SqlRepository<TEntity>  -->Bu şekilde sağladık.
        where TEntity : BaseEntity //TEntity dediğimiz şey ProductEntity,CategoryEntity vs yerine geçer.
    {
        private readonly BilgeShopContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public SqlRepository(BilgeShopContext db)
        {
            
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity); 
            _db.SaveChanges();

        }

        public void Delete(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)//Veri tabanından veri çekeceğimiz zaman kullanırız.
        {
            return _dbSet.FirstOrDefault(predicate);  
            //TODO:First-Single-FirstOrDefault-SingleOfDefault anlatılacak.
        }


        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)// Sayfamda listeleme yapacağım zaman çekeceğim verilerin hangi kriterde olması gerektiğini  burda belirliyoruz.
        {
            return predicate is not null ? _dbSet.Where(predicate) :_dbSet;
        }

        public TEntity GetById(int id)
        {
            var entity = _dbSet.Find(id);
            return (entity);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            _db.SaveChanges();
        }
    }

   
}

/*
 
 * VERİ BULMA METOTLARI(Veri tabanından veri çekerken kullanacağız.)
 1-Find ();:ıd ile eşleşen veriyi buluyor.
 2-First();:İlk eşleşen veriyi döner,hiç veri bulmazsa hata verir.
 3-FirstOrDefault();:İlk eşleşen veriyi döner.Hiç veri bulamazsa null döner.
 4-Single ();: İlk eşleşen veriyi döner.Başka eşleşen varsa veya hiç yoksa hata ( errror )verir.
 5-SingleOrDefault ();:İlk eşleşen veriyi döner başka eşleşen varsa error verir.Hiç yoksa null döner. 

 */