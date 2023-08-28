using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Auth.Dto
{
    class CreateUserDto
    {
        public string username { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
    }
}
