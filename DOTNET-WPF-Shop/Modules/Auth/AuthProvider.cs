using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.User;
using DOTNET_WPF_Shop.Modules.User.Dto;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Modules.Auth
{
    class ConfirmationCodeException : Exception
    {
        public ConfirmationCodeException(): base("The email is not veryfied. Please verify your email") { }
    }

    class AuthProvider
    {
        private UserProvider userProvider = new();

        private SmtpClient GetSmtpClient()
        {
            String host = Config.Config.GetJsonData("smtp:host");
            int port = int.Parse(Config.Config.GetJsonData("smtp:port"));
            String email = Config.Config.GetJsonData("smtp:email");
            String password = Config.Config.GetJsonData("smtp:password");
            bool ssl = bool.Parse(Config.Config.GetJsonData("smtp:ssl"));

            return new(host, port)
            {
                EnableSsl = ssl,
                Credentials = new NetworkCredential(email, password)
            };
        }

        public String GenerateConfirmationCode()
        {
            return Guid.NewGuid().ToString()[..6].ToUpperInvariant();
        }

        public void SendCodeToEmail(string email, string code)
        {
            using SmtpClient smtpClient = GetSmtpClient();
            smtpClient.Send(
                Config.Config.GetJsonData("smtp:email")!,
                email,
                "Signup successful",
                code
            );
        }

        public void RedirectToEmailConfirmationPage(Window view, UserEntity user)
        {
            EmailConfirmation emailConfirmationView = new EmailConfirmation(user, view);
            view.Hide();
            emailConfirmationView.ShowDialog();
            view.Show();
        }

        public void RedirectToMainPage(Window view, Guid userId, String username)
        {
            Main.Main mainView = new Main.Main(userId, username);
            mainView.Show();
            view.Close();
        }

        public async Task<UserEntity> Signup(SignupUserDto signupUserDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(signupUserDto.Password);

            CreateUserDto createUserDto = new()
            {
                Username = signupUserDto.Username,
                Email = signupUserDto.Email,
                PasswordHash = passwordHash,
                ConfirmationCode = signupUserDto.ConfirmationCode,
            };

            UserEntity user = await userProvider.GetByEmail(signupUserDto.Email);

            if (user != null)
            {
                if (user.ConfirmationCode != null) return user;
                else throw new Exception("User already exists");
            }

            UserEntity newUser =  userProvider.Create(createUserDto);

            SendCodeToEmail(newUser.Email, newUser.ConfirmationCode);

            return newUser;
        }

        public async Task<UserEntity> Signin(SigninUserDto signinUserDto) 
        {
            UserEntity user = await userProvider.GetByEmail(signinUserDto.Email);

            if (user == null) throw new Exception("User doesn't exist");

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(signinUserDto.Password, user.PasswordHash);

            if (!isPasswordCorrect) throw new Exception("Incorrect password");
            if (user.ConfirmationCode != null) throw new ConfirmationCodeException();

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
