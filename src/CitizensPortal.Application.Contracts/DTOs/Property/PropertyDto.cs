using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Property
{
    public class PropertyDto : FullAuditedEntityDto<Guid>
    {
        public string PropertyReference { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Suburb { get; set; }
        public string Ward { get; set; }
        public PropertyType Type { get; set; }
        public PropertyStatus Status { get; set; }
        public decimal? LandSize { get; set; }
        public decimal? BuildingSize { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? YearBuilt { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? CurrentValuation { get; set; }
        public DateTime? LastValuationDate { get; set; }
        public List<PropertyOwnershipDto> Ownerships { get; set; }
    }

    public class PropertyOwnershipDto : EntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public PropertyOwnershipType OwnershipType { get; set; }
        public decimal? OwnershipPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUpdatePropertyDto
    {
        public string PropertyReference { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Suburb { get; set; }
        public string Ward { get; set; }
        public PropertyType Type { get; set; }
        public decimal? LandSize { get; set; }
        public decimal? BuildingSize { get; set; }
        public int? NumberOfBedrooms { get; set; }
        public int? NumberOfBathrooms { get; set; }
        public int? YearBuilt { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
