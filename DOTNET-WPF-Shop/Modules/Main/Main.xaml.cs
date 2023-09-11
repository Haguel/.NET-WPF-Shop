using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Product;
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
        private Cart.Cart cartView;
        private MainProvider provider = new();
        private ProductProvider productProvider = new();
        public ObservableCollection<ProductEntity> Products { get; set; }

        public Main(Guid userId)
        {
            InitializeComponent();

            cartView = new(userId, this);
            Products = provider.GetProducts();
            this.DataContext = this;
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button buyButton)
            {
                if (buyButton.Parent is StackPanel productStackPanel)
                {
                    if (productStackPanel.Children[1] is TextBlock productTitle)
                    {
                        cartView.PutProduct(productProvider.GetByTitle(productTitle.Text));
                    }
                }
            }
        }

        private void CartButtonClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectToCartPage(this, cartView);
        }
    }
}
