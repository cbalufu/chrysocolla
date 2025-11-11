namespace CitizensPortal.Domain.Shared.Enums
{
    public enum AlertSeverity
    {
        Info,
        Warning,
        Severe,
        Critical
    }

    public enum AlertType
    {
        Weather,
        Fire,
        Flood,
        Earthquake,
        PowerOutage,
        WaterOutage,
        PublicSafety,
        HealthEmergency,
        TrafficAlert,
        Other
    }

    public enum AlertStatus
    {
        Active,
        Resolved,
        Expired,
        Cancelled
    }
}
