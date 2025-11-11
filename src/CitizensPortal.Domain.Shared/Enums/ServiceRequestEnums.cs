namespace CitizensPortal.Domain.Shared.Enums
{
    public enum ServiceRequestType
    {
        BulkWastePickup,
        GardenWastePickup,
        StreetMaintenance,
        TreeTrimming,
        PotholeRepair,
        StreetLightRepair,
        DrainageCleaning,
        PestControl,
        BuildingInspection,
        HealthInspection,
        FireSafetyInspection,
        Other
    }

    public enum ServiceRequestStatus
    {
        Submitted,
        Acknowledged,
        Scheduled,
        InProgress,
        Completed,
        Cancelled,
        OnHold
    }

    public enum ServiceRequestPriority
    {
        Low,
        Medium,
        High,
        Urgent
    }
}
