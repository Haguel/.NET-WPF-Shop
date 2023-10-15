using System;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String PasswordHash { get; set; }

        public CartEntity Cart { get; set; }
    }
}
