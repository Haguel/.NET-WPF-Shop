﻿using DOTNET_WPF_Shop.Modules.Auth.Dto;
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
        private AuthProvider provider = new AuthProvider();
        private ProviderUtils providerUtils = new();

        public PasswordManagement()
        {
            InitializeComponent();
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(sender as TextBox);
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxFocus(sender as TextBox);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            provider.HidePage(this);
        }

        private async void AcceptButtonClick(object sender, RoutedEventArgs e)
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

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
