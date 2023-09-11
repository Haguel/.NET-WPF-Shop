using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Auth;
using Microsoft.Data.SqlClient;
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

namespace DOTNET_WPF_Shop.Modules.Start
{
    public partial class Start : Window
    {
        StartProvider provider = new StartProvider();

        public Start()
        {
            InitializeComponent();
        }

        private void SignupClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectTo(this, new Signup());
        }

        private void SigninClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectTo(this, new Signin());
        }

        private void ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectTo(this, new PasswordManagement());
        }
    }
}
