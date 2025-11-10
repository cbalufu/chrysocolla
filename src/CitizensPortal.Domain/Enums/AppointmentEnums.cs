namespace CitizensPortal.Domain.Enums
{
    public enum AppointmentStatus
    {
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

    public enum DayOfWeek
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }
}
