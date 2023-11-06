using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Entities
{
    public abstract class BaseEntity   //Public yapmalıyız.
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; } //Modifiad Date'in null olmasını sağladık. Default olarak gelmesini istemedik.

        public bool IsDeleted { get; set; }
    }

    //Görevi miras vermek olduğu için abstract class açtık. TEntity sadece bizim yaptığımız bir isimlendirme.Bu class'ı biz oluşturduk.

   
    //Class virtual olamaz zaten abstract olabilir.
    public abstract class BaseConfiguration<TEntity> :
        IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity//Bu class'ın sadece BaseEntity'lere sahip yapılara aşağıda yazacaklarımızı kullanalım  dedik bu satırda.
    {
       //Eğer metota abstract deseydik ezilemezdi.Ama override edilerek aynen kullanılabilirdi.
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == false); // Bu veri tabanı üzerinde yapacağımız bütün sorgularda  yukarıdaki Linq sorgusu ilave olarak geçerli olacak.Böylelikle benim silinmişleri getir şeklinde bir where kodlaması yapmam gerekmeyecek.
            builder.Property(x=>x.ModifiedDate).IsRequired(false);
            //Modified Date'in boş bırakbileceğini anlattık.
        }
        //Virtual tanımlıyorum ki derived class'lar ezilsin.Abstract tanımlasaydık ezemezdik.
        //Ama her ikisinde de override edebiliriz.
    }
}
