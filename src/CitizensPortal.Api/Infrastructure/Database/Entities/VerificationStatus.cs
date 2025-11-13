namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Status of identity verification for a citizen.
/// </summary>
public enum VerificationStatus
{
    /// <summary>Verification pending review</summary>
    Pending = 1,

    /// <summary>Identity verified and approved</summary>
    Verified = 2,

    /// <summary>Verification rejected</summary>
    Rejected = 3
}
