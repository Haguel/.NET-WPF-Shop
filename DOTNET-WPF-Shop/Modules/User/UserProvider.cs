using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.Main;
using DOTNET_WPF_Shop.Modules.User.Dto;
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

        public UserEntity GetByEmail(string email)
        {
            var user = dataContext
                .Users
                .FirstOrDefault(u => u.Email == email);

            return user;
        }

        public UserEntity GetById(Guid id)
        {
            var user = dataContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            return user;
        }

        public UserEntity Create(CreateUserDto createUserDto)
        {
            UserEntity newUser = new()
            {
                Id = Guid.NewGuid(),
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = createUserDto.PasswordHash,
                Cart = new CartEntity()
            };

            dataContext.Users.Add(newUser);
            dataContext.SaveChanges();

            return newUser;
        }

        public void Update(UpdateUserDto updateUserDto)
        {
            UserEntity user = dataContext
                                .Users
                                .FirstOrDefault(user => user.Id == updateUserDto.Id);

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.PasswordHash = updateUserDto.PasswordHash;

            dataContext.SaveChanges();
        }
    }
}
