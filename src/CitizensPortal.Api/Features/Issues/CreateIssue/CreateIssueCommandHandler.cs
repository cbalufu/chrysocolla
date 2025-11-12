using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Issues.CreateIssue;

public sealed class CreateIssueCommandHandler
    : IRequestHandler<CreateIssueCommand, ErrorOr<CreateIssueResponse>>
{
    private readonly ApplicationDbContext _context;

    public CreateIssueCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<CreateIssueResponse>> Handle(
        CreateIssueCommand request,
        CancellationToken cancellationToken)
    {
        // Verify citizen exists
        var citizenExists = await _context.Citizens
            .AnyAsync(c => c.Id == request.CitizenId, cancellationToken);

        if (!citizenExists)
        {
            return Error.NotFound(
                code: "Citizen.NotFound",
                description: "Citizen not found");
        }

        var issue = new Issue
        {
            Id = Guid.NewGuid(),
            CitizenId = request.CitizenId,
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Status = "Submitted", // Initial status
            Priority = request.Priority,
            Location = request.Location,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            ImageUrls = request.ImageUrls,
            CreatedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateIssueResponse(
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
            issue.CreatedAt
        );
    }
}
