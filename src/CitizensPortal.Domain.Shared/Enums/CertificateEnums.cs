namespace CitizensPortal.Domain.Shared.Enums
{
    public enum CertificateType
    {
        BirthCertificate,
        DeathCertificate,
        MarriageCertificate,
        DivorceDecree,
        PropertyCertificate,
        TaxClearanceCertificate,
        ResidenceCertificate,
        GoodConductCertificate,
        Other
    }

    public enum CertificateRequestStatus
    {
        Draft,
        Submitted,
        UnderReview,
        PendingPayment,
        Processing,
        ReadyForCollection,
        Collected,
        Mailed,
        Rejected,
        Cancelled
    }
}
