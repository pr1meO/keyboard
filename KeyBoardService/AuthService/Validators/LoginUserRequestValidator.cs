using AuthService.API.Contracts.Users;
using FluentValidation;

namespace AuthService.API.Validators;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    private const int MIN_LENGTH = 4;
    private const int MAX_LENGTH = 30;

    public LoginUserRequestValidator()
    {
        RuleFor(l => l.Login)
            .Length(MIN_LENGTH, MAX_LENGTH)
            .WithMessage($"Login length from {MIN_LENGTH} to {MAX_LENGTH} characters.");

        RuleFor(l => l.Password)
            .MinimumLength(MIN_LENGTH)
            .WithMessage($"The minimum length of the password is {MIN_LENGTH} characters.");
    }
}
