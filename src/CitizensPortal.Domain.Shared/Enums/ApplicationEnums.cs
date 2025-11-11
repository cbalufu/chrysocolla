namespace CitizensPortal.Domain.Shared.Enums
{
    public enum ApplicationType
    {
        Housing,
        BusinessLicense,
        BuildingPermit,
        ZoningPermit,
        EventPermit,
        ParkingPermit,
        HealthCertificate,
        FireSafetyCertificate,
        LiquorLicense,
        TradeLicense,
        Other
    }

    public enum ApplicationStatus
    {
        Draft,
        Submitted,
        UnderReview,
        PendingDocuments,
        PendingPayment,
        Approved,
        Rejected,
        Withdrawn,
        Expired
    }
}
