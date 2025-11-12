using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Issues.GetIssueById;

public sealed class GetIssueByIdQueryHandler
    : IRequestHandler<GetIssueByIdQuery, ErrorOr<GetIssueByIdResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetIssueByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetIssueByIdResponse>> Handle(
        GetIssueByIdQuery request,
        CancellationToken cancellationToken)
    {
        var issue = await _context.Issues
            .Include(i => i.Citizen)
            .Include(i => i.Comments)
            .FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

        if (issue == null)
        {
            return Error.NotFound(
                code: "Issue.NotFound",
                description: "Issue not found");
        }

        // Verify the issue belongs to the requesting citizen
        if (issue.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "Issue.AccessDenied",
                description: "You do not have permission to view this issue");
        }

        var comments = issue.Comments
            .Where(c => !c.IsInternal) // Only return public comments to citizens
            .OrderBy(c => c.CreatedAt)
            .Select(c => new IssueCommentDto(
                c.Id,
                c.UserId,
                c.UserName,
                c.Comment,
                c.CreatedAt
            ))
            .ToList();

        return new GetIssueByIdResponse(
            issue.Id,
            issue.CitizenId,
            new CitizenInfo(
                issue.Citizen.Id,
                issue.Citizen.FirstName,
                issue.Citizen.LastName,
                issue.Citizen.Email
            ),
            issue.Title,
            issue.Description,
            issue.Category,
            issue.Status,
            issue.Priority,
            issue.Location,
            issue.Latitude,
            issue.Longitude,
            issue.ImageUrls,
            issue.CreatedAt,
            issue.UpdatedAt,
            issue.ResolvedAt,
            comments
        );
    }
}
