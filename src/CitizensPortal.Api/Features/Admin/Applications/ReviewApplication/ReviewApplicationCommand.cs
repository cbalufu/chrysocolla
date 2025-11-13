using ErrorOr;
using MediatR;

namespace CitizensPortal.Api.Features.Admin.Applications.ReviewApplication;

public sealed record ReviewApplicationCommand(
    Guid ApplicationId,
    Guid ReviewedByUserId,
    string Status,
    string? ReviewNotes
) : IRequest<ErrorOr<ReviewApplicationResponse>>;
