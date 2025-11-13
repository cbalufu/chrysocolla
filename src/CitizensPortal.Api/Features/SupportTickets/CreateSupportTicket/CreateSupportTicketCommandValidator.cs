using FluentValidation;

namespace CitizensPortal.Api.Features.SupportTickets.CreateSupportTicket;

public sealed class CreateSupportTicketCommandValidator : AbstractValidator<CreateSupportTicketCommand>
{
    public CreateSupportTicketCommandValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(200).WithMessage("Subject must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(5000).WithMessage("Description must not exceed 5000 characters");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required")
            .Must(BeValidCategory).WithMessage("Invalid category. Valid categories: Technical, Account, Payment, General");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required")
            .Must(BeValidPriority).WithMessage("Invalid priority. Valid priorities: Low, Medium, High");
    }

    private bool BeValidCategory(string category)
    {
        var validCategories = new[] { "Technical", "Account", "Payment", "General" };
        return validCategories.Contains(category);
    }

    private bool BeValidPriority(string priority)
    {
        var validPriorities = new[] { "Low", "Medium", "High" };
        return validPriorities.Contains(priority);
    }
}
