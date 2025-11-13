using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Issues.UpdateIssue;

public sealed class UpdateIssueCommandHandler
    : IRequestHandler<UpdateIssueCommand, ErrorOr<UpdateIssueResponse>>
{
    private readonly ApplicationDbContext _context;

    public UpdateIssueCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UpdateIssueResponse>> Handle(
        UpdateIssueCommand request,
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

        // Verify ownership
        if (issue.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "Issue.AccessDenied",
                description: "You do not have permission to update this issue");
        }

        // Only allow updates if issue is not resolved
        if (issue.Status == "Resolved" || issue.Status == "Closed")
        {
            return Error.Validation(
                code: "Issue.CannotUpdate",
                description: "Cannot update a resolved or closed issue");
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Title))
        {
            issue.Title = request.Title;
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            issue.Description = request.Description;
        }

        if (!string.IsNullOrEmpty(request.Priority))
        {
            issue.Priority = request.Priority;
        }

        if (request.Location != null)
        {
            issue.Location = request.Location;
        }

        if (request.Latitude.HasValue)
        {
            issue.Latitude = request.Latitude;
        }

        if (request.Longitude.HasValue)
        {
            issue.Longitude = request.Longitude;
        }

        if (request.ImageUrls != null)
        {
            issue.ImageUrls = request.ImageUrls;
        }

        issue.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateIssueResponse(
            issue.Id,
            issue.Title,
            issue.Description,
            issue.Category,
            issue.Status,
            issue.Priority,
            issue.Location,
            issue.Latitude,
            issue.Longitude,
            issue.ImageUrls,
            issue.UpdatedAt.Value
        );
    }
}
