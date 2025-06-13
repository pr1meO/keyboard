using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

        public List<CartProduct> CartProducts { get; set; } = [];

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
