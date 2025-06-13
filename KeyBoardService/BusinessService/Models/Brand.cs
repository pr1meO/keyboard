namespace BusinessService.Models
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Product> Products { get; set; } = [];
    }
}
