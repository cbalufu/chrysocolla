using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.Issues.UpdateIssueStatus;

public sealed class UpdateIssueStatusCommandHandler
    : IRequestHandler<UpdateIssueStatusCommand, ErrorOr<UpdateIssueStatusResponse>>
{
    private readonly ApplicationDbContext _context;

    public UpdateIssueStatusCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UpdateIssueStatusResponse>> Handle(
        UpdateIssueStatusCommand request,
        CancellationToken cancellationToken)
    {
        var issue = await _context.Issues
            .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

        if (issue == null)
        {
            return Error.NotFound(
                code: "Issue.NotFound",
                description: "Issue not found");
        }

        issue.Status = request.Status;
        issue.UpdatedAt = DateTime.UtcNow;

        // Set ResolvedAt if status is Resolved or Closed
        if ((request.Status == "Resolved" || request.Status == "Closed") && !issue.ResolvedAt.HasValue)
        {
            issue.ResolvedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateIssueStatusResponse(
            issue.Id,
            issue.Status,
            issue.ResolvedAt,
            issue.UpdatedAt.Value
        );
    }
}
