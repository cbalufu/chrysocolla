using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents an emergency alert sent to citizens
    /// </summary>
    public class EmergencyAlert : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

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

        // Affected area (GeoJSON polygon)
        public string AffectedAreaGeoJson { get; set; }
        public string AffectedWards { get; set; } // Comma-separated ward names

        // Contact information
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }

        // Instructions
        public string SafetyInstructions { get; set; }
        public string ActionRequired { get; set; }

        // Tracking
        public int ViewCount { get; set; }
        public int AcknowledgementCount { get; set; }

        public ICollection<AlertAcknowledgement> Acknowledgements { get; set; }

        protected EmergencyAlert()
        {
            Acknowledgements = new List<AlertAcknowledgement>();
        }

        public EmergencyAlert(
            Guid id,
            string title,
            string message,
            AlertType type,
            AlertSeverity severity,
            Guid? tenantId = null) : base(id)
        {
            Title = title;
            Message = message;
            Type = type;
            Severity = severity;
            Status = AlertStatus.Active;
            IssuedDate = DateTime.UtcNow;
            TenantId = tenantId;
            AlertNumber = GenerateAlertNumber();
            Acknowledgements = new List<AlertAcknowledgement>();
        }

        private string GenerateAlertNumber()
        {
            return $"ALERT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }

        public void Resolve()
        {
            Status = AlertStatus.Resolved;
            ResolvedDate = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Tracks which citizens have acknowledged an alert
    /// </summary>
    public class AlertAcknowledgement : CreationAuditedEntity<Guid>
    {
        public Guid EmergencyAlertId { get; set; }
        public EmergencyAlert EmergencyAlert { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public DateTime AcknowledgedDate { get; set; }
        public bool IsSafe { get; set; }
        public string Notes { get; set; }

        protected AlertAcknowledgement() { }

        public AlertAcknowledgement(Guid id, Guid alertId, Guid citizenId, bool isSafe)
        {
            Id = id;
            EmergencyAlertId = alertId;
            CitizenId = citizenId;
            IsSafe = isSafe;
            AcknowledgedDate = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Represents an evacuation route
    /// </summary>
    public class EvacuationRoute : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        // Starting point
        public string StartLocation { get; set; }
        public double? StartLatitude { get; set; }
        public double? StartLongitude { get; set; }

        // Destination (safe zone)
        public string DestinationLocation { get; set; }
        public double? DestinationLatitude { get; set; }
        public double? DestinationLongitude { get; set; }

        // Route path (GeoJSON LineString)
        public string RouteGeoJson { get; set; }

        // Route information
        public decimal? DistanceKm { get; set; }
        public int? EstimatedTimeMinutes { get; set; }

        public bool IsActive { get; set; }
        public string Instructions { get; set; }

        protected EvacuationRoute() { }

        public EvacuationRoute(Guid id, string name, string startLocation, string destinationLocation, Guid? tenantId = null) : base(id)
        {
            Name = name;
            StartLocation = startLocation;
            DestinationLocation = destinationLocation;
            IsActive = true;
            TenantId = tenantId;
        }
    }

    /// <summary>
    /// Emergency contact directory
    /// </summary>
    public class EmergencyContact : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternatePhone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public bool IsAvailable24_7 { get; set; }
        public string OperatingHours { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        protected EmergencyContact() { }

        public EmergencyContact(Guid id, string serviceName, string phoneNumber, Guid? tenantId = null) : base(id)
        {
            ServiceName = serviceName;
            PhoneNumber = phoneNumber;
            IsActive = true;
            TenantId = tenantId;
        }
    }
}
