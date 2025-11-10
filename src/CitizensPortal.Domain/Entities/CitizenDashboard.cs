using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Aggregated dashboard statistics for a citizen
    /// </summary>
    public class CitizenDashboardStats : Entity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        // Bills & Payments
        public decimal TotalOutstandingBalance { get; set; }
        public int UnpaidBillsCount { get; set; }
        public decimal TotalPaidThisYear { get; set; }

        // Applications
        public int ActiveApplicationsCount { get; set; }
        public int PendingApplicationsCount { get; set; }
        public int ApprovedApplicationsCount { get; set; }

        // Issues & Service Requests
        public int OpenIssuesCount { get; set; }
        public int OpenServiceRequestsCount { get; set; }

        // Notifications & Tickets
        public int UnreadNotificationsCount { get; set; }
        public int ActiveTicketsCount { get; set; }

        // Appointments
        public int UpcomingAppointmentsCount { get; set; }

        // Last updated
        public DateTime LastUpdated { get; set; }

        protected CitizenDashboardStats() { }

        public CitizenDashboardStats(Guid id, Guid citizenId, Guid? tenantId = null)
        {
            Id = id;
            CitizenId = citizenId;
            TenantId = tenantId;
            LastUpdated = DateTime.UtcNow;
        }

        public void RefreshStats()
        {
            LastUpdated = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Tracks service usage by category for analytics
    /// </summary>
    public class ServiceUsageLog : CreationAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string ServiceType { get; set; } // "Bill", "Application", "Issue", etc.
        public string ServiceAction { get; set; } // "View", "Create", "Update", "Payment"
        public Guid? RelatedEntityId { get; set; }

        public DateTime AccessTime { get; set; }

        protected ServiceUsageLog() { }

        public ServiceUsageLog(Guid id, Guid citizenId, string serviceType, string serviceAction, Guid? tenantId = null)
        {
            Id = id;
            CitizenId = citizenId;
            ServiceType = serviceType;
            ServiceAction = serviceAction;
            AccessTime = DateTime.UtcNow;
            TenantId = tenantId;
        }
    }

    /// <summary>
    /// Monthly spending analysis for citizens
    /// </summary>
    public class MonthlySpendingAnalysis : Entity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public decimal PropertyRatesSpent { get; set; }
        public decimal WaterSpent { get; set; }
        public decimal ElectricitySpent { get; set; }
        public decimal WasteSpent { get; set; }
        public decimal OtherSpent { get; set; }
        public decimal TotalSpent { get; set; }

        protected MonthlySpendingAnalysis() { }

        public MonthlySpendingAnalysis(Guid id, Guid citizenId, int year, int month, Guid? tenantId = null)
        {
            Id = id;
            CitizenId = citizenId;
            Year = year;
            Month = month;
            TenantId = tenantId;
        }
    }
}
