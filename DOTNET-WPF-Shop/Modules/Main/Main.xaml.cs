using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Product;
using DOTNET_WPF_Shop.Utils;
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
        private ProviderUtils providerUtils = new();

        public String username { get; set; }
        public ObservableCollection<ProductEntity> Products { get; set; }

        public Main(Guid userId, string username)
        {
            InitializeComponent();

            cartView = new(userId, this);
            Products = new();

            this.DataContext = this;
            this.username = username;
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<ProductEntity> products = await provider.GetProductsAsync();

            LoadingText.Visibility = Visibility.Collapsed;

            foreach (ProductEntity product in products)
            {
                Products.Add(product);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxFocus(sender as TextBox);

            string title = "Mango"; 
            var product = Products.FirstOrDefault(p => p.Title == title);
            if (product != null)
            {
                ListViewItem lvi = itemsListView.ItemContainerGenerator.ContainerFromItem(product) as ListViewItem;
                lvi.Visibility = Visibility.Collapsed;
            }


        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(sender as TextBox);
        }
    }
}
