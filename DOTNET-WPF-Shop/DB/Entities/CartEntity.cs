using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class CartEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public UserEntity User { get; set; }
        public List<ProductEntity> Products { get; set; }
    }
}
