using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProductService.API.Configurations;
using ProductService.API.Contracts.Brands;
using ProductService.API.Contracts.CartProducts;
using ProductService.API.Contracts.Categories;
using ProductService.API.Contracts.Products;
using ProductService.API.Implementations.Repositories;
using ProductService.API.Implementations.Services;
using ProductService.API.Interfaces.Repositories;
using ProductService.API.Interfaces.Services;
using ProductService.API.Validators.Brands;
using ProductService.API.Validators.CartProducts;
using ProductService.API.Validators.Categories;
using ProductService.API.Validators.Products;
using System.Security.Claims;
using System.Text;

namespace BusinessService.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IBrandsRepository, BrandsRepository>();
            services.AddScoped<ICatalogsRepository, CatalogsRepository>();
            services.AddScoped<ICartProductsRepository, CartProductsRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }

        public static IServiceCollection AddUtilities(
            this IServiceCollection services)
        {
            services.AddScoped<IStockService, StockService>();

            return services;
        }

        public static IServiceCollection AddBusiness(
            this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartProductService, CartProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();

            return services;
        }

        public static IServiceCollection AddValidators(
            this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateBrandRequest>, CreateBrandRequestValidator>();
            services.AddScoped<IValidator<UpdateBrandRequest>, UpdateBrandRequestValidator>();

            services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryRequestValidator>();
            services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryRequestValidator>();

            services.AddScoped<IValidator<CreateProductRequest>, CreateProductRequestValidator>();
            services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductRequestValidator>();

            services.AddScoped<IValidator<CreateCartProductRequest>, CreateCartProductRequestValidator>();
            services.AddScoped<IValidator<UpdateCartProductRequest>, UpdateCartProductRequestValidator>();

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
