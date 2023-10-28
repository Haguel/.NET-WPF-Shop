using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Product
{
    public class ProductProvider
    {
        DataContext dataContext = App.dataContext;

        public async Task<ProductEntity> GetByTitle(string title)
        {
            ProductEntity product = dataContext
                .Products
                .FirstOrDefault(product => product.Title == title);

            return product;
        }

        public async Task<ProductEntity> GetById(Guid id)
        {
            ProductEntity product = dataContext
                .Products
                .FirstOrDefault(product => product.Id == id);

            return product;
        }

        public async Task<List<ProductEntity>> GetNotRemoved() 
        {
            List<ProductEntity> selectedProducts = await dataContext
                .Products
                .Where(product => product.IsRemoved == false)
                .OrderByDescending(product => product.Title)
                .ToListAsync();

            return selectedProducts;
        }

    }
}
