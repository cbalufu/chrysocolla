using FluentValidation;

namespace CitizensPortal.Api.Features.Admin.CertificateRequests.UpdateRequestStatus;

public sealed class UpdateRequestStatusValidator : AbstractValidator<UpdateRequestStatusCommand>
{
    private static readonly string[] ValidStatuses =
    {
        "Submitted",
        "UnderReview",
        "Approved",
        "Ready",
        "Collected",
        "Rejected"
    };

    public UpdateRequestStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(status => ValidStatuses.Contains(status))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .When(x => x.Status == "Rejected")
            .WithMessage("Rejection reason is required when status is Rejected.");

        RuleFor(x => x.RejectionReason)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.RejectionReason))
            .WithMessage("Rejection reason cannot exceed 500 characters.");
    }
}
