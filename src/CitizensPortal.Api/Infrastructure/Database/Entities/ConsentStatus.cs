namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Citizen's consent status for cross-council data sharing.
/// </summary>
public enum ConsentStatus
{
    /// <summary>Consent not yet provided</summary>
    NotProvided = 1,

    /// <summary>Consent granted for cross-council data sharing</summary>
    Granted = 2,

    /// <summary>Consent revoked by citizen</summary>
    Revoked = 3
}
