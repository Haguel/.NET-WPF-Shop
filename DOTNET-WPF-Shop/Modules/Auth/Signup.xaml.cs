using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.User;
using DOTNET_WPF_Shop.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Modules.Auth
{
    public partial class Signup : Window
    {
        private AuthProvider provider = new AuthProvider();
        private ProviderUtils providerUtils = new ProviderUtils();
        private CancellationTokenSource cancelTokenSource;

        public Signup()
        {
            InitializeComponent();
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
            provider.HidePage(this);
        }

        private async Task _AcceptButtonClick()
        {
            SignupUserDto signupUserDto = new()
            {
                Username = usernameField.Text,
                Email = emailField.Text,
                Password = passwordField.Text,
            };

            bool isDataValid = new ProviderUtils().ValidateDto(signupUserDto);

            try
            {
                if (isDataValid)
                {
                    UserEntity user = await Task.Run(() => 
                    {
                        return provider.Signup(signupUserDto); 
                    });

                    provider.RedirectToMainPage(this, user.Id, user.Username);
                }

                cancelTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                cancelTokenSource.Cancel();

                MessageBox.Show(ex.Message);
            }
        }

        private async void Event_AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            cancelTokenSource = new();

            await Task.WhenAll(
                provider.HandleOffDoneButton(DoneButton, cancelTokenSource.Token),
                _AcceptButtonClick()
            );
        }
    }
}
