using BusinessService.Contracts.Products;
using FluentValidation;

namespace BusinessService.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        private const int MIN_LENGTH = 4;
        private const int MAX_LENGTH = 30;
        private const int MIN_NUMBER = 0;
        private const int MAX_LENGTH_DESCRIPTION = 255;

        public UpdateProductRequestValidator()
        {
            RuleFor(p => p.Name)
                .Length(MIN_LENGTH, MAX_LENGTH)
                .WithMessage($"Name length from {MIN_LENGTH} to {MAX_LENGTH} characters.");

            RuleFor(p => p.Description)
                .NotEmpty()
                .MaximumLength(MAX_LENGTH_DESCRIPTION)
                .WithMessage($"Description must not exceed {MAX_LENGTH_DESCRIPTION} characters.");

            RuleFor(p => p.Price)
                .GreaterThan(MIN_NUMBER)
                .WithMessage($"The product price must be more than {MIN_NUMBER}");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(MIN_NUMBER)
                .WithMessage($"The stock of the product must be greater than or equal to {MIN_NUMBER}.");

            RuleFor(p => p.CategoryId)
                .NotEmpty()
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Category ID is not valid.");

            RuleFor(p => p.BrandId)
                .NotEmpty()
                .Must(id => Guid.TryParse(id.ToString(), out _))
                .WithMessage("Brand ID is not valid.");
        }
    }
}
