using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Product;
using DOTNET_WPF_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private CancellationTokenSource cancelTokenSource;

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

        private async void NotifyAboutBuying(string pruductTitle)
        {
            CancellationToken cancelToken = cancelTokenSource.Token;
            NotificationTextBlock.Text = $"{pruductTitle} is added to the cart";

            await Task.Delay(2000);

            if (!cancelToken.IsCancellationRequested) NotificationTextBlock.Text = string.Empty;
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

                        if (cancelTokenSource != null) cancelTokenSource.Cancel();

                        cancelTokenSource = new();

                        NotifyAboutBuying(productTitle.Text);
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
            providerUtils.HandleTextBoxFocus(SearchBar);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(SearchBar);
        }

        private void MakeAllProductsVisible()
        {
            foreach (var item in itemsListView.Items)
            {
                ListViewItem lvi = itemsListView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (lvi != null) lvi.Visibility = Visibility.Visible;
            }
        }

        private void HideProductsThatDontHaveSubTitle(string subTitle)
        {
            var products = Products.Where(p => !p.Title.ToLower().Contains(subTitle.ToLower()));

            foreach (var product in products)
            {
                ListViewItem lvi = itemsListView.ItemContainerGenerator.ContainerFromItem(product) as ListViewItem;

                if (lvi != null) lvi.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // this if statement is only intended for properly event working during the initialization
            if (Products == null || SearchBar == null || itemsListView == null) return;

            MakeAllProductsVisible();

            if (SearchBar.Text != SearchBar.Tag)
            {
                HideProductsThatDontHaveSubTitle(SearchBar.Text);
            }
        }

    }   
}
