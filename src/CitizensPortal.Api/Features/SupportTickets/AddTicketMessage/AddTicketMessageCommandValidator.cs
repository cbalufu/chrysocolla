using FluentValidation;

namespace CitizensPortal.Api.Features.SupportTickets.AddTicketMessage;

public sealed class AddTicketMessageCommandValidator : AbstractValidator<AddTicketMessageCommand>
{
    public AddTicketMessageCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(5000).WithMessage("Message must not exceed 5000 characters");

        RuleFor(x => x.AttachmentUrls)
            .Must(BeValidJson).WithMessage("Attachment URLs must be valid JSON")
            .When(x => !string.IsNullOrEmpty(x.AttachmentUrls));
    }

    private bool BeValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return true;
        try
        {
            System.Text.Json.JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
