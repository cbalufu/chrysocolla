using Carter;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilBranding.GetBranding;

public sealed class GetCouncilBranding : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/council-branding",
            async (
                Guid? tenantId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCouncilBrandingQuery(tenantId);

                var result = await sender.Send(query, cancellationToken);

                return result.Match(
                    success => Results.Ok(success),
                    errors => Results.BadRequest(new
                    {
                        errors = errors.Select(e => new
                        {
                            code = e.Code,
                            description = e.Description
                        })
                    })
                );
            })
            .WithName("GetCouncilBranding")
            .WithTags("CouncilBranding")
            .WithOpenApi()
            .Produces<GetCouncilBrandingResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .WithSummary("Get council branding configuration")
            .WithDescription("Retrieves the branding configuration for a council including logo, colors, fonts, and contact information. Public endpoint - no auth required.");
    }
}
