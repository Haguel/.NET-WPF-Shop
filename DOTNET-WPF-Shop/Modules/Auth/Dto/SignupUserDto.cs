using System.ComponentModel.DataAnnotations;

namespace DOTNET_WPF_Shop.Modules.User
{
    class SignupUserDto
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "username")]
        [MinLength(6, ErrorMessage = "Username minimum length is 6 characters")]
        [MaxLength(50, ErrorMessage = "Username minimum length is 6 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "email")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "password")]
        [MinLength(6, ErrorMessage = "Password minimum length is 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confimation code is required")]
        [Display(Name = "code")]
        [StringLength(6, ErrorMessage = "Confimation code must be 6 characters length")]
        public string ConfirmationCode { get; set; }
    }
}
