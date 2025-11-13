using FluentValidation;

namespace CitizensPortal.Api.Features.CouncilRegistration.ReviewRegistration;

public sealed class ReviewCouncilRegistrationValidator
    : AbstractValidator<ReviewCouncilRegistrationCommand>
{
    public ReviewCouncilRegistrationValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty()
            .WithMessage("Request ID is required");

        RuleFor(x => x.ReviewerUserId)
            .NotEmpty()
            .WithMessage("Reviewer user ID is required");

        RuleFor(x => x.ReviewerName)
            .NotEmpty()
            .WithMessage("Reviewer name is required")
            .MaximumLength(200)
            .WithMessage("Reviewer name must not exceed 200 characters");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .When(x => !x.Approve)
            .WithMessage("Rejection reason is required when rejecting a request")
            .MaximumLength(1000)
            .WithMessage("Rejection reason must not exceed 1000 characters");
    }
}
