using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.User;
using DOTNET_WPF_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DOTNET_WPF_Shop.Modules.Auth
{
    public partial class Signin : Window
    {
        private AuthProvider provider = new AuthProvider();
        private ProviderUtils providerUtils = new ProviderUtils();
        private CancellationTokenSource cancelTokenSource;

        public Signin()
        {
            InitializeComponent();
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxFocus(sender as TextBox);
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(sender as TextBox);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            provider.HidePage(this);
        }

        private async Task _AcceptButtonClick()
        {
            SigninUserDto signinUserDto = new()
            {
                Email = emailField.Text,
                Password = passwordField.Text,
            };

            bool isDataValid = new ProviderUtils().ValidateDto(signinUserDto);

            try
            {
                if (isDataValid)
                {
                    UserEntity user = await Task.Run(() =>
                    {
                        return provider.Signin(signinUserDto);
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

        private async void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            cancelTokenSource = new();

            await Task.WhenAll(
                provider.HandleOffDoneButton(DoneButton, cancelTokenSource.Token),
                _AcceptButtonClick()
            );
        }
    }
}
