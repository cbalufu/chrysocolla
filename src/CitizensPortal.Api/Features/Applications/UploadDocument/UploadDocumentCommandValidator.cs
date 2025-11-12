using FluentValidation;

namespace CitizensPortal.Api.Features.Applications.UploadDocument;

public sealed class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("Document type is required")
            .MaximumLength(100).WithMessage("Document type must not exceed 100 characters");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required")
            .MaximumLength(255).WithMessage("File name must not exceed 255 characters");

        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("File URL is required")
            .MaximumLength(2000).WithMessage("File URL must not exceed 2000 characters");

        RuleFor(x => x.FileSize)
            .GreaterThan(0).WithMessage("File size must be greater than 0")
            .LessThan(52428800).WithMessage("File size must be less than 50MB"); // 50MB limit

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Content type is required")
            .MaximumLength(100).WithMessage("Content type must not exceed 100 characters");
    }
}
