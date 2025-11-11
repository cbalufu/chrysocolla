namespace CitizensPortal.Domain.Shared.Enums
{
    public enum TicketCategory
    {
        TechnicalSupport,
        BillingInquiry,
        ApplicationSupport,
        AccountIssue,
        GeneralInquiry,
        Complaint,
        FeatureRequest,
        Other
    }

    public enum TicketPriority
    {
        Low,
        Normal,
        High,
        Urgent
    }

    public enum TicketStatus
    {
        Pending,
        Open,
        InProgress,
        WaitingForCustomer,
        WaitingForStaff,
        Resolved,
        Closed,
        Reopened
    }
}
