using FluentValidation;

namespace CitizensPortal.Api.Features.IdentityVerification.SubmitRequest;

public sealed class SubmitVerificationRequestValidator
    : AbstractValidator<SubmitVerificationRequestCommand>
{
    public SubmitVerificationRequestValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty()
            .WithMessage("Citizen ID is required");

        RuleFor(x => x.NationalIdType)
            .IsInEnum()
            .WithMessage("Valid national ID type is required");

        RuleFor(x => x.NationalIdValue)
            .NotEmpty()
            .WithMessage("National ID value is required")
            .MaximumLength(50)
            .WithMessage("National ID value must not exceed 50 characters");
    }
}
