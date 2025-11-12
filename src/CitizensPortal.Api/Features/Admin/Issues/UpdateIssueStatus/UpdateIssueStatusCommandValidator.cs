using FluentValidation;

namespace CitizensPortal.Api.Features.Admin.Issues.UpdateIssueStatus;

public sealed class UpdateIssueStatusCommandValidator : AbstractValidator<UpdateIssueStatusCommand>
{
    public UpdateIssueStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(BeValidStatus).WithMessage("Invalid status. Valid statuses: Submitted, InProgress, Resolved, Closed");
    }

    private bool BeValidStatus(string status)
    {
        var validStatuses = new[] { "Submitted", "InProgress", "Resolved", "Closed" };
        return validStatuses.Contains(status);
    }
}
