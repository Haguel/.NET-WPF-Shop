using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.DB
{
    public class DataContext : DbContext
    {
        private string DbName = "DOTNET-WPF-SHOP";

        public DbSet<Entities.UserEntity> Users { get; set; }
        public DbSet<Entities.ProductEntity> Products { get; set; }

        public DataContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=" + DbName + ";Integrated Security=True");
        }
    }
}
