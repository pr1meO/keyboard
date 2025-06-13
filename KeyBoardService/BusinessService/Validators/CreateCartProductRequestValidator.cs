using BusinessService.Contracts.CartProducts;
using FluentValidation;

namespace BusinessService.Validators
{
    public class CreateCartProductRequestValidator : AbstractValidator<CreateCartProductRequest>
    {
        private const int MIN_NUMBER = 0;

        public CreateCartProductRequestValidator()
        {
            RuleFor(cp => cp.Quantity)
                .GreaterThan(MIN_NUMBER)
                .WithMessage($"The number of products must be greater than {MIN_NUMBER}.");

            RuleFor(cp => cp.ProductId)
                .NotEmpty()
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Product ID is not valid.");
        }
    }
}
