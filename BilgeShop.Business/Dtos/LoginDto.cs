using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Dtos
{
    public class LoginDto//WebUI katmanına viewModel'le gelen verileri , Dto ile Business katmanına taşıyacağız.s
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
