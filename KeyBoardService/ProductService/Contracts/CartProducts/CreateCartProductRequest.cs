namespace ProductService.API.Contracts.CartProducts
{
    public class CreateCartProductRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
