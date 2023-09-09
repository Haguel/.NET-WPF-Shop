using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.User;
using DOTNET_WPF_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        AuthProvider provider = new AuthProvider();

        public PasswordManagement()
        {
            InitializeComponent();
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            provider.HandleTextBoxUnfocus(sender as TextBox);
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            provider.HandleTextBoxFocus(sender as TextBox);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            provider.HidePage(this);
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
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
                if (isDataValid) provider.ChangePassword(changePasswordDto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            provider.RedirectToMainPage(this);
        }
    }
}
