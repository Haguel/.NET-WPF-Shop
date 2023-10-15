using DOTNET_WPF_Shop.Modules.Auth;
using System.Windows;

namespace DOTNET_WPF_Shop.Modules.Start
{
    public partial class Start : Window
    {
        private StartProvider provider = new StartProvider();

        public Start()
        {
            InitializeComponent();
        }

        private void Event_SignupClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectTo(this, new Signup());
        }

        private void Event_SigninClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectTo(this, new Signin());
        }

        private void Event_ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectTo(this, new PasswordManagement());
        }
    }
}
