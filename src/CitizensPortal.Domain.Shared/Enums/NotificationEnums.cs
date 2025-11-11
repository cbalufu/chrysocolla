namespace CitizensPortal.Domain.Shared.Enums
{
    public enum NotificationType
    {
        BillGenerated,
        BillDue,
        BillOverdue,
        PaymentReceived,
        ApplicationStatusChanged,
        ApplicationApproved,
        ApplicationRejected,
        IssueStatusChanged,
        IssueResolved,
        TicketResponse,
        GeneralAnnouncement,
        SystemAlert
    }

    public enum NotificationPriority
    {
        Low,
        Normal,
        High,
        Urgent
    }
}
