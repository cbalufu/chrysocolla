using FluentValidation;

namespace CitizensPortal.Api.Features.CertificateRequests.SubmitCertificateRequest;

public sealed class SubmitCertificateRequestCommandValidator : AbstractValidator<SubmitCertificateRequestCommand>
{
    public SubmitCertificateRequestCommandValidator()
    {
        RuleFor(x => x.CertificateType)
            .NotEmpty().WithMessage("Certificate type is required")
            .Must(BeValidCertificateType).WithMessage("Invalid certificate type. Valid types: Birth, Death, Marriage, Residence, GoodConduct, TaxClearance");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("Purpose is required")
            .MaximumLength(500).WithMessage("Purpose must not exceed 500 characters");

        RuleFor(x => x.DeliveryMethod)
            .NotEmpty().WithMessage("Delivery method is required")
            .Must(BeValidDeliveryMethod).WithMessage("Invalid delivery method. Valid methods: Collection, Email, Postal");
    }

    private bool BeValidCertificateType(string type)
    {
        var validTypes = new[] { "Birth", "Death", "Marriage", "Residence", "GoodConduct", "TaxClearance" };
        return validTypes.Contains(type);
    }

    private bool BeValidDeliveryMethod(string method)
    {
        var validMethods = new[] { "Collection", "Email", "Postal" };
        return validMethods.Contains(method);
    }
}
