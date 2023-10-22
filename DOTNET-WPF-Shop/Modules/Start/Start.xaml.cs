using DOTNET_WPF_Shop.Modules.Auth;
using DOTNET_WPF_Shop.Utils;
using System.Windows;

namespace DOTNET_WPF_Shop.Modules.Start
{
    public partial class Start : Window
    {
        private ProviderUtils providerUtils = new();

        public Start()
        {
            InitializeComponent();
        }

        private void Event_SignupClick(object sender, RoutedEventArgs e)
        {
            providerUtils.RedirectTo(this, new Signup());
        }

        private void Event_SigninClick(object sender, RoutedEventArgs e)
        {
            providerUtils.RedirectTo(this, new Signin());
        }

        private void Event_ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            providerUtils.RedirectTo(this, new PasswordManagement());
        }
    }
}
