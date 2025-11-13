using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.Applications.ReviewApplication;

public sealed class ReviewApplicationCommandHandler
    : IRequestHandler<ReviewApplicationCommand, ErrorOr<ReviewApplicationResponse>>
{
    private readonly ApplicationDbContext _context;

    public ReviewApplicationCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<ReviewApplicationResponse>> Handle(
        ReviewApplicationCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

        if (application == null)
        {
            return Error.NotFound(
                code: "Application.NotFound",
                description: "Application not found");
        }

        var now = DateTime.UtcNow;

        application.Status = request.Status;
        application.ReviewedByUserId = request.ReviewedByUserId;
        application.ReviewNotes = request.ReviewNotes;
        application.ReviewedAt = now;
        application.UpdatedAt = now;

        // Set ApprovedAt if status is Approved
        if (request.Status == "Approved" && !application.ApprovedAt.HasValue)
        {
            application.ApprovedAt = now;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new ReviewApplicationResponse(
            application.Id,
            application.Status,
            application.ReviewedByUserId.Value,
            application.ReviewNotes,
            application.ReviewedAt.Value,
            application.ApprovedAt
        );
    }
}
