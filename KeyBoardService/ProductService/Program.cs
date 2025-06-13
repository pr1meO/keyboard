using BusinessService.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.API.Configurations;
using ProductService.API.Contexts;
using ProductService.API.Extensions;
using ProductService.API.Middleware;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {

        var basePath = AppContext.BaseDirectory;
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(basePath, xmlFile);

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
        options.IncludeXmlComments(xmlPath);
    }
);

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRepositories();

builder.Services.AddUtilities();

builder.Services.AddBusiness();

builder.Services.AddValidators();

builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddAuthorization();

builder.Services.AddRabbit();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
