using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Cart
{
    public class CartProvider
    {
        private CartEntity cart;
        private DataContext dataContext = new();
        private UserProvider userProvider = new();

        public CartProvider(Guid userId) { this.cart = GetFromUserId(userId); }

        public CartEntity GetFromUserId(Guid userId)
        {
            UserEntity user = userProvider.GetById(userId);

            return user.Cart;
        }

        public void RemoveAllProductsFromCart()
        {
            List<CartProduct> cartProducts = dataContext.CartProducts
                .Include(cartProduct => cartProduct.Cart)
                .Where(cartProduct => cartProduct.CartId == cart.Id)
                .Select(cartProduct => cartProduct)
                .ToList();

            foreach (CartProduct cartProduct in cartProducts)
            {
                dataContext.CartProducts.Remove(cartProduct);
            }

            dataContext.SaveChanges();
        }

        public void RemoveProductFromCart(ProductEntity product)
        {
            CartProduct cartProduct = dataContext.CartProducts
                .Include(cartProduct => cartProduct.Product)
                .Include(cartProduct => cartProduct.Cart)
                .Where(cartProduct => cartProduct.CartId == cart.Id && cartProduct.ProductId == product.Id)
                .Select(cartProduct => cartProduct)
                .ToList()[0];

            dataContext.CartProducts.Remove(cartProduct);
            dataContext.SaveChanges();
        }

        public bool IsProductInCart(ProductEntity product)
        {
            List<ProductEntity> products = dataContext.CartProducts
                .Include(cartProduct => cartProduct.Product)
                .Include(cartProduct => cartProduct.Cart)
                .Where(cartProduct => cartProduct.CartId == cart.Id && cartProduct.ProductId == product.Id)
                .Select(cartProduct => cartProduct.Product)
                .ToList();

            return products.Count() == 1;
        }

        public void PutProduct(ProductEntity product)
        {
            CartProduct cartProduct = new()
            {
                Product = product,
                Cart = cart
            };

            dataContext.Products.Attach(product);
            dataContext.Carts.Attach(cart);
            dataContext.CartProducts.Add(cartProduct);
            dataContext.SaveChanges();
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
