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
        private DataContext dataContext = new();
        private UserProvider userProvider = new();

        public CartEntity GetFromUserId(Guid userId)
        {
            UserEntity user = userProvider.GetById(userId);

            return user.Cart;
        }

        public void PutProduct(ProductEntity product, CartEntity cart)
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

        public List<ProductEntity> GetProductsFromCart(CartEntity cart)
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
