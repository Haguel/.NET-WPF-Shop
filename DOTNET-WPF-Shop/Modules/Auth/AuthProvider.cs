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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DOTNET_WPF_Shop.Modules.Auth
{
    class AuthProvider
    {
        private UserProvider userProvider = new();

        public void RedirectToMainPage(Window view, Guid userId, String username)
        {
            Main.Main mainView = new Main.Main(userId, username);
            mainView.Show();
            view.Close();
        }

        public void HidePage(Window view)
        {
            new Start.Start().Show();
            view.Close();
        }

        public async Task<UserEntity> Signup(SignupUserDto signupUserDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(signupUserDto.Password);

            CreateUserDto createUserDto = new()
            {
                Username = signupUserDto.Username,
                Email = signupUserDto.Email,
                PasswordHash = passwordHash
            };

            UserEntity user = await userProvider.GetByEmail(signupUserDto.Email);

            if (user != null) throw new Exception("User already exists");

            return userProvider.Create(createUserDto);
        }

        public async Task<UserEntity> Signin(SigninUserDto signinUserDto) 
        {
            UserEntity user = await userProvider.GetByEmail(signinUserDto.Email);

            if (user == null) throw new Exception("User doesn't exist");

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(signinUserDto.Password, user.PasswordHash);

            if(!isPasswordCorrect) throw new Exception("Incorrect password");

            return user;
        }

        public async void ChangePassword(ChangePassswordDto changePasswordDto)
        {
            UserEntity user = await userProvider.GetByEmail(changePasswordDto.Email);

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

        public async Task HandleOffDoneButton(Button DoneButton, CancellationToken cancelToken)
        {
            DoneButton.IsEnabled = false;
            string[] loadingWheel = { "Loading", "Loading.", "Loading..", "Loading..." };

            while (!DoneButton.IsEnabled)
            {
                for (int i = 0; i < loadingWheel.Length; i++)
                {
                    if (cancelToken.IsCancellationRequested)
                    {
                        DoneButton.Content = "Done";
                        DoneButton.IsEnabled = true;
                    }
                    else
                    {
                        DoneButton.Content = loadingWheel[i];
                    }

                    await Task.Delay(500);
                }
            }
        }
    }
}
