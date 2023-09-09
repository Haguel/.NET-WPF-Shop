using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Product
{
    public class ProductProvider
    {
        DataContext dataContext = new();

        public ProductEntity GetByTitle(string title)
        {
            ProductEntity product = dataContext
                .Products
                .FirstOrDefault(product => product.Title == title);

            return product;
        }

    }
}
