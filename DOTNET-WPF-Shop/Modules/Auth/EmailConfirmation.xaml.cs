using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.User;
using DOTNET_WPF_Shop.Modules.User.Dto;
using DOTNET_WPF_Shop.Utils;
using System.Windows;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Modules.Auth
{
    public partial class EmailConfirmation : Window
    {
        private AuthProvider provider = new AuthProvider();
        private UserProvider userProvider = new UserProvider();
        private ProviderUtils providerUtils = new ProviderUtils();
        private UserEntity currentUser;
        private Window previousView;

        public EmailConfirmation(UserEntity user, Window previousView)
        {
            InitializeComponent();

            currentUser = user;
            this.previousView = previousView;
        }

        private void Event_TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxFocus(sender as TextBox);
        }

        private void Event_TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(sender as TextBox);
        }

        private void Event_BackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Event_ResendCodeClick(object sender, RoutedEventArgs e)
        {

            UpdateUserDto updateUserDto = new()
            {
                Id = currentUser.Id,
                Username = currentUser.Username,
                Email = currentUser.Email,
                PasswordHash = currentUser.PasswordHash,
                ConfirmationCode = provider.GenerateConfirmationCode()
            };

            UserEntity user = userProvider.Update(updateUserDto);

            provider.SendCodeToEmail(user.Email, user.ConfirmationCode);
        }

        private async void Event_AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentUser.ConfirmationCode == null)
            {
                MessageBox.Show("User's email already confirmed");

                provider.RedirectToMainPage(this, currentUser.Id, currentUser.Username);
            }

            if (codeField.Text == currentUser.ConfirmationCode)
            {
                MessageBox.Show("Your email has been confirmed!");

                UpdateUserDto updateUserDto = new()
                {
                    Id = currentUser.Id,
                    Username = currentUser.Username,
                    Email = currentUser.Email,
                    PasswordHash = currentUser.PasswordHash,
                    ConfirmationCode = null!
                };

                userProvider.Update(updateUserDto);

                provider.RedirectToMainPage(this, currentUser.Id, currentUser.Username);
                previousView.Close();
            }
            else MessageBox.Show("Wrong code. Pleasy try again");
        }
    }
}
