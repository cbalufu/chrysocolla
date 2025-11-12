using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Applications.SubmitApplication;

public sealed class SubmitApplicationCommandHandler
    : IRequestHandler<SubmitApplicationCommand, ErrorOr<SubmitApplicationResponse>>
{
    private readonly ApplicationDbContext _context;

    public SubmitApplicationCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<SubmitApplicationResponse>> Handle(
        SubmitApplicationCommand request,
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

        // Generate application number
        var applicationNumber = $"APP-{request.ApplicationType.ToUpper()[..3]}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var now = DateTime.UtcNow;

        var application = new Application
        {
            Id = Guid.NewGuid(),
            CitizenId = request.CitizenId,
            ApplicationType = request.ApplicationType,
            ApplicationNumber = applicationNumber,
            Title = request.Title,
            Description = request.Description,
            FormDataJson = request.FormDataJson,
            Status = "Submitted",
            CreatedAt = now,
            SubmittedAt = now
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync(cancellationToken);

        return new SubmitApplicationResponse(
            application.Id,
            application.ApplicationNumber,
            application.ApplicationType,
            application.Title,
            application.Description,
            application.Status,
            application.CreatedAt,
            application.SubmittedAt.Value
        );
    }
}
