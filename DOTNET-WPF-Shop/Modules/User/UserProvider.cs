using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DOTNET_WPF_Shop.Modules.User
{
    class UserProvider
    {
        private DataContext dataContext;

        public UserProvider() { dataContext = new(); }

        public UserEntity findUserByEmail(string email)
        {
            var user = dataContext.Users
                .FirstOrDefault(u => u.Email == email);

            return user;
        }


        public void Create(CreateUserDto createUserDto)
        {
            UserEntity newUser = new()
            {
                Id = Guid.NewGuid(),
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = createUserDto.PasswordHash
            };

            dataContext.Users.Add(newUser);
            dataContext.SaveChanges();
        }
    }
}
