using System;
using System.Collections.Generic;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class CartEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public UserEntity User { get; set; }
        public List<CartProductEntity> CartProducts { get; set; }
    }
}
