using FluentValidation;

namespace CitizensPortal.Api.Features.Admin.Notifications.SendCustomNotification;

public sealed class SendCustomNotificationCommandValidator : AbstractValidator<SendCustomNotificationCommand>
{
    public SendCustomNotificationCommandValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required")
            .Must(BeValidType).WithMessage("Invalid type. Valid types: Bill, Application, Issue, Ticket, Announcement, EmergencyAlert");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(BeValidPriority).WithMessage("Invalid priority. Valid priorities: Low, Medium, High, Urgent");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(200).WithMessage("Subject must not exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(2000).WithMessage("Message must not exceed 2000 characters");
    }

    private bool BeValidType(string type)
    {
        var validTypes = new[] { "Bill", "Application", "Issue", "Ticket", "Announcement", "EmergencyAlert" };
        return validTypes.Contains(type);
    }

    private bool BeValidPriority(string priority)
    {
        var validPriorities = new[] { "Low", "Medium", "High", "Urgent" };
        return validPriorities.Contains(priority);
    }
}
