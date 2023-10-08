using DOTNET_WPF_Shop.DB;
using DOTNET_WPF_Shop.DB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.CartProduct
{
    public class CartProductProvider
    {
        DataContext dataContext = new();

        public async Task<CartProductEntity> Get(ProductEntity product, CartEntity cart)
        {
            CartProductEntity cartProduct = await dataContext
                .CartProducts
                .Include(cartProduct => cartProduct.Product)
                .Include(cartProduct => cartProduct.Cart)
                .Where(cartProduct => cartProduct.CartId == cart.Id && cartProduct.ProductId == product.Id)
                .FirstOrDefaultAsync();

            return cartProduct;
        }

        public async Task<List<CartProductEntity>> GetAllFromCart(CartEntity cart)
        {
            List<CartProductEntity> cartProducts = await dataContext.CartProducts
                .Include(cartProduct => cartProduct.Cart)
                .Include(cartProduct => cartProduct.Product)
                .Where(cartProduct => cartProduct.CartId == cart.Id)
                .Select(cartProduct => cartProduct)
                .ToListAsync();

            return cartProducts;
        }

        public async Task<CartProductEntity> Create(ProductEntity product, CartEntity cart)
        {
            CartProductEntity cartProduct = new()
            {
                ProductId = product.Id,
                CartId = cart.Id,
            };

            dataContext.CartProducts.Add(cartProduct);
            dataContext.SaveChanges();

            dataContext.Entry(cartProduct).Reference(cp => cp.Product).Load();
            dataContext.Entry(cartProduct).Reference(cp => cp.Cart).Load();

            return cartProduct;
        }

        public async Task Remove(CartProductEntity cartProduct)
        {
            dataContext.CartProducts.Remove(cartProduct);
            dataContext.SaveChanges();
            dataContext.Entry(cartProduct).State = EntityState.Detached;
        }
    }
}
