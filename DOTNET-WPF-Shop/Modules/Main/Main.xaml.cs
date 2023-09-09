using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DOTNET_WPF_Shop.Modules.Main
{
    public partial class Main : Window
    {
        private MainProvider provider = new();
        public ObservableCollection<ProductEntity> Products { get; set; }

        public Main()
        {
            InitializeComponent();

            Products = provider.GetProducts();
            this.DataContext = this;
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.Tag.ToString());
        }
    }
}
