using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a property in the municipal system
    /// </summary>
    public class Property : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string PropertyReference { get; set; } // Unique property ID in the municipality
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Suburb { get; set; }
        public string Ward { get; set; }

        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; }

        // Physical details
        public decimal? LandSize { get; set; } // in square meters
        public decimal? BuildingSize { get; set; } // in square meters
        public int? NumberOfBedrooms { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? YearBuilt { get; set; }

        // GPS coordinates
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Valuation
        public decimal? CurrentValuation { get; set; }
        public DateTime? LastValuationDate { get; set; }

        // Ownership
        public ICollection<PropertyOwnership> Ownerships { get; set; }
        public ICollection<PropertyValuation> ValuationHistory { get; set; }
        public ICollection<PropertyTaxHistory> TaxHistory { get; set; }

        protected Property()
        {
            Ownerships = new List<PropertyOwnership>();
            ValuationHistory = new List<PropertyValuation>();
            TaxHistory = new List<PropertyTaxHistory>();
        }

        public Property(
            Guid id,
            string propertyReference,
            string address,
            PropertyType type,
            Guid? tenantId = null) : base(id)
        {
            PropertyReference = propertyReference;
            Address = address;
            Type = type;
            Status = PropertyStatus.Active;
            TenantId = tenantId;
            Ownerships = new List<PropertyOwnership>();
            ValuationHistory = new List<PropertyValuation>();
            TaxHistory = new List<PropertyTaxHistory>();
        }
    }

    /// <summary>
    /// Links properties to citizens (owners/tenants)
    /// </summary>
    public class PropertyOwnership : CreationAuditedEntity<Guid>
    {
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public PropertyOwnershipType OwnershipType { get; set; }
        public decimal? OwnershipPercentage { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        protected PropertyOwnership() { }

        public PropertyOwnership(Guid id, Guid propertyId, Guid citizenId, PropertyOwnershipType ownershipType)
        {
            Id = id;
            PropertyId = propertyId;
            CitizenId = citizenId;
            OwnershipType = ownershipType;
            StartDate = DateTime.UtcNow;
            IsActive = true;
        }
    }

    /// <summary>
    /// Property valuation history
    /// </summary>
    public class PropertyValuation : CreationAuditedEntity<Guid>
    {
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }

        public decimal ValuationAmount { get; set; }
        public DateTime ValuationDate { get; set; }
        public ValuationMethod Method { get; set; }
        public string ValuedBy { get; set; }
        public string Notes { get; set; }

        protected PropertyValuation() { }

        public PropertyValuation(Guid id, Guid propertyId, decimal amount, ValuationMethod method)
        {
            Id = id;
            PropertyId = propertyId;
            ValuationAmount = amount;
            ValuationDate = DateTime.UtcNow;
            Method = method;
        }
    }

    /// <summary>
    /// Property tax payment history
    /// </summary>
    public class PropertyTaxHistory : CreationAuditedEntity<Guid>
    {
        public Guid PropertyId { get; set; }
        public Property Property { get; set; }

        public string TaxYear { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }

        protected PropertyTaxHistory() { }

        public PropertyTaxHistory(Guid id, Guid propertyId, string taxYear, decimal taxAmount, DateTime dueDate)
        {
            Id = id;
            PropertyId = propertyId;
            TaxYear = taxYear;
            TaxAmount = taxAmount;
            Balance = taxAmount;
            AmountPaid = 0;
            DueDate = dueDate;
        }
    }
}
