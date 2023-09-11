using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public Double Price { get; set; }
        public String ImageSrc { get; set; }
        public Boolean? IsRemoved { get; set; } = null!;

        public List<CartProduct> CartProducts { get; set; }
    }
}
