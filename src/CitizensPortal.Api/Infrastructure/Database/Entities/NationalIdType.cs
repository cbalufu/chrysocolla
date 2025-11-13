namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Types of national identifiers supported for citizen identification.
/// </summary>
public enum NationalIdType
{
    /// <summary>National ID number</summary>
    NationalId = 1,

    /// <summary>Tax Identification Number (TIN)</summary>
    TIN = 2,

    /// <summary>Passport number</summary>
    Passport = 3
}
