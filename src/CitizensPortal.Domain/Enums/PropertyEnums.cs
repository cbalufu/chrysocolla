namespace CitizensPortal.Domain.Enums
{
    public enum PropertyType
    {
        Residential,
        Commercial,
        Industrial,
        Agricultural,
        Vacant,
        MixedUse
    }

    public enum PropertyStatus
    {
        Active,
        Inactive,
        UnderConstruction,
        Demolished,
        Disputed
    }

    public enum PropertyOwnershipType
    {
        Owner,
        Tenant,
        CoOwner,
        Leaseholder
    }

    public enum ValuationMethod
    {
        MarketValue,
        ReplacementCost,
        IncomeApproach,
        ComparativeSales,
        Municipal
    }
}
