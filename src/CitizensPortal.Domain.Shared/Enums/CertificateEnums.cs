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
        Issued,
        ReadyForCollection,
        Collected,
        Mailed,
        Rejected,
        Cancelled
    }
}
