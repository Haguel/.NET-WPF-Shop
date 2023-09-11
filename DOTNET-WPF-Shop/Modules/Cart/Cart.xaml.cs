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

namespace DOTNET_WPF_Shop.Modules.Cart
{
    public partial class Cart : Window
    {
        private CartEntity cart;
        private Main.Main mainView;
        private CartProvider cartProvider = new();
        public ObservableCollection<ProductEntity> Products { get; set; }

        public Cart(Guid userId, Main.Main mainView)
        {
            InitializeComponent();

            this.mainView = mainView;
            cart = cartProvider.GetFromUserId(userId);

            List<ProductEntity> products = cartProvider.GetProductsFromCart(cart);

            Products = (products == null) ? new() : new(products);

            this.DataContext = this;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }


        public void PutProduct(ProductEntity product) 
        {
            cartProvider.PutProduct(product, cart);

            Products.Add(product);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            cartProvider.RedirectToMainPage(this, mainView);
        }

    }
}
