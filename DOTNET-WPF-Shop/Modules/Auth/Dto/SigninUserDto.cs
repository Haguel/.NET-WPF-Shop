using System.ComponentModel.DataAnnotations;

namespace DOTNET_WPF_Shop.Modules.Auth.Dto
{
    class SigninUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "password")]
        [MinLength(6, ErrorMessage = "Password minimum length is 6 characters")]
        public string Password { get; set; }
    }
}
