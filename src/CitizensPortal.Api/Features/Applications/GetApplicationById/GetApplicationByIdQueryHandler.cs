using CitizensPortal.Api.Infrastructure.Database;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Applications.GetApplicationById;

public sealed class GetApplicationByIdQueryHandler
    : IRequestHandler<GetApplicationByIdQuery, ErrorOr<GetApplicationByIdResponse>>
{
    private readonly ApplicationDbContext _context;

    public GetApplicationByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<GetApplicationByIdResponse>> Handle(
        GetApplicationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var application = await _context.Applications
            .Include(a => a.Documents)
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

        if (application == null)
        {
            return Error.NotFound(
                code: "Application.NotFound",
                description: "Application not found");
        }

        // Verify ownership
        if (application.CitizenId != request.CitizenId)
        {
            return Error.Forbidden(
                code: "Application.AccessDenied",
                description: "You do not have permission to view this application");
        }

        var documents = application.Documents
            .OrderBy(d => d.UploadedAt)
            .Select(d => new ApplicationDocumentDto(
                d.Id,
                d.DocumentType,
                d.FileName,
                d.FileUrl,
                d.FileSize,
                d.ContentType,
                d.UploadedAt
            ))
            .ToList();

        return new GetApplicationByIdResponse(
            application.Id,
            application.ApplicationNumber,
            application.ApplicationType,
            application.Title,
            application.Description,
            application.FormDataJson,
            application.Status,
            application.CreatedAt,
            application.SubmittedAt,
            application.ReviewedAt,
            application.ApprovedAt,
            application.ReviewNotes,
            documents
        );
    }
}
