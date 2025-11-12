using FluentValidation;

namespace CitizensPortal.Api.Features.Admin.Applications.ReviewApplication;

public sealed class ReviewApplicationCommandValidator : AbstractValidator<ReviewApplicationCommand>
{
    public ReviewApplicationCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(BeValidStatus).WithMessage("Invalid status. Valid statuses: Approved, Rejected, UnderReview");

        RuleFor(x => x.ReviewNotes)
            .MaximumLength(2000).WithMessage("Review notes must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.ReviewNotes));
    }

    private bool BeValidStatus(string status)
    {
        var validStatuses = new[] { "Approved", "Rejected", "UnderReview" };
        return validStatuses.Contains(status);
    }
}
