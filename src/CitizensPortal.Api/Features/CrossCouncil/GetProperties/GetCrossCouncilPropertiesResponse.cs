namespace CitizensPortal.Api.Features.CrossCouncil.GetProperties;

public sealed record GetCrossCouncilPropertiesResponse(
    List<AggregatedPropertyDto> Properties,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);

public sealed record AggregatedPropertyDto(
    Guid Id,
    Guid TenantId,
    string TenantName,
    Guid CitizenId,
    string PropertyNumber,
    string PropertyType,
    string Address,
    decimal? AssessedValue,
    decimal? AnnualTaxAmount,
    string Status,
    DateTime CreatedAt
);
