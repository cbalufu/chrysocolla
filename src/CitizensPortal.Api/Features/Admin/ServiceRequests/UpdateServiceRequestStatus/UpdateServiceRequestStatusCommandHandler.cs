using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Admin.ServiceRequests.UpdateServiceRequestStatus;

public sealed class UpdateServiceRequestStatusCommandHandler
    : IRequestHandler<UpdateServiceRequestStatusCommand, ErrorOr<UpdateServiceRequestStatusResponse>>
{
    private readonly ApplicationDbContext _context;

    public UpdateServiceRequestStatusCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<UpdateServiceRequestStatusResponse>> Handle(
        UpdateServiceRequestStatusCommand request,
        CancellationToken cancellationToken)
    {
        var serviceRequest = await _context.ServiceRequests
            .FirstOrDefaultAsync(sr => sr.Id == request.ServiceRequestId, cancellationToken);

        if (serviceRequest == null)
        {
            return Error.NotFound(
                code: "ServiceRequest.NotFound",
                description: "Service request not found");
        }

        var now = DateTime.UtcNow;

        serviceRequest.Status = request.Status;
        serviceRequest.UpdatedAt = now;

        // Handle assignment
        if (request.AssignedToUserId.HasValue && serviceRequest.AssignedToUserId != request.AssignedToUserId)
        {
            serviceRequest.AssignedToUserId = request.AssignedToUserId;
            serviceRequest.AssignedAt = now;
        }

        // Handle completion
        if (request.Status == "Completed" && !serviceRequest.CompletedAt.HasValue)
        {
            serviceRequest.CompletedAt = now;
            serviceRequest.CompletionNotes = request.CompletionNotes;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateServiceRequestStatusResponse(
            serviceRequest.Id,
            serviceRequest.Status,
            serviceRequest.AssignedToUserId,
            serviceRequest.AssignedAt,
            serviceRequest.CompletedAt,
            serviceRequest.CompletionNotes,
            serviceRequest.UpdatedAt
        );
    }
}
