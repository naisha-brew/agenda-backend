using Agenda.API.dto;
using Agenda.API.models;
using FluentValidation;

namespace Agenda.API.validators;

public class AgendaModelValidator : AbstractValidator<AgendaRequest>
{
    public AgendaModelValidator()
    {
        RuleFor(a => a.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email have to be a valid email");
        RuleFor(a => a.Phone).NotEmpty().WithMessage("Phone is required");
        RuleFor(a => a.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(a => a.UserEmail).NotEmpty().WithMessage("User email is required")
            .EmailAddress().WithMessage("User email have to be a valid email");
    }
}