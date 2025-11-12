using FluentValidation;

namespace CitizensPortal.Api.Features.Issues.UpdateIssue;

public sealed class UpdateIssueCommandValidator : AbstractValidator<UpdateIssueCommand>
{
    public UpdateIssueCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Priority)
            .Must(BeValidPriority).WithMessage("Invalid priority. Valid priorities: Low, Medium, High, Critical")
            .When(x => !string.IsNullOrEmpty(x.Priority));

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

    private bool BeValidPriority(string? priority)
    {
        if (string.IsNullOrEmpty(priority)) return true;
        var validPriorities = new[] { "Low", "Medium", "High", "Critical" };
        return validPriorities.Contains(priority);
    }
}
