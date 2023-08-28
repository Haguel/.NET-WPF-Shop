using BCrypt.Net;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Modules.Auth
{
    class AuthProvider
    {
        public void HandleTextBoxUnfocus(TextBox textBox)
        {
            if (textBox.Text == "") textBox.Text = textBox.Tag as string;
        }

        public void HandleTextBoxFocus(TextBox textBox)
        {
            if (textBox.Text == textBox.Tag as string) textBox.Text = "";
        }

        public void Signup(SignupUserDto signupUserDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(signupUserDto.password);

            CreateUserDto createUserDto = new()
            {
                username = signupUserDto.username,
                email = signupUserDto.email,
                passwordHash = passwordHash
            };

            new UserProvider().Create(createUserDto);
        }
    }
}
