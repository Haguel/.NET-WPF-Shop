using BCrypt.Net;
using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.Main;
using DOTNET_WPF_Shop.Modules.Start;
using DOTNET_WPF_Shop.Modules.User;
using DOTNET_WPF_Shop.Modules.User.Dto;
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

        public void RedirectToMainPage(Window view, Guid userId)
        {
            Main.Main mainView = new Main.Main(userId);
            mainView.Show();
            view.Hide();
        }

        public void HidePage(Window view)
        {
            view.Hide();
        }

        public UserEntity Signup(SignupUserDto signupUserDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(signupUserDto.Password);

            CreateUserDto createUserDto = new()
            {
                Username = signupUserDto.Username,
                Email = signupUserDto.Email,
                PasswordHash = passwordHash
            };

            UserEntity user = userProvider.GetByEmail(signupUserDto.Email);

            if (user != null) throw new Exception("User already exists");

            return userProvider.Create(createUserDto);
        }

        public UserEntity Signin(SigninUserDto signinUserDto) 
        {
            UserEntity user = userProvider.GetByEmail(signinUserDto.Email);

            if (user == null) throw new Exception("User doesn't exist");

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(signinUserDto.Password, user.PasswordHash);

            if(!isPasswordCorrect) throw new Exception("Incorrect password");

            return user;
        }

        public void ChangePassword(ChangePassswordDto changePasswordDto)
        {
            UserEntity user = userProvider.GetByEmail(changePasswordDto.Email);

            if (user == null) throw new Exception("User doesn't exist");

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.PasswordHash);

            if (!isPasswordCorrect) throw new Exception("Incorrect password");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            UpdateUserDto updateUserDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                PasswordHash = passwordHash
            };

            userProvider.Update(updateUserDto);
        }
    }
}
