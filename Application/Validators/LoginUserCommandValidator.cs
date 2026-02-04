using Application.Features.Users.Command;
using FluentValidation;

namespace Application.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Email field cannt be empty.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password field cannot be empty.");
    }
}
