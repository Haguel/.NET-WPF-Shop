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
        private string actualPasswordText = "";

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

                    if(addedChar != ' ') actualPasswordText += addedChar;
                }

                textBox.Text = new string('*', actualPasswordText.Length);

                textBox.SelectionStart = textBox.Text.Length;

                textBox.TextChanged += Event_PasswordTextBoxTextChanged;
            }
                Password = actualPasswordText,
                ConfirmationCode = provider.GenerateConfirmationCode()
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

                    provider.RedirectToEmailConfirmationPage(this, user);
                }

                cancelTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
