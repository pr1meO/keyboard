using AuthService.API.Contracts.Users;
using FluentValidation;

namespace AuthService.API.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    private const int MIN_LENGTH = 4;
    private const int MAX_LENGTH = 30;
    private string PHONE_NUMBER = @"^\+7 (9\d{2}) \d{3}-\d{2}-\d{2}";

    public RegisterUserRequestValidator()
    {
        RuleFor(r => r.Lastname)
            .Length(MIN_LENGTH, MAX_LENGTH)
            .WithMessage($"Last name length from {MIN_LENGTH} to {MAX_LENGTH} characters.");

        RuleFor(r => r.Firstname)
            .Length(MIN_LENGTH, MAX_LENGTH)
            .WithMessage($"First name length from {MIN_LENGTH} to {MAX_LENGTH} characters.");

        RuleFor(r => r.PhoneNumber)
            .Matches(PHONE_NUMBER)
            .WithMessage("Incorrect phone number format.");

        RuleFor(r => r.Login)
            .Length(MIN_LENGTH, MAX_LENGTH)
            .WithMessage($"Login length from {MIN_LENGTH} to {MAX_LENGTH} characters.");

        RuleFor(r => r.Password)
            .MinimumLength(MIN_LENGTH)
            .WithMessage($"The minimum length of the password is {MIN_LENGTH} characters.");
    }
}
