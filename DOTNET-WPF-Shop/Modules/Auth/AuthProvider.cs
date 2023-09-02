using BCrypt.Net;
using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.Main;
using DOTNET_WPF_Shop.Modules.Start;
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
        private UserProvider userProvider = new();

        public void HandleTextBoxUnfocus(TextBox textBox)
        {
            if (textBox.Text == "") textBox.Text = textBox.Tag as string;
        }

        public void HandleTextBoxFocus(TextBox textBox)
        {
            if (textBox.Text == textBox.Tag as string) textBox.Text = "";
        }

        public void RedirectToMainPage(Window view)
        {
            view.Hide();
            new Main.Main().ShowDialog();
        }

        public void Signup(SignupUserDto signupUserDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(signupUserDto.Password);

            CreateUserDto createUserDto = new()
            {
                Username = signupUserDto.Username,
                Email = signupUserDto.Email,
                PasswordHash = passwordHash
            };

            UserEntity user = userProvider.findUserByEmail(signupUserDto.Email);

            if (user != null) throw new Exception("User already exists");

            userProvider.Create(createUserDto);
        }

        public void Signin(SigninUserDto signinUserDto) 
        {
            UserEntity user = userProvider.findUserByEmail(signinUserDto.Email);

            if (user == null) throw new Exception("User doesn't exist");

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(signinUserDto.Password, user.PasswordHash);

            if(!isPasswordCorrect) throw new Exception("Incorrect password");
        }
    }
}
