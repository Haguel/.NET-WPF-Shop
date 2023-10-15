using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Cart;
using DOTNET_WPF_Shop.Modules.Product;
using DOTNET_WPF_Shop.Modules.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Main
{
    public class MainProvider
    {
        ProductProvider productProvider = new();

        public async Task<List<ProductEntity>> GetProducts()
        {
            List<ProductEntity> selectedProducts = await productProvider.GetNotRemoved();
            List<ProductEntity> products = selectedProducts == null ? new() : new(selectedProducts);

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
