using BusinessService.Configurations;
using BusinessService.Interfaces.Auths.Tokens;
using BusinessService.Interfaces.Hashs;
using BusinessService.Interfaces.Repositories;
using BusinessService.Interfaces.Roles;
using BusinessService.Interfaces.Users;
using BusinessService.Repositories;
using BusinessService.Services.Auths.Tokens;
using BusinessService.Services.Hashs;
using BusinessService.Services.Roles;
using BusinessService.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BusinessService.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

            return services;
        }

        public static IServiceCollection AddUtilities(
            this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }

        public static IServiceCollection AddBusiness(
            this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }

        public static IServiceCollection AddTokens(
            this IServiceCollection services)
        {
            services.AddScoped<ISigningService, SigningService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(TokenOptions)).Get<TokenOptions>()!;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(AuthenticationSchemes.Access, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey
                    };
                })
                .AddJwtBearer(AuthenticationSchemes.Refresh, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = false,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = signingKey,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            return services;
        }
    }
}
