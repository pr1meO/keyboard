namespace AuthService.API.Contracts.Users
{
    public class LoginUserRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
