﻿using DOTNET_WPF_Shop.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_WPF_Shop.DB
{
    public class DataContext : DbContext
    {
        private string DbName = Config.Config.GetJsonData("DB:name");

        public DbSet<Entities.UserEntity> Users { get; set; }
        public DbSet<Entities.ProductEntity> Products { get; set; }
        public DbSet<Entities.CartEntity> Carts { get; set; }
        public DbSet<Entities.CartProductEntity> CartProducts { get; set; }
        public DbSet<Entities.CategoryEntity> Categories { get; set; }

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

            modelBuilder.Entity<CartProductEntity>()
                .HasKey(cartProduct => new { cartProduct.CartId, cartProduct.ProductId });

            modelBuilder.Entity<CartProductEntity>()
                .HasOne(cartProduct => cartProduct.Cart)
                .WithMany(cart => cart.CartProducts)
                .HasForeignKey(cartProduct => cartProduct.CartId);

            modelBuilder.Entity<ProductEntity>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId);

            modelBuilder.Entity<CartProductEntity>()
                .HasOne(cartProduct => cartProduct.Product)
                .WithMany(products => products.CartProducts)
                .HasForeignKey(cartProduct => cartProduct.ProductId);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<ProductEntity>()
                .HasIndex(product => product.Title)
                .IsUnique();

            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(category => category.Title)
                .IsUnique();
        }
    }
}
