namespace CitizensPortal.Domain.Enums
{
    public enum DocumentCategory
    {
        Personal,
        Property,
        Tax,
        Application,
        Certificate,
        License,
        Permit,
        Bill,
        Receipt,
        Contract,
        Legal,
        Other
    }

    public enum DocumentStatus
    {
        Active,
        Expired,
        Revoked,
        Archived
    }

    public enum DocumentAccessLevel
    {
        Private,
        Shared,
        Public
    }
}
