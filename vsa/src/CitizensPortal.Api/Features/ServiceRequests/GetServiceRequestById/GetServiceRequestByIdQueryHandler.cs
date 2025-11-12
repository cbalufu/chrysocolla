using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.ServiceRequests.GetServiceRequestById;

public sealed class GetServiceRequestByIdQueryHandler
    : IRequestHandler<GetServiceRequestByIdQuery, ErrorOr<GetServiceRequestByIdResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetServiceRequestByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetServiceRequestByIdResponse>> Handle(
        GetServiceRequestByIdQuery request,
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

        // Verify ownership
        if (serviceRequest.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "ServiceRequest.Forbidden",
                description: "You are not authorized to view this service request");
        }

        return new GetServiceRequestByIdResponse(
            serviceRequest.Id,
            serviceRequest.RequestNumber,
            serviceRequest.CitizenId,
            serviceRequest.ServiceType,
            serviceRequest.Title,
            serviceRequest.Description,
            serviceRequest.Priority,
            serviceRequest.Status,
            serviceRequest.Location,
            serviceRequest.PreferredServiceDate,
            serviceRequest.AssignedToUserId,
            serviceRequest.AssignedAt,
            serviceRequest.CompletedAt,
            serviceRequest.CompletionNotes,
            serviceRequest.CreatedAt,
            serviceRequest.UpdatedAt
        );
    }
}
