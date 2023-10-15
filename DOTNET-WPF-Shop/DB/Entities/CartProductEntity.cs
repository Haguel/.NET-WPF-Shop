using System;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class CartProductEntity
    {
        public int Quantity { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }

        public CartEntity Cart { get; set; }
        public ProductEntity Product { get; set; }

        public CartProductEntity()
        {
            Quantity = 1; 
        }
    }
}
