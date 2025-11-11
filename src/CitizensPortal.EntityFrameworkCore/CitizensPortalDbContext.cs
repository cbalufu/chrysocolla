using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using CitizensPortal.Domain.Entities;

namespace CitizensPortal.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class CitizensPortalDbContext : AbpDbContext<CitizensPortalDbContext>
{
        // Phase 1 - Core
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<IssueReport> IssueReports { get; set; }
        public DbSet<IssueAttachment> IssueAttachments { get; set; }
        public DbSet<IssueComment> IssueComments { get; set; }
        public DbSet<Domain.Entities.Application> Applications { get; set; }
        public DbSet<ApplicationDocument> ApplicationDocuments { get; set; }
        public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }
        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        // Phase 2 - Enhanced Services
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentAllocation> PaymentAllocations { get; set; }
        public DbSet<PaymentReceipt> PaymentReceipts { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentAccess> DocumentAccesses { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
        public DbSet<PropertyValuation> PropertyValuations { get; set; }
        public DbSet<PropertyTaxHistory> PropertyTaxHistories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentSlot> AppointmentSlots { get; set; }
        public DbSet<CertificateRequest> CertificateRequests { get; set; }
        public DbSet<CertificateRequestDocument> CertificateRequestDocuments { get; set; }
        public DbSet<Certificate> Certificates { get; set; }

        // Phase 3 - Advanced Features
        public DbSet<InfrastructureProject> InfrastructureProjects { get; set; }
        public DbSet<ProjectUpdate> ProjectUpdates { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<ServiceArea> ServiceAreas { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<ConsultationComment> ConsultationComments { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteOption> VoteOptions { get; set; }
        public DbSet<CitizenVote> CitizenVotes { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumReply> ForumReplies { get; set; }
        public DbSet<EmergencyAlert> EmergencyAlerts { get; set; }
        public DbSet<AlertAcknowledgement> AlertAcknowledgements { get; set; }
        public DbSet<EvacuationRoute> EvacuationRoutes { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceRequestAttachment> ServiceRequestAttachments { get; set; }
        public DbSet<ServiceRequestUpdate> ServiceRequestUpdates { get; set; }
        public DbSet<CitizenDashboardStats> CitizenDashboardStats { get; set; }
        public DbSet<ServiceUsageLog> ServiceUsageLogs { get; set; }
        public DbSet<MonthlySpendingAnalysis> MonthlySpendingAnalyses { get; set; }

        public CitizensPortalDbContext(DbContextOptions<CitizensPortalDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ABP modules
            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureTenantManagement();
            builder.ConfigureFeatureManagement();

            // Configure Citizens Portal entities
            builder.ConfigureCitizensPortal();
        }
    }
