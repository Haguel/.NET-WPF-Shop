using DOTNET_WPF_Shop.DB.Entities;
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
    public partial class Signup : Window
    {
        AuthProvider provider = new AuthProvider();

        public Signup()
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

        private async void _AcceptButtonClick()
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
                    UserEntity user = await Task.Run(() => provider.Signup(signupUserDto));

                    provider.RedirectToMainPage(this, user.Id, user.Username);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DoneButton.IsEnabled = true;
            }
        }

        private async void HandleOffDoneButton()
        {
            DoneButton.IsEnabled = false;
            DoneButton.Content = "Loading";

            while (!DoneButton.IsEnabled)
            {
                if (DoneButton.Content != "Loading...")
                {
                    Dispatcher.Invoke(() =>
                    {
                        DoneButton.Content += ".";
                    });

                    await Task.Delay(400);
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        DoneButton.Content = "Loading";
                    });
                }
            }
        }

        private async void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            HandleOffDoneButton();
            _AcceptButtonClick();

            DoneButton.IsEnabled = true;
            DoneButton.Content = "Done";
        }
    }
}
