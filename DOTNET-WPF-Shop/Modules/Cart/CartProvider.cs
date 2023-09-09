using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Cart
{
    public class CartProvider
    {
        private DataContext dataContext = new();

        public void PutProduct(ProductEntity product, UserEntity user)
        {
            CartEntity cart = user.Cart;

            cart.Products.Add(product);
            dataContext.SaveChanges();
        }
    }
}
