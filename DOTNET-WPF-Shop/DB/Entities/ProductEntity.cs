using System;
using System.Collections.Generic;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public Double Price { get; set; }
        public String ImageSrc { get; set; }
        public Boolean IsRemoved { get; set; }
        public Guid CategoryId { get; set; }

        public List<CartProductEntity> CartProducts { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
