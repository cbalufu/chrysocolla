using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.ServiceRequests.CreateServiceRequest;

public sealed class CreateServiceRequestCommandHandler
    : IRequestHandler<CreateServiceRequestCommand, ErrorOr<CreateServiceRequestResponse>>
{
    private readonly ApplicationDbContext _context;

    public CreateServiceRequestCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<CreateServiceRequestResponse>> Handle(
        CreateServiceRequestCommand request,
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

        // Generate request number
        var requestNumber = $"SR-{request.ServiceType.ToUpper()[..3]}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var serviceRequest = new ServiceRequest
        {
            Id = Guid.NewGuid(),
            CitizenId = request.CitizenId,
            RequestNumber = requestNumber,
            ServiceType = request.ServiceType,
            Title = request.Title,
            Description = request.Description,
            Status = "Submitted",
            Priority = request.Priority,
            Location = request.Location,
            PreferredServiceDate = request.PreferredServiceDate,
            CreatedAt = DateTime.UtcNow
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.ServiceRequests.Add(serviceRequest);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateServiceRequestResponse(
            serviceRequest.Id,
            serviceRequest.RequestNumber,
            serviceRequest.ServiceType,
            serviceRequest.Title,
            serviceRequest.Status,
            serviceRequest.Priority,
            serviceRequest.CreatedAt
        );
    }
}
