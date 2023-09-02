using DOTNET_WPF_Shop.Modules.Auth.Dto;
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
        public void findUserByEmail(string email) { }

        public void Create(CreateUserDto createUserDto)
        {
            MessageBox.Show(createUserDto.username);
            MessageBox.Show(createUserDto.email);
            MessageBox.Show(createUserDto.passwordHash);
        }
    }
}
