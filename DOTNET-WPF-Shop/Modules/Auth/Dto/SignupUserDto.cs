using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DOTNET_WPF_Shop.Modules.User
{
    class SignupUserDto
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "username")]
        [MinLength(6, ErrorMessage = "Username minimum length is 6 characters")]
        [MaxLength(50, ErrorMessage = "Username minimum length is 6 characters")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "password")]
        [MinLength(6, ErrorMessage = "Password minimum length is 6 characters")]
        public string password { get; set; }
    }
}
