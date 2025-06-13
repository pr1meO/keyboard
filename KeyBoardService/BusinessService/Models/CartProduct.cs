using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.Models
{
    public class CartProduct
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(Cart))]
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
