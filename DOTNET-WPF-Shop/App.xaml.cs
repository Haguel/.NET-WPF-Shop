using DOTNET_WPF_Shop.DB;
using System.Windows;

namespace DOTNET_WPF_Shop
{
    public partial class App : Application
    {
        public static DataContext dataContext = new();
    }
}
