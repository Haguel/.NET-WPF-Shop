using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class CategoryEntity
    {
        public Guid Id { get; set; }
        public String Title { get; set; }

        public List<ProductEntity> Products { get; set; }
    }
}
