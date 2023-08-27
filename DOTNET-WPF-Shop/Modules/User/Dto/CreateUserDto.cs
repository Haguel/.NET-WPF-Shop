using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Auth.Dto
{
    class CreateUserDto
    {
        public string username { get; }
        public string email { get; }
        public string passwordHash { get; }
    }
}
