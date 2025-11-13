using FluentValidation;

namespace CitizensPortal.Api.Features.ServiceRequests.CreateServiceRequest;

public sealed class CreateServiceRequestCommandValidator : AbstractValidator<CreateServiceRequestCommand>
{
    public CreateServiceRequestCommandValidator()
    {
        RuleFor(x => x.ServiceType)
            .NotEmpty().WithMessage("Service type is required")
            .MaximumLength(100).WithMessage("Service type must not exceed 100 characters")
            .Must(BeValidServiceType).WithMessage("Invalid service type. Valid types: WasteCollection, StreetRepair, StreetLighting, TreeMaintenance, AnimalControl, Other");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(BeValidPriority).WithMessage("Invalid priority. Valid priorities: Low, Medium, High");

        RuleFor(x => x.Location)
            .MaximumLength(500).WithMessage("Location must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.PreferredServiceDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Preferred service date must be today or in the future")
            .When(x => x.PreferredServiceDate.HasValue);
    }

    private bool BeValidServiceType(string serviceType)
    {
        var validTypes = new[] { "WasteCollection", "StreetRepair", "StreetLighting", "TreeMaintenance", "AnimalControl", "Other" };
        return validTypes.Contains(serviceType);
    }

    private bool BeValidPriority(string priority)
    {
        var validPriorities = new[] { "Low", "Medium", "High" };
        return validPriorities.Contains(priority);
    }
}
