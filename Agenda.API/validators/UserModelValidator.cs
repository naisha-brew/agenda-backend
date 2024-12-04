using Agenda.API.models;
using FluentValidation;

namespace Agenda.API.validators;

public class UserModelValidator : AbstractValidator<UserModel>
{
    public UserModelValidator()
    {
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email have to be valid");
        RuleFor(u => u.Userpwd).NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(20).WithMessage("Password must be between 6 and 20 characters");
        RuleFor(u => u.Username).NotEmpty().WithMessage("Username is required");
    }   
}