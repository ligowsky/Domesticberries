using System.Text.RegularExpressions;
using FluentValidation;

namespace Dberries.Warehouse.Presentation;

public partial class AuthRequestValidator : AbstractValidator<AuthRequestDto>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(EmailValidationRegex())
            .WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(PasswordValidationRegex())
            .WithMessage("Password must be at least 8 characters long and contain at least one special character");
    }

    [GeneratedRegex("^[a-zA-Z0-9_!#$%&’*+/=?`{|}~^.-]+@[a-zA-Z0-9.-]+$")]
    private static partial Regex EmailValidationRegex();
    
    [GeneratedRegex(@"^(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]).{8,}$")]
    private static partial Regex PasswordValidationRegex();
}