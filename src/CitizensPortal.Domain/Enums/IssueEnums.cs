namespace CitizensPortal.Domain.Enums
{
    public enum IssueCategory
    {
        RoadMaintenance,
        StreetLighting,
        WaterAndSanitation,
        WasteManagement,
        PublicSafety,
        Parks,
        Noise,
        IllegalDumping,
        TrafficSignals,
        Other
    }

    public enum IssuePriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum IssueStatus
    {
        New,
        UnderReview,
        Assigned,
        InProgress,
        OnHold,
        Resolved,
        Closed,
        Rejected
    }
}
