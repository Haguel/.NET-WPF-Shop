using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.CartProduct;
using DOTNET_WPF_Shop.Modules.Product;
using Microsoft.Extensions.Primitives;
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
        private CartEntity cart;
        private CartProductProvider cartProductProvider = new();
        private ProductProvider productProvider = new();
        
        public ObservableCollection<CartProductEntity> CartProducts { get; set; }

        public Cart(Guid userId, Main.Main mainView)
        {
            InitializeComponent();

            this.mainView = mainView;
            cartProvider = new(userId);
            CartProducts = new();

            this.DataContext = this;
        }

        public async Task LoadCartProducts()
        {
            int tempProductsCounter = 0;
            cart = cartProvider.GetCart();

            List<CartProductEntity> cartProducts = await cartProvider.GetCartProducts();

            foreach (CartProductEntity cartproduct in cartProducts)
            {
                tempProductsCounter += 1 * cartproduct.Quantity;

                CartProducts.Add(cartproduct);
            }

            mainView.CountOfProducts = tempProductsCounter;
        }

        private int GetIndexOfCartProduct(CartProductEntity cartProduct)
        {
            for (int i = 0; i < CartProducts.Count; i++)
            {
                if (CartProducts[i].Product.Title == cartProduct.Product.Title)
                {
                    return i;
                }
            }

            return -1;
        }

        public async void PutProduct(ProductEntity product) 
        {
            bool isProductInCart = await cartProvider.IsProductInCart(product);

            if (!isProductInCart)
            {
                CartProductEntity cartProduct = await cartProvider.PutProduct(product);

                CartProducts.Add(cartProduct);
            }
            else
            {
                CartProductEntity cartProduct = await cartProductProvider.Get(product, cart);

                int deleteIndex = GetIndexOfCartProduct(cartProduct);
                CartProducts.RemoveAt(deleteIndex);

                CartProductEntity updatedCartproduct = await cartProvider.UpdateProductQuantity(cartProduct, 1);

                CartProducts.Insert(deleteIndex, updatedCartproduct);
            }

            mainView.CountOfProducts++;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            cartProvider.RedirectToMainPage(this, mainView);
        }

        private void EmptyCartButtonClick(object sender, RoutedEventArgs e)
        {
            CartProducts.Clear();

            cartProvider.RemoveAllProductsFromCart();

            mainView.CountOfProducts = 0;
        }

        private async void RemoveProductFromCartClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button removeProductButton)
            {
                ProductEntity product = productProvider.GetByTitle(Convert.ToString(removeProductButton.Tag));
                CartProductEntity cartProduct = await cartProductProvider.Get(product, cart);

                for (int i = CartProducts.Count - 1; i >= 0; i--)
                {
                    if (CartProducts[i].Product.Title == cartProduct.Product.Title)
                    {
                        CartProducts.RemoveAt(i);
                        break;
                    }
                }

                cartProvider.RemoveProductFromCart(product);

                mainView.CountOfProducts--;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCartProducts();
        }
    }
}
