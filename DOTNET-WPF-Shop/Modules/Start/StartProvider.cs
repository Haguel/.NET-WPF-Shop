using DOTNET_WPF_Shop.Modules.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DOTNET_WPF_Shop.Modules.Start
{
    public class StartProvider
    {
        public void RedirectTo(Start view, Window anotherView)
        {
            anotherView.ShowDialog();
            view.Close();
        }
    }
}
