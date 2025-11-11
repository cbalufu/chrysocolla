using FluentValidation;

namespace CitizensPortal.Api.Features.Citizens.Register;

public sealed class RegisterCitizenCommandValidator : AbstractValidator<RegisterCitizenCommand>
{
    public RegisterCitizenCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters")
            .Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("Invalid phone number format");

        RuleFor(x => x.NationalId)
            .MaximumLength(50).WithMessage("National ID must not exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.NationalId));

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(BeAValidAge).WithMessage("Citizen must be at least 18 years old");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters");
    }

    private static bool BeAValidAge(DateTime dateOfBirth)
    {
        var age = DateTime.UtcNow.Year - dateOfBirth.Year;
        if (DateTime.UtcNow < dateOfBirth.AddYears(age))
            age--;

        return age >= 18;
    }
}
