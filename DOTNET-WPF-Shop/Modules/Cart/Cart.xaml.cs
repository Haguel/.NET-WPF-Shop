using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.CartProduct;
using DOTNET_WPF_Shop.Modules.Product;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
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
    public partial class Cart : Window, INotifyPropertyChanged
    {
        private Main.Main mainView;
        private CartProvider provider;
        private CartEntity cart;
        private CartProductProvider cartProductProvider = new();
        private ProductProvider productProvider = new();
        
        public ObservableCollection<CartProductEntity> CartProducts { get; set; }
        private double _totalPrice;
        public double TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = Math.Round(value, 2);

                    OnPropertyChanged("TotalPrice");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Cart(Guid userId, Main.Main mainView)
        {
            InitializeComponent();

            this.mainView = mainView;
            provider = new(userId);
            CartProducts = new();
            TotalPrice = 0;

            this.DataContext = this;
        }

        public async Task LoadCartProducts()
        {
            int productsCounter = 0;
            cart = provider.GetCart();

            List<CartProductEntity> cartProducts = await provider.GetCartProducts();

            foreach (CartProductEntity cartproduct in cartProducts)
            {
                productsCounter += 1 * cartproduct.Quantity;
                TotalPrice += cartproduct.Quantity * cartproduct.Product.Price;

                CartProducts.Add(cartproduct);
            }

            mainView.ChangeCountOfProductProp(productsCounter);
        }

        public async void PutProduct(ProductEntity product) 
        {
            bool isProductInCart = await provider.IsProductInCart(product);

            if (!isProductInCart)
            {
                CartProductEntity cartProduct = await provider.PutProduct(product);

                CartProducts.Add(cartProduct);
            }
            else
            {
                await UpdateProductQuantity(product, 1);
            }

            ChangeTotalPriceProp(1, product.Price);
            mainView.ChangeCountOfProductProp(1); 
        }

        public void ChangeTotalPriceProp(int modifier, double price) { TotalPrice += modifier * price; }
        public void ZeroTotalPriceProp() { TotalPrice = 0; }

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

        private async Task UpdateProductQuantity(ProductEntity product, int modifier)
        {
            CartProductEntity cartProduct = await cartProductProvider.Get(product, cart);

            int deleteIndex = GetIndexOfCartProduct(cartProduct);
            CartProducts.RemoveAt(deleteIndex);

            if (cartProduct.Quantity == 1 && modifier < 0)
            {
                await provider.RemoveProductFromCart(product);
            }
            else
            {
                CartProductEntity updatedCartproduct = provider.UpdateProductQuantity(cartProduct, modifier);

                CartProducts.Insert(deleteIndex, updatedCartproduct);
            }

        }

        private async void RemoveProductFromCartClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button removeProductButton)
            {

                ProductEntity product = productProvider.GetByTitle(Convert.ToString(removeProductButton.Tag));
                CartProductEntity cartProduct = await cartProductProvider.Get(product, cart);

                ChangeTotalPriceProp(-1, product.Price);

                for (int i = CartProducts.Count - 1; i >= 0; i--)
                {
                    if (CartProducts[i].Product.Title == cartProduct.Product.Title)
                    {
                        CartProducts.RemoveAt(i);
                        break;
                    }
                }

                provider.RemoveProductFromCart(product);
                mainView.ChangeCountOfProductProp(-1);
            }
        }

        private async void PlusQuantity_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductEntity product = button.Tag as ProductEntity;

            ChangeTotalPriceProp(1, product.Price);
            await UpdateProductQuantity(product, 1);

            mainView.ChangeCountOfProductProp(1);
        }

        private async void MinusQuantity_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductEntity product = button.Tag as ProductEntity;

            ChangeTotalPriceProp(-1, product.Price);
            await UpdateProductQuantity(product, -1);

            mainView.ChangeCountOfProductProp(-1);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectToMainPage(this, mainView);
        }

        private async void EmptyCartButtonClick(object sender, RoutedEventArgs e)
        {
            CartProducts.Clear();
            await provider.RemoveAllProductsFromCart();

            ZeroTotalPriceProp();
            mainView.ZeroCountOfProductProp();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
