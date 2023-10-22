namespace DOTNET_WPF_Shop.Modules.Auth.Dto
{
    class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
