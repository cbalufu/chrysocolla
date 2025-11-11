namespace CitizensPortal.Domain.Shared.Enums
{
    public enum BillType
    {
        PropertyRates,
        Water,
        Electricity,
        Waste,
        Sewerage,
        Other
    }

    public enum BillStatus
    {
        Unpaid,
        PartiallyPaid,
        Paid,
        Overdue,
        Disputed,
        Cancelled
    }
}
