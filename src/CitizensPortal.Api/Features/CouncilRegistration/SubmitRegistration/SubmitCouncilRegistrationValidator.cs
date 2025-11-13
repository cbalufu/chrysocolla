using FluentValidation;

namespace CitizensPortal.Api.Features.CouncilRegistration.SubmitRegistration;

public sealed class SubmitCouncilRegistrationValidator
    : AbstractValidator<SubmitCouncilRegistrationCommand>
{
    public SubmitCouncilRegistrationValidator()
    {
        RuleFor(x => x.CouncilName)
            .NotEmpty()
            .WithMessage("Council name is required")
            .MaximumLength(200)
            .WithMessage("Council name must not exceed 200 characters");

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .WithMessage("Registration number is required")
            .MaximumLength(100)
            .WithMessage("Registration number must not exceed 100 characters");

        RuleFor(x => x.Region)
            .NotEmpty()
            .WithMessage("Region is required")
            .MaximumLength(100)
            .WithMessage("Region must not exceed 100 characters");

        RuleFor(x => x.ContactName)
            .NotEmpty()
            .WithMessage("Contact name is required")
            .MaximumLength(200)
            .WithMessage("Contact name must not exceed 200 characters");

        RuleFor(x => x.ContactEmail)
            .NotEmpty()
            .WithMessage("Contact email is required")
            .EmailAddress()
            .WithMessage("Valid email address is required")
            .MaximumLength(255)
            .WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.ContactPhone)
            .NotEmpty()
            .WithMessage("Contact phone is required")
            .MaximumLength(50)
            .WithMessage("Phone must not exceed 50 characters");

        RuleFor(x => x.AdminName)
            .NotEmpty()
            .WithMessage("Admin name is required")
            .MaximumLength(200)
            .WithMessage("Admin name must not exceed 200 characters");

        RuleFor(x => x.AdminEmail)
            .NotEmpty()
            .WithMessage("Admin email is required")
            .EmailAddress()
            .WithMessage("Valid admin email address is required")
            .MaximumLength(255)
            .WithMessage("Admin email must not exceed 255 characters");

        RuleFor(x => x.PhysicalAddress)
            .NotEmpty()
            .WithMessage("Physical address is required")
            .MaximumLength(500)
            .WithMessage("Physical address must not exceed 500 characters");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required")
            .MaximumLength(20)
            .WithMessage("Postal code must not exceed 20 characters");
    }
}
