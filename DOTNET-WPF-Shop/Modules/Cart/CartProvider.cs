﻿using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.User;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DOTNET_WPF_Shop.Modules.CartProduct;

namespace DOTNET_WPF_Shop.Modules.Cart
{
    public class CartProvider
    {
        private CartEntity cart;
        private DataContext dataContext = App.dataContext;
        private UserProvider userProvider = new();
        private CartProductProvider cartProductProvider = new();

        public CartProvider(Guid userId) { this.cart = GetFromUserId(userId); }

        public CartEntity GetFromUserId(Guid userId)
        {
            UserEntity user = userProvider.GetById(userId);

            return user.Cart;
        }

        public CartEntity GetCart() { return cart; }

        public async Task RemoveAllProductsFromCart()
        {
            List<CartProductEntity> cartProducts = await cartProductProvider.GetAllFromCart(cart);

            foreach (CartProductEntity cartProduct in cartProducts)
            {
                await cartProductProvider.Remove(cartProduct);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task RemoveProductFromCart(ProductEntity product)
        {
            CartProductEntity cartProduct = await cartProductProvider.Get(product, cart);

            await cartProductProvider.Remove(cartProduct);
        }

        public async Task<bool> IsProductInCart(ProductEntity product)
        {
            CartProductEntity cartProduct = await cartProductProvider.Get(product, cart);

            return cartProduct != null;
        }        

        public async Task<CartProductEntity> PutProduct(ProductEntity product)
        {
            return await cartProductProvider.Create(product, cart);
        }

        public async Task<List<CartProductEntity>> GetCartProducts()
        {
            List<CartProductEntity> cartProducts = await cartProductProvider.GetAllFromCart(cart);

            return cartProducts;
        }

        public List<ProductEntity> GetProductsFromCart()
        {
            List<ProductEntity> products = dataContext.CartProducts
                .Include(cartProduct => cartProduct.Product)
                .Where(cartProduct => cartProduct.CartId == cart.Id)
                .Select(cartProduct => cartProduct.Product)
                .ToList();

            return products;
        }

        public void RedirectToMainPage(Cart cartView, Main.Main mainView)
        {
            cartView.Hide();
            mainView.Show();
        }
    }
}
