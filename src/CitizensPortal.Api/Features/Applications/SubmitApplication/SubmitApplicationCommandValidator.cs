using FluentValidation;

namespace CitizensPortal.Api.Features.Applications.SubmitApplication;

public sealed class SubmitApplicationCommandValidator : AbstractValidator<SubmitApplicationCommand>
{
    public SubmitApplicationCommandValidator()
    {
        RuleFor(x => x.ApplicationType)
            .NotEmpty().WithMessage("Application type is required")
            .MaximumLength(100).WithMessage("Application type must not exceed 100 characters")
            .Must(BeValidApplicationType).WithMessage("Invalid application type. Valid types: BuildingPermit, BusinessLicense, ResidencyPermit, ParkingPermit, EventPermit, Other");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.FormDataJson)
            .NotEmpty().WithMessage("Form data is required")
            .Must(BeValidJson).WithMessage("Form data must be valid JSON");
    }

    private bool BeValidApplicationType(string applicationType)
    {
        var validTypes = new[] { "BuildingPermit", "BusinessLicense", "ResidencyPermit", "ParkingPermit", "EventPermit", "Other" };
        return validTypes.Contains(applicationType);
    }

    private bool BeValidJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return false;

        try
        {
            System.Text.Json.JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
