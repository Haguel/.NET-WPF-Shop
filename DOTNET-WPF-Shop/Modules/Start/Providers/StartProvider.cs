using DOTNET_WPF_Shop.Modules.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Start
{
    public class StartProvider
    {
        public void RedirectToSignupPage(Start view)
        {
            view.Hide();
            new Signup().ShowDialog();
            view.Show();
        }

        public void RedirectToSigninPage(Start view)
        {
            view.Hide();
            new Signin().ShowDialog();
            view.Show();
        }
    }
}
