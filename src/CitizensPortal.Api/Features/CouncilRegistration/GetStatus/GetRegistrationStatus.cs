using Carter;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilRegistration.GetStatus;

public sealed class GetRegistrationStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/council-registration/status/{referenceNumber}",
            async (
                string referenceNumber,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetRegistrationStatusQuery(referenceNumber);

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
            .WithName("GetRegistrationStatus")
            .WithTags("CouncilRegistration")
            .WithOpenApi()
            .Produces<GetRegistrationStatusResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .WithSummary("Get registration status by reference number (Public endpoint)")
            .WithDescription("Check the status of a council registration request using the reference number provided during submission.");
    }
}
