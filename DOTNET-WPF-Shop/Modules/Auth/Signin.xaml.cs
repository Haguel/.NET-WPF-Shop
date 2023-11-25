using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth.Dto;
using DOTNET_WPF_Shop.Modules.User;
using DOTNET_WPF_Shop.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Modules.Auth
{
    public partial class Signin : Window
    {
        private AuthProvider provider = new AuthProvider();
        private UserProvider userProvider = new UserProvider();
        private ProviderUtils providerUtils = new ProviderUtils();
        private CancellationTokenSource cancelTokenSource;
        private string actualPasswordText = "";

        public Signin()
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
            providerUtils.RedirectTo(this, new Start.Start());
        }

        private void Event_PasswordTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!passwordField.Text.Equals(passwordField.Tag))
            {
                TextBox textBox = sender as TextBox;

                // Disable this event to be able replace text with asterisks without triggering it
                textBox.TextChanged -= Event_PasswordTextBoxTextChanged;

                if (textBox.Text.Length < actualPasswordText.Length)
                {
                    actualPasswordText = actualPasswordText.Remove(actualPasswordText.Length - 1);
                }
                else if (textBox.Text.Length > actualPasswordText.Length)
                {
                    char addedChar = textBox.Text[textBox.Text.Length - 1];

                    if (addedChar != ' ') actualPasswordText += addedChar;
                }

                textBox.Text = new string('*', actualPasswordText.Length);

                textBox.SelectionStart = textBox.Text.Length;

                textBox.TextChanged += Event_PasswordTextBoxTextChanged;
            }
                UserEntity user = await userProvider.GetByEmail(signinUserDto.Email);
                provider.RedirectToEmailConfirmationPage(this, user);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cancelTokenSource.Cancel();
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
