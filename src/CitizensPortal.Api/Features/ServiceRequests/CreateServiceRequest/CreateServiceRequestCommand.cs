using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.ServiceRequests.CreateServiceRequest;

public sealed record CreateServiceRequestCommand(
    Guid CitizenId,
    string ServiceType,
    string Title,
    string Description,
    string Priority,
    string? Location,
    DateTime? PreferredServiceDate
) : IRequest<ErrorOr<CreateServiceRequestResponse>>;
