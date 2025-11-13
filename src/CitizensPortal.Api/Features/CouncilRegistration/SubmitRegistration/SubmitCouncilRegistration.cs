using Carter;
using MediatR;

namespace CitizensPortal.Api.Features.CouncilRegistration.SubmitRegistration;

public sealed class SubmitCouncilRegistration : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/council-registration/submit",
            async (
                SubmitCouncilRegistrationDto dto,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new SubmitCouncilRegistrationCommand(
                    dto.CouncilName,
                    dto.RegistrationNumber,
                    dto.Region,
                    dto.ContactName,
                    dto.ContactEmail,
                    dto.ContactPhone,
                    dto.AdminName,
                    dto.AdminEmail,
                    dto.PhysicalAddress,
                    dto.City,
                    dto.PostalCode
                );

                var result = await sender.Send(command, cancellationToken);

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
            .WithName("SubmitCouncilRegistration")
            .WithTags("CouncilRegistration")
            .WithOpenApi()
            .Produces<SubmitCouncilRegistrationResponse>(StatusCodes.Status200OK)
            .Produces<object>(StatusCodes.Status400BadRequest)
            .WithSummary("Submit council registration request (Public endpoint)")
            .WithDescription("Allows local authorities to register on the platform. No authentication required. Returns a reference number for tracking the application status.");
    }
}

public sealed record SubmitCouncilRegistrationDto(
    string CouncilName,
    string RegistrationNumber,
    string Region,
    string ContactName,
    string ContactEmail,
    string ContactPhone,
    string AdminName,
    string AdminEmail,
    string PhysicalAddress,
    string City,
    string PostalCode
);
