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

namespace DOTNET_WPF_Shop.Modules.Cart
{
    public partial class Cart : Window
    {
        private Main.Main mainView;
        private CartProvider cartProvider;
        private ProductProvider productProvider = new();
        public ObservableCollection<ProductEntity> Products { get; set; }

        public Cart(Guid userId, Main.Main mainView)
        {
            InitializeComponent();

            this.mainView = mainView;
            cartProvider = new(userId);

            List<ProductEntity> products = cartProvider.GetProductsFromCart();

            Products = (products == null) ? new() : new(products);

            this.DataContext = this;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }


        public void PutProduct(ProductEntity product) 
        {
            if (!cartProvider.IsProductInCart(product))
            {
                cartProvider.PutProduct(product);

                if (Products == null) Products = new();

                Products.Add(product); 
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            cartProvider.RedirectToMainPage(this, mainView);
        }

        private void EmptyCartButtonClick(object sender, RoutedEventArgs e)
        {
            Products.Clear();

            cartProvider.RemoveAllProductsFromCart();
        }

        private void RemoveProductFromCartClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button removeProductButton)
            {
                ProductEntity product = productProvider.GetByTitle(Convert.ToString(removeProductButton.Tag));

                for (int i = Products.Count - 1; i >= 0; i--)
                {
                    if (Products[i].Title == product.Title)
                    {
                        Products.RemoveAt(i);
                        break;
                    }
                }

                cartProvider.RemoveProductFromCart(product);
            }
        }
    }
}
