using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public List<CartProduct> CartProducts { get; set; } = [];

        [ForeignKey(nameof(Brand))]
        public Guid BrandId { get; set; }
        public Brand? Brand { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
