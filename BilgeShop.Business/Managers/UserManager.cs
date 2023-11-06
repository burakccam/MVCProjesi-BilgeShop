using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Business.Types;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Enums;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class UserManager : IUserService
    {
        private  readonly IRepository<UserEntity> _userRepository; //Db kullanmak yerine repository kullandım çünkü repository'i tamda bunun için oluşturmuştuk.
        private  readonly IDataProtector _dataProtector;//Şifreyi veri tabanına kaydederken kriptolama işlemine yardımcı olucak Dataprotection paketini indirdim.Ve burda Dependency Injection yaptım.
        public UserManager(IRepository<UserEntity> userRepository,IDataProtectionProvider dataProtectionProvider)
        {
            _userRepository = userRepository;
            _dataProtector = dataProtectionProvider.CreateProtector("security");
                
        }


        public ServiceMessage AddUser(AddUserDto addUserDto)
        {
           var hasMail=_userRepository.GetAll(x=>x.Email.ToLower()==addUserDto.Email.ToLower()).ToList();

            if (hasMail.Any())
            {
                return new ServiceMessage()
                {
                    IsSucced = false,
                    Message="Bu email adresli bir kullanıcı zatem mevcut."
                };
            }

            var entity = new UserEntity()
            {
                FirstName = addUserDto.FirstName,
                LastName = addUserDto.LastName,
                Email = addUserDto.Email,
                Password = _dataProtector.Protect(addUserDto.Password),
                UserType = UserTypeEnum.User
            };
            _userRepository.Add(entity);
            return new ServiceMessage()
            {
                IsSucced = true
            };
        }

        public UserInfoDto LoginUser(LoginDto loginDto)
        {
            var userEntity = _userRepository.Get(x => x.Email == loginDto.Email);//Client'tan gelen veriyi var mı diye sorguladık.
            if (userEntity is null)
            {
                return null;
                //Eğer form üzerinde gönderilen email adresi ile eşleşen bir veri tabloda yoksa,oturum açılamayacağı için geriye view dönülmüyor.
                //Hata kısmını ayrı ayrı belirtmeyiz giriş yapma kısmında email veya şifreyi ayrı ayrı tutturulmasını engellemek için.
            }

            var rawPassword = _dataProtector.Unprotect(userEntity.Password);// Girilen Mail varsa kayıtlı şifreyi açıyoruz.

            if (rawPassword==loginDto.Password)
            {
                return new UserInfoDto()
                {
                    Id=userEntity.Id,
                    Email=userEntity.Email,
                    FirstName=userEntity.FirstName,
                    LastName=userEntity.LastName,
                    UserType = userEntity.UserType,

                };
            }
            else
            {
                return null;
                //Hata kısmını ayrı ayrı belirtmeyiz giriş yapma kısmında email veya şifreyi ayrı ayrı tutturulmasını engellemek için.
            }
        }
    }
}
