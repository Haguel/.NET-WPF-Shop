using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Cart;
using DOTNET_WPF_Shop.Modules.Product;
using DOTNET_WPF_Shop.Modules.User;
using Microsoft.EntityFrameworkCore;
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
        DataContext dataContext = new();

        public async Task<ObservableCollection<ProductEntity>> GetProductsAsync()
        {
            List<ProductEntity> selectedProducts = await dataContext
                .Products
                .Where(product => product.IsRemoved == false)
                .ToListAsync();

            ObservableCollection<ProductEntity> products = (selectedProducts == null) ? new() : new(selectedProducts);

            return products;
        }


        public void RedirectToCartPage(Main view, Cart.Cart cartView)
        {
            view.Hide();
            cartView.ShowDialog();
            view.Show();
        }
    }
}
