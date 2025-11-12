using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.Issues.AssignIssue;

public sealed class AssignIssueCommandHandler
    : IRequestHandler<AssignIssueCommand, ErrorOr<AssignIssueResponse>>
{
    private readonly ApplicationDbContext _context;

    public AssignIssueCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<AssignIssueResponse>> Handle(
        AssignIssueCommand request,
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

        issue.AssignedToUserId = request.AssignedToUserId;
        issue.UpdatedAt = DateTime.UtcNow;

        if (issue.Status == "Submitted")
        {
            issue.Status = "InProgress";
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new AssignIssueResponse(
            issue.Id,
            issue.AssignedToUserId.Value,
            issue.UpdatedAt.Value
        );
    }
}
