namespace CitizensPortal.Api.Infrastructure.Database.Entities;

/// <summary>
/// Method used to verify a citizen's identity.
/// </summary>
public enum VerificationMethod
{
    /// <summary>Self-service online document upload</summary>
    SelfService = 1,

    /// <summary>In-person verification at council office</summary>
    InPerson = 2,

    /// <summary>Bulk import from existing system</summary>
    BulkImport = 3
}
