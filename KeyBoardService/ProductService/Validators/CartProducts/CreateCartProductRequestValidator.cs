using FluentValidation;
using ProductService.API.Contracts.CartProducts;

namespace ProductService.API.Validators.CartProducts;

public class CreateCartProductRequestValidator : AbstractValidator<CreateCartProductRequest>
{
    private const int MIN_NUMBER = 0;

    public CreateCartProductRequestValidator()
    {
        RuleFor(cp => cp.Quantity)
            .GreaterThan(MIN_NUMBER)
            .WithMessage($"The number of products must be greater than {MIN_NUMBER}.");
    }
}
