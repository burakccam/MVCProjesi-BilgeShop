using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Enums
{
    public enum UserTypeEnum //internal-->public    class-->enum yaptık. Sadece bizim seçtiğimiz verilerin seçilmesi için enum oluşturduk.Enumlar hem string hemde int karşılığı vardır.Ama veri tabanında int karşılığı olarak kayıt altına alınır.
    {
        User=1,  //Bool ile karışmasın diye böyle yaptık.
        Admin
    }
}
