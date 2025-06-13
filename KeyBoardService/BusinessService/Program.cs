using BusinessService;
using BusinessService.Configurations;
using BusinessService.Context;
using BusinessService.Contracts;
using BusinessService.Contracts.Brands;
using BusinessService.Contracts.CartProducts;
using BusinessService.Contracts.Categories;
using BusinessService.Contracts.Products;
using BusinessService.Extensions;
using BusinessService.Interfaces.Auths;
using BusinessService.Interfaces.Auths.Claims;
using BusinessService.Interfaces.Cart;
using BusinessService.Interfaces.Products;
using BusinessService.Interfaces.Repositories;
using BusinessService.Repositories;
using BusinessService.Services;
using BusinessService.Services.Auths;
using BusinessService.Services.Auths.Claims;
using BusinessService.Services.Products;
using BusinessService.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2",
            new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            }
        );
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// ������������
builder.Services.AddRepositories();
builder.Services.AddUtilities();
builder.Services.AddBusiness();

// ������������
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClaimProvider, ClaimProvider>();
builder.Services.AddTokens();

// ������������
builder.Services.AddScoped<IBrandsRepository, BrandsRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();

// ������������
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// ������������
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// ������������
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

// ������������
builder.Services.AddScoped<ICartProductsRepository, CartProductsRepository>();
builder.Services.AddScoped<ICartProductService, CartProductService>();

// ������������
builder.Services.AddScoped<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
builder.Services.AddScoped<IValidator<LoginUserRequest>, LoginUserRequestValidator>();

// ������������
builder.Services.AddScoped<IValidator<CreateBrandRequest>, CreateBrandRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateBrandRequest>, UpdateBrandRequestValidator>();

// ������������
builder.Services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryRequestValidator>();

// ������������
builder.Services.AddScoped<IValidator<CreateProductRequest>, CreateProductRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductRequestValidator>();

// ������������
builder.Services.AddScoped<IValidator<CreateCartProductRequest>, CreateCartProductRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateCartProductRequest>, UpdateCartProductRequestValidator>();

// ������������
builder.Services.AddScoped<IStockService, StockService>();

builder.Services.Configure<TokenOptions>(
    configuration.GetSection(nameof(TokenOptions)));

builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

/*app.UseMiddleware<ExceptionMiddleware>();*/

app.MapControllers();

app.Run();