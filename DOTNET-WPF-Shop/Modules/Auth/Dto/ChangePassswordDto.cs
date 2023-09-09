using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTNET_WPF_Shop.Modules.Auth.Dto
{
    public class ChangePassswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "password")]
        [MinLength(6, ErrorMessage = "Password minimum length is 6 characters")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "password")]
        [MinLength(6, ErrorMessage = "Password minimum length is 6 characters")]
        public string NewPassword { get; set; }
    }
}
