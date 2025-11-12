using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Applications.SubmitApplication;

public sealed record SubmitApplicationCommand(
    Guid CitizenId,
    string ApplicationType,
    string Title,
    string Description,
    string FormDataJson
) : IRequest<ErrorOr<SubmitApplicationResponse>>;
