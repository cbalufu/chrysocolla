using FluentValidation;

namespace CitizensPortal.Api.Features.EmergencyAlerts.CreateAlert;

public sealed class CreateAlertCommandValidator : AbstractValidator<CreateAlertCommand>
{
    public CreateAlertCommandValidator()
    {
        RuleFor(x => x.AlertType)
            .NotEmpty().WithMessage("Alert type is required")
            .MaximumLength(50).WithMessage("Alert type must not exceed 50 characters")
            .Must(BeValidAlertType).WithMessage("Invalid alert type. Valid types: Fire, Flood, Storm, Earthquake, Security, Health, Other");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(1000).WithMessage("Message must not exceed 1000 characters");

        RuleFor(x => x.Severity)
            .NotEmpty().WithMessage("Severity is required")
            .Must(BeValidSeverity).WithMessage("Invalid severity. Valid severities: Info, Warning, Severe, Critical");

        RuleFor(x => x.AffectedAreas)
            .MaximumLength(1000).WithMessage("Affected areas must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.AffectedAreas));

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future")
            .When(x => x.ExpiresAt.HasValue);
    }

    private bool BeValidAlertType(string alertType)
    {
        var validTypes = new[] { "Fire", "Flood", "Storm", "Earthquake", "Security", "Health", "Other" };
        return validTypes.Contains(alertType);
    }

    private bool BeValidSeverity(string severity)
    {
        var validSeverities = new[] { "Info", "Warning", "Severe", "Critical" };
        return validSeverities.Contains(severity);
    }
}
