using FluentValidation;

namespace CitizensPortal.Api.Features.Issues.AddComment;

public sealed class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required")
            .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters");
    }
}
