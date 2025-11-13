using FluentValidation;

namespace CitizensPortal.Api.Features.FederatedProfile.CreateOrLink;

public sealed class CreateOrLinkFederatedProfileCommandValidator
    : AbstractValidator<CreateOrLinkFederatedProfileCommand>
{
    public CreateOrLinkFederatedProfileCommandValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty()
            .WithMessage("Citizen ID is required");

        RuleFor(x => x.NationalId)
            .NotEmpty()
            .WithMessage("National ID is required")
            .MinimumLength(5)
            .WithMessage("National ID must be at least 5 characters")
            .MaximumLength(50)
            .WithMessage("National ID cannot exceed 50 characters");

        RuleFor(x => x.NationalIdType)
            .IsInEnum()
            .WithMessage("Invalid National ID Type");
    }
}
