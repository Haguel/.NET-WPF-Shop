using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.User.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.User
{
    class UserProvider
    {
        private DataContext dataContext = new();

        public async Task<UserEntity> GetByEmail(string email)
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
                .Include(user => user.Cart)
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
                ConfirmationCode = createUserDto.ConfirmationCode,
            };

            CartEntity usersCart = new CartEntity();
            newUser.Cart = usersCart;

            dataContext.Users.Add(newUser);
            dataContext.Carts.Add(usersCart);
            dataContext.SaveChanges();

            return newUser;
        }

        public UserEntity Update(UpdateUserDto updateUserDto)
        {
            UserEntity user = dataContext
                    .Users
                    .FirstOrDefault(user => user.Id == updateUserDto.Id);

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.PasswordHash = updateUserDto.PasswordHash;
            user.ConfirmationCode = updateUserDto.ConfirmationCode;

            dataContext.SaveChanges();

            return user;
        }
    }
}
