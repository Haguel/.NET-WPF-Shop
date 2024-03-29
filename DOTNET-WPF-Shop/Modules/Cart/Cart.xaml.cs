﻿using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.CartProduct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DOTNET_WPF_Shop.Modules.Cart
{
    public partial class Cart : Window, INotifyPropertyChanged
    {
        private Main.Main mainView;
        private CartProvider provider;
        private CartEntity cart;
        private CartProductProvider cartProductProvider = new();
        private double _totalPrice;
        private object priceLocker = new object();
        
        public bool isMainClosed = false;
        public ObservableCollection<CartProductEntity> CartProducts { get; init; }
        public double TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = Math.Round(value, 3);

                    if (Math.Abs(_totalPrice) < 0.001) _totalPrice = +0;

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

        public void ChangeTotalPriceProp(int modifier, double price) 
        {
            lock (priceLocker) { TotalPrice += modifier * price; }
        }
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
                CartProductEntity updatedCartproduct = cartProductProvider.UpdateProductQuantity(cartProduct, modifier);

                CartProducts.Insert(deleteIndex, updatedCartproduct);
            }

        }

        private async void Event_RemoveProductFromCartClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button removeProductButton)
            {
                ProductEntity product = removeProductButton.Tag as ProductEntity;
                CartProductEntity cartProduct = await cartProductProvider.Get(product, cart);

                ChangeTotalPriceProp(-1 * cartProduct.Quantity, Math.Round(product.Price, 3));
                mainView.ChangeCountOfProductProp(-1 * cartProduct.Quantity);

                for (int i = CartProducts.Count - 1; i >= 0; i--)
                {
                    if (CartProducts[i].Product.Title == cartProduct.Product.Title)
                    {
                        CartProducts.RemoveAt(i);
                        break;
                    }
                }

                await provider.RemoveProductFromCart(product);
            }
        }

        private async void Event_PlusQuantityClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductEntity product = button.Tag as ProductEntity;

            ChangeTotalPriceProp(1, product.Price);
            await UpdateProductQuantity(product, 1);

            mainView.ChangeCountOfProductProp(1);
        }

        private async void Event_MinusQuantityClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProductEntity product = button.Tag as ProductEntity;

            ChangeTotalPriceProp(-1, product.Price);
            await UpdateProductQuantity(product, -1);

            mainView.ChangeCountOfProductProp(-1);
        }

        private void Event_BackButtonClick(object sender, RoutedEventArgs e)
        {
            provider.RedirectToMainPage(this, mainView);
        }

        private async void Event_EmptyCartButtonClick(object sender, RoutedEventArgs e)
        {
            CartProducts.Clear();
            await provider.RemoveAllProductsFromCart();

            ZeroTotalPriceProp();
            mainView.ZeroCountOfProductProp();
        }
        
        private void Event_WindowClosing(object sender, CancelEventArgs e)
        {
            // In order to have opportunity to open this window this ShowDialog() again
            if (!isMainClosed)
            {
                e.Cancel = true;

                provider.RedirectToMainPage(this, mainView);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
