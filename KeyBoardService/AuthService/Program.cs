using AuthService.API.Configurations;
using AuthService.API.Context;
using AuthService.API.Extensions;
using AuthService.API.Middlewares;
using BusinessService.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    var basePath = AppContext.BaseDirectory;


    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(basePath, xmlFile);


    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidators();

builder.Services.AddHashers();

builder.Services.AddRepositories();

builder.Services.Configure<TokenOptions>(
    configuration.GetSection(nameof(TokenOptions)));

builder.Services.AddClaims();

builder.Services.AddTokens();

builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddBusiness();

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
