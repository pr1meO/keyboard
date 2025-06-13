using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Lastname { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public List<RefreshToken> RefreshTokens { get; set; } = [];

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
