using FluentValidation;

namespace CitizensPortal.Api.Features.Issues.CreateIssue;

public sealed class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
{
    public CreateIssueCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required")
            .MaximumLength(50).WithMessage("Category must not exceed 50 characters")
            .Must(BeValidCategory).WithMessage("Invalid category. Valid categories: Infrastructure, Safety, Cleanliness, Utilities, Transport, Environment, Other");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(BeValidPriority).WithMessage("Invalid priority. Valid priorities: Low, Medium, High, Critical");

        RuleFor(x => x.Location)
            .MaximumLength(500).WithMessage("Location must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180")
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.ImageUrls)
            .MaximumLength(2000).WithMessage("Image URLs must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.ImageUrls));
    }

    private bool BeValidCategory(string category)
    {
        var validCategories = new[] { "Infrastructure", "Safety", "Cleanliness", "Utilities", "Transport", "Environment", "Other" };
        return validCategories.Contains(category);
    }

    private bool BeValidPriority(string priority)
    {
        var validPriorities = new[] { "Low", "Medium", "High", "Critical" };
        return validPriorities.Contains(priority);
    }
}
