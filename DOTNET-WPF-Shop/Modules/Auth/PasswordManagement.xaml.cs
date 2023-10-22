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
    public partial class PasswordManagement : Window
    {
        private AuthProvider provider = new AuthProvider();
        private ProviderUtils providerUtils = new();
        private CancellationTokenSource cancelTokenSource;

        public PasswordManagement()
        {
            InitializeComponent();
        }

        private void Event_TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(sender as TextBox);
        }

        private void Event_TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxFocus(sender as TextBox);
        }

        private void Event_BackButtonClick(object sender, RoutedEventArgs e)
        {
            providerUtils.RedirectTo(this, new Start.Start());
        }

        private async Task _AcceptButtonClick()
        {
            ChangePassswordDto changePasswordDto = new()
            {
                Email = emailField.Text,
                OldPassword = oldPasswordField.Text,
                NewPassword = newPasswordField.Text,
            };

            bool isDataValid = new ProviderUtils().ValidateDto(changePasswordDto);

            try
            {
                if (isDataValid)
                {
                   await Task.Run(() => provider.ChangePassword(changePasswordDto));
                }

                cancelTokenSource.Cancel();

                providerUtils.RedirectTo(this, new Start.Start());
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
