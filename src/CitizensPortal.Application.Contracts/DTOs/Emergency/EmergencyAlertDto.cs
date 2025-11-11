using System;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Emergency
{
    public class EmergencyAlertDto : FullAuditedEntityDto<Guid>
    {
        public string AlertNumber { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string DetailedInformation { get; set; }
        public AlertType Type { get; set; }
        public AlertSeverity Severity { get; set; }
        public AlertStatus Status { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string AffectedWards { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
        public string SafetyInstructions { get; set; }
        public string ActionRequired { get; set; }
        public int ViewCount { get; set; }
        public int AcknowledgementCount { get; set; }
    }

    public class CreateUpdateEmergencyAlertDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string DetailedInformation { get; set; }
        public AlertType Type { get; set; }
        public AlertType AlertType { get => Type; set => Type = value; } // Alias
        public AlertSeverity Severity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string AffectedArea { get; set; } // Text description of affected area
        public string AffectedWards { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
        public string SafetyInstructions { get; set; }
        public string ActionRequired { get; set; }
    }

    public class EvacuationRouteDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartLocation { get; set; }
        public double? StartLatitude { get; set; }
        public double? StartLongitude { get; set; }
        public string DestinationLocation { get; set; }
        public double? DestinationLatitude { get; set; }
        public double? DestinationLongitude { get; set; }
        public string RouteGeoJson { get; set; }
        public decimal? DistanceKm { get; set; }
        public int? EstimatedTimeMinutes { get; set; }
        public bool IsActive { get; set; }
        public string Instructions { get; set; }
    }
}
