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
        new StartProvider provider = new StartProvider();

        public Start()
        {
            InitializeComponent();
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            provider.RedirectToSignupPage(this);
        }

        private void Signin_Click(object sender, RoutedEventArgs e)
        {
            provider.RedirectToSigninPage(this);
        }
    }
}
