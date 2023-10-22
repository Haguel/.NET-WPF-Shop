using System;
using System.ComponentModel.DataAnnotations;

namespace DOTNET_WPF_Shop.DB.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String PasswordHash { get; set; }
        [StringLength(6)]
        public String? ConfirmationCode { get; set; } = null;

        public CartEntity Cart { get; set; }
    }
}
