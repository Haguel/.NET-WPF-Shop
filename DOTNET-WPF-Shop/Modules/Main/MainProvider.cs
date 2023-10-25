using DOTNET_WPF_Shop.DB.Entities;
using DOTNET_WPF_Shop.Modules.Product;
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
    }
}
