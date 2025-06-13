namespace ProductService.API.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public List<CartProduct> CartProducts { get; set; } = [];
    }
}
