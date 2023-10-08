using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Product;
using DOTNET_WPF_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
    public partial class Main : Window, INotifyPropertyChanged
    {
        private Cart.Cart cartView;
        private MainProvider provider = new();
        private ProductProvider productProvider = new();
        private ProviderUtils providerUtils = new();
        private CancellationTokenSource cancelTokenSource;
        private int _countOfProducts;
        private bool isSortHandling = true;

        public String Username { get; set; }
        public ObservableCollection<ProductEntity> Products { get; set; }
        public int CountOfProducts
        {
            get { return _countOfProducts; }
            set
            {
                if (_countOfProducts != value)
                {
                    _countOfProducts = value;
                    OnPropertyChanged("CountOfProducts");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Main(Guid userId, string username)
        {
            InitializeComponent();

            cartView = new(userId, this);
            Products = new();
            CountOfProducts = 0;

            this.DataContext = this;
            Username = username;
        }

        public void ChangeCountOfProductProp(int modifier) { CountOfProducts += modifier;  }
        public void ZeroCountOfProductProp() { CountOfProducts = 0; }

        private async Task LoadProducts()
        {
            ObservableCollection<ProductEntity> products = await provider.GetProductsSortedByAsc(ProductEntity => ProductEntity.Title);

            LoadingText.Visibility = Visibility.Collapsed;

            await AddToProductsProp(products);
        }

        private async Task AddToProductsProp(ObservableCollection<ProductEntity> products)
        {
            foreach (ProductEntity product in products)
            {
                Products.Add(product);
            }
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

        private async Task HandleSortByComboBox()
        {
            if (Products == null) return;

            ObservableCollection<ProductEntity> products = new();
            ComboBoxItem SortByOptionItem = SortByComboBox.SelectedItem as ComboBoxItem;

            Products.Clear();

            if (SortByOptionItem.Name == SortByTitleAscItem.Name)
            {
                products = await provider.GetProductsSortedByAsc(ProductEntity => ProductEntity.Title);
            }
            else if (SortByOptionItem.Name == SortByTitleDescItem.Name)
            {
                products = await provider.GetProductsSortedByDesc(ProductEntity => ProductEntity.Title);
            }
            else if (SortByOptionItem.Name == SortByPriceAscItem.Name)
            {
                products = await provider.GetProductsSortedByAsc(ProductEntity => ProductEntity.Price.ToString());
            }
            else if (SortByOptionItem.Name == SortByPriceDescItem.Name)
            {
                products = await provider.GetProductsSortedByDesc(ProductEntity => ProductEntity.Price.ToString());
            }
            
            AddToProductsProp(products);
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

        private void CartButtonClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectToCartPage(this, cartView);
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

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxFocus(SearchBar);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(SearchBar);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await cartView.LoadCartProducts();

            await LoadProducts();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox SortByComboBox = sender as ComboBox;
            isSortHandling = SortByComboBox.IsDropDownOpen;

            HandleSortByComboBox();
        }

        private void SortByComboBox_DropDownClosed(object sender, EventArgs e)
        {

        }
    }   
}
