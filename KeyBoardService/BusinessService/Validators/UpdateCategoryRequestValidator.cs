using BusinessService.Contracts.Categories;
using FluentValidation;

namespace BusinessService.Validators
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        private const int MIN_LENGTH = 4;
        private const int MAX_LENGTH = 30;

        public UpdateCategoryRequestValidator()
        {
            RuleFor(c => c.Name)
                .Length(MIN_LENGTH, MAX_LENGTH)
                .WithMessage($"Name length from {MIN_LENGTH} to {MAX_LENGTH} characters.");
        }
    }
}
