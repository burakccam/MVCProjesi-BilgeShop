using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Repositories
{
    public interface IRepository<TEntity>  //IRepository'i genericleştirdik.Ve sadece class özelliğine sahip olanlar için dedik.
        where TEntity : class   //
    {
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Delete(int id);
        void Update(TEntity entity);
        TEntity GetById(int id);

        //Bu yaptığımız tamamen  bir sorgu Listelem için yaptık.Üzerine sonradan sorgu ekleme yapabilme ihtimalini düşündüğümüz için ToList() yapmadık.
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null); //Sorgu yapılacak kısımı ilgilendiren metot.
        /* Bir sql sorgusunu(linq) parametre olarak göndermek istiyorsanız bu metodu kullanıyoruz. Parametrenin tipi -->Expression<Func<TEntity, bool>> =null diyerek  bu metodun parametre alarak ya da almayarak çalışabileceğini gösteriyorum.
         Paramatre olarak bir filtreleme gönderirse o filtrelemeye uygun yapılar çekilir.
        Parametre olarak bir filtreleme gönderilmezse bütüm yapılar gelir.*/

        TEntity Get(Expression<Func<TEntity, bool>> predicate);/*Nesneyi direkt çekmek için kullancağız.*/
    }
}
