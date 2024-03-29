﻿using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Category
{
    public class CategoryProvider
    {
        DataContext dataContext = App.dataContext;

        public async Task<List<CategoryEntity>> GetAll()
        {
            return await dataContext
                .Categories
                .Include(category => category.Products)
                .ToListAsync();
        }

        public async Task<CategoryEntity> GetByTitle(string title)
        {
            return await dataContext
                .Categories
                .Include(category => category.Products)
                .Where(category => category.Title == title)
                .FirstOrDefaultAsync();
        }
    }
}
