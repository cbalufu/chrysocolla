namespace CitizensPortal.Domain.Shared.Enums
{
    public enum ProjectStatus
    {
        Planning,
        Planned,
        Approved,
        InProgress,
        OnHold,
        Completed,
        Cancelled
    }

    public enum ProjectCategory
    {
        Roads,
        Water,
        Electricity,
        Parks,
        Buildings,
        Bridges,
        Drainage,
        StreetLighting,
        Other
    }

    public enum ServiceAreaType
    {
        Water,
        Electricity,
        WasteCollection,
        Emergency,
        Healthcare,
        Education,
        Other
    }
}
