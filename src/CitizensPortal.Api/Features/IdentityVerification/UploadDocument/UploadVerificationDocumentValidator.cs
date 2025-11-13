using FluentValidation;

namespace CitizensPortal.Api.Features.IdentityVerification.UploadDocument;

public sealed class UploadVerificationDocumentValidator
    : AbstractValidator<UploadVerificationDocumentCommand>
{
    private static readonly string[] AllowedContentTypes = new[]
    {
        "image/jpeg",
        "image/jpg",
        "image/png",
        "application/pdf"
    };

    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    public UploadVerificationDocumentValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty()
            .WithMessage("Citizen ID is required");

        RuleFor(x => x.RequestId)
            .NotEmpty()
            .WithMessage("Request ID is required");

        RuleFor(x => x.DocumentType)
            .IsInEnum()
            .WithMessage("Valid document type is required");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name is required")
            .MaximumLength(255)
            .WithMessage("File name must not exceed 255 characters");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .WithMessage("Content type is required")
            .Must(ct => AllowedContentTypes.Contains(ct.ToLowerInvariant()))
            .WithMessage($"Content type must be one of: {string.Join(", ", AllowedContentTypes)}");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("File size must be greater than 0")
            .LessThanOrEqualTo(MaxFileSize)
            .WithMessage($"File size must not exceed {MaxFileSize / 1024 / 1024}MB");
    }
}
