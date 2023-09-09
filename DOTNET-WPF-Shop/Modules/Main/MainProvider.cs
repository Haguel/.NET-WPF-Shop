using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Cart;
using DOTNET_WPF_Shop.Modules.Product;
using DOTNET_WPF_Shop.Modules.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DOTNET_WPF_Shop.Modules.Main
{
    public class MainProvider
    {
        private ProductProvider productProvider = new();
        private UserProvider userProvider = new();
        private CartProvider cartProvider = new();
        DataContext dataContext = new();

        public ObservableCollection<ProductEntity> GetProducts()
        {
            var query = dataContext
                .Products
                .Select(product => product);

            ObservableCollection<ProductEntity> products = new();

            foreach (ProductEntity product in query) { products.Add(product); }

            return products;
        }

        public void PutProductToCart(String productTitle, Guid userId)
        {
            ProductEntity product = productProvider.GetByTitle(productTitle);

            if (product == null) throw new Exception($"There is no product with title {productTitle}");

            UserEntity user = userProvider.GetById(userId);

            cartProvider.PutProduct(product, user);
        }
    }
}
