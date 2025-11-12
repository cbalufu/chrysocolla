using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.Issues.AddInternalComment;

public sealed class AddInternalCommentCommandHandler
    : IRequestHandler<AddInternalCommentCommand, ErrorOr<AddInternalCommentResponse>>
{
    private readonly ApplicationDbContext _context;

    public AddInternalCommentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<AddInternalCommentResponse>> Handle(
        AddInternalCommentCommand request,
        CancellationToken cancellationToken)
    {
        // Verify issue exists
        var issueExists = await _context.Issues
            .AnyAsync(i => i.Id == request.IssueId, cancellationToken);

        if (!issueExists)
        {
            return Error.NotFound(
                code: "Issue.NotFound",
                description: "Issue not found");
        }

        // Create comment
        var comment = new IssueComment
        {
            Id = Guid.NewGuid(),
            IssueId = request.IssueId,
            UserId = request.UserId,
            UserName = request.UserName,
            Comment = request.Comment,
            IsInternal = request.IsInternal,
            CreatedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.IssueComments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return new AddInternalCommentResponse(
            comment.Id,
            comment.IssueId,
            comment.UserId,
            comment.UserName,
            comment.Comment,
            comment.IsInternal,
            comment.CreatedAt
        );
    }
}
