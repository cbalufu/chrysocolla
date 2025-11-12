using FluentValidation;

namespace CitizensPortal.Api.Features.Admin.Issues.AddInternalComment;

public sealed class AddInternalCommentCommandValidator : AbstractValidator<AddInternalCommentCommand>
{
    public AddInternalCommentCommandValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required")
            .MaximumLength(2000).WithMessage("Comment must not exceed 2000 characters");
    }
}
