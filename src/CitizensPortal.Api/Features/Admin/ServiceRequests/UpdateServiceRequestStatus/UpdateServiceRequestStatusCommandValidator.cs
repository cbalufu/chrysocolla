using FluentValidation;

namespace CitizensPortal.Api.Features.Admin.ServiceRequests.UpdateServiceRequestStatus;

public sealed class UpdateServiceRequestStatusCommandValidator
    : AbstractValidator<UpdateServiceRequestStatusCommand>
{
    public UpdateServiceRequestStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(BeValidStatus).WithMessage("Invalid status. Valid statuses: Submitted, Assigned, InProgress, Completed, Cancelled");

        RuleFor(x => x.CompletionNotes)
            .MaximumLength(2000).WithMessage("Completion notes must not exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.CompletionNotes));
    }

    private bool BeValidStatus(string status)
    {
        var validStatuses = new[] { "Submitted", "Assigned", "InProgress", "Completed", "Cancelled" };
        return validStatuses.Contains(status);
    }
}
