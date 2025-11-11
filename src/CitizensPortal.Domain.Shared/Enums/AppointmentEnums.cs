namespace CitizensPortal.Domain.Shared.Enums
{
    public enum AppointmentStatus
    {
        Requested,
        Scheduled,
        Confirmed,
        InProgress,
        Completed,
        Cancelled,
        NoShow,
        Rescheduled
    }

    public enum AppointmentType
    {
        InPerson,
        Virtual,
        Phone
    }
}
