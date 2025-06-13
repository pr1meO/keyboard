using Microsoft.EntityFrameworkCore;
using ProductService.API.Models;

namespace ProductService.API.Contexts
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
    }
}