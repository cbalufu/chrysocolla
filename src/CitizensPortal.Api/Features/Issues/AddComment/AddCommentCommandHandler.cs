using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Issues.AddComment;

public sealed class AddCommentCommandHandler
    : IRequestHandler<AddCommentCommand, ErrorOr<AddCommentResponse>>
{
    private readonly ApplicationDbContext _context;

    public AddCommentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<AddCommentResponse>> Handle(
        AddCommentCommand request,
        CancellationToken cancellationToken)
    {
        // Verify issue exists
        var issue = await _context.Issues
            .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

        if (issue == null)
        {
            return Error.NotFound(
                code: "Issue.NotFound",
                description: "Issue not found");
        }

        // Get citizen info
        var citizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (citizen == null)
        {
            return Error.NotFound(
                code: "Citizen.NotFound",
                description: "Citizen not found");
        }

        // Create comment
        var comment = new IssueComment
        {
            Id = Guid.NewGuid(),
            IssueId = request.IssueId,
            UserId = request.CitizenId,
            UserName = $"{citizen.FirstName} {citizen.LastName}",
            Comment = request.Comment,
            IsInternal = false, // Citizen comments are always public
            CreatedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.IssueComments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return new AddCommentResponse(
            comment.Id,
            comment.IssueId,
            comment.UserId,
            comment.UserName,
            comment.Comment,
            comment.CreatedAt
        );
    }
}
