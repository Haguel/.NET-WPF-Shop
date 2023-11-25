using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Category;
using DOTNET_WPF_Shop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DOTNET_WPF_Shop.Modules.Main
{
    public partial class Main : Window, INotifyPropertyChanged
    {
        private MainProvider provider = new();
        private CategoryProvider categoryProvider = new();
        private ProviderUtils providerUtils = new();

        private string allCategory = "All";
        private string currentSearchText = "";
        private Cart.Cart cartView;
        private CancellationTokenSource cancelTokenSource;
        private int _countOfProducts;
        private List<ProductEntity> products = new();

        public String Username { get; set; }
        public ObservableCollection<string> CategoryTitles { get; set; }
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

            CategoryTitles = new();
            CategoryTitles.Add(allCategory);

            Products = new();
            CountOfProducts = 0;
            cartView = new(userId, this);

            this.DataContext = this;
            Username = username;
        }

        public void ChangeCountOfProductProp(int modifier) { CountOfProducts += modifier;  }
        public void ZeroCountOfProductProp() { CountOfProducts = 0; }

        private async Task LoadCategories()
        {
            List<CategoryEntity> categories = await categoryProvider.GetAll();

            foreach (CategoryEntity category in categories)
            {
                CategoryTitles.Add(category.Title);
            }
        }

        private async Task LoadProducts()
        {
            products = await provider.GetProducts();

            LoadingText.Visibility = Visibility.Collapsed;

            await AddToProductsProp(products);
        }

        private async Task AddToProductsProp(List<ProductEntity> products)
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

        private async void Event_BuyButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button buyButton)
            {
                if (buyButton.Parent is StackPanel productStackPanel)
                {
                    ProductEntity product = productStackPanel.Tag as ProductEntity;
                    cartView.PutProduct(product);

                    if (cancelTokenSource != null) cancelTokenSource.Cancel();

                    cancelTokenSource = new();

                    NotifyAboutBuying(product.Title);
                }
            }
        }

        private void SetProductsFromCategory(CategoryEntity category)
        {
            List<ProductEntity> categoryProducts = category.Products;    

            Products.Clear();

            foreach (ProductEntity product in categoryProducts) 
            {
                Products.Add(product);
            }
        }
        
        private async Task HandleFilterComboBox()
        {
            if (Products == null) return;

            string currentCategoryTitle = FilterComboBox.SelectedItem as string;

            if (currentCategoryTitle == allCategory)
            {
                SetAllProducts();
            }
            else
            {
                CategoryEntity currentCategory = await categoryProvider.GetByTitle(currentCategoryTitle);

                if (currentCategory == null) 
                {
                    MessageBox.Show("Error: There is no category with title: " + currentCategoryTitle);

                    return;
                }

                SetProductsFromCategory(currentCategory);
            }

            if(currentSearchText != "") HideProductsThatDontHaveSubString(currentSearchText);
        }

        private void SetAllProducts()
        {
            Products.Clear();

            foreach (ProductEntity product in products) 
            {
                Products.Add(product);
            }
        }

        private void MakeAllProductsVisible()
        {
            foreach (ProductEntity product in Products)
            {
                ListViewItem listViewItem = itemsListView.ItemContainerGenerator.ContainerFromItem(product) as ListViewItem;

                listViewItem.Visibility = Visibility.Visible;
            }
        }

        private void HandleProductIfContainsSubString(ProductEntity product, ListViewItem listViewItem, string subString)
        {
            if (product.Title.ToLower().Contains(subString.ToLower()))
            {
                listViewItem.Visibility = Visibility.Visible;
            }
            else
            {
                listViewItem.Visibility = Visibility.Collapsed;
            }
        }

        private void HideProductsThatDontHaveSubString(string subString)
        {
            List<ProductEntity> listViewProducts = itemsListView.Items.Cast<ProductEntity>().ToList();

            foreach (ProductEntity product in listViewProducts)
            {
                ListViewItem listViewItem = itemsListView.ItemContainerGenerator.ContainerFromItem(product) as ListViewItem;

                HandleProductIfContainsSubString(product, listViewItem, subString);
            }
        }

        private void Event_CartButtonClick(object sender, RoutedEventArgs e)
        {
            providerUtils.OpenModal(this, cartView);
        }

        private void Event_TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Products == null || SearchBar == null || itemsListView == null) return;

            if (SearchBar.Text != SearchBar.Tag)
            {
                currentSearchText = SearchBar.Text;

                MakeAllProductsVisible();

                if (currentSearchText != "") HideProductsThatDontHaveSubString(currentSearchText);
            }
        }

        private void Event_TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxFocus(SearchBar);
        }

        private void Event_TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            providerUtils.HandleTextBoxUnfocus(SearchBar);
        }

        private async void Event_WindowLoaded(object sender, RoutedEventArgs e)
        {
            await cartView.LoadCartProducts();

            await LoadCategories();
            await LoadProducts();
        }

        private void Event_FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandleFilterComboBox();
        }

        private void Event_WindowClosing(object sender, CancelEventArgs e)
        {
            cartView.Show();
            cartView.isMainClosed = true;
            cartView.Close();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }   
}
