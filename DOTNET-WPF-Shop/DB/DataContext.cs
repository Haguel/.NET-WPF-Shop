using DOTNET_WPF_Shop.DB.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasOne(user => user.Cart)
                .WithOne(cart => cart.User)
                .HasForeignKey<CartEntity>(cart => cart.UserId);

            modelBuilder.Entity<ProductEntity>()
                .HasOne(product => product.Cart)
                .WithMany(cart => cart.Products)
                .HasForeignKey(product => product.CartId)
                .HasPrincipalKey(cart => cart.Id);
        }
    }
}
