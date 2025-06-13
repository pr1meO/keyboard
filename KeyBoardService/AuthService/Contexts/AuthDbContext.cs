using AuthService.API.Enums;
using AuthService.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuthService.API.Context
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                Enum
                .GetValues<RoleType>()
                .Select(r => new Role
                {
                    Id = (int)r,
                    Name = r.ToString()
                }));
        }
    }
}