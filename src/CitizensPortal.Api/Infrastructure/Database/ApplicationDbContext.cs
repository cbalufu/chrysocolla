using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CitizensPortal.Api.Infrastructure.Database;

/// <summary>
/// Application database context with multi-tenancy support.
/// Automatically filters queries by tenant using global query filters.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    private readonly ITenantAccessor _tenantAccessor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ITenantAccessor tenantAccessor)
        : base(options)
    {
        _tenantAccessor = tenantAccessor;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Citizen> Citizens => Set<Citizen>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<IssueComment> IssueComments => Set<IssueComment>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<ApplicationDocument> ApplicationDocuments => Set<ApplicationDocument>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<EmergencyAlert> EmergencyAlerts => Set<EmergencyAlert>();
    public DbSet<AlertAcknowledgement> AlertAcknowledgements => Set<AlertAcknowledgement>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<TicketMessage> TicketMessages => Set<TicketMessage>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<CertificateRequest> CertificateRequests => Set<CertificateRequest>();
    public DbSet<CitizenFederatedProfile> CitizenFederatedProfiles => Set<CitizenFederatedProfile>();
    public DbSet<LinkedCitizenProfile> LinkedCitizenProfiles => Set<LinkedCitizenProfile>();
    public DbSet<IdentityVerificationRequest> IdentityVerificationRequests => Set<IdentityVerificationRequest>();
    public DbSet<VerificationDocument> VerificationDocuments => Set<VerificationDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply global query filter for multi-tenant entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IMultiTenant).IsAssignableFrom(entityType.ClrType))
            {
                // Create expression: entity => entity.TenantId == _tenantAccessor.TenantId
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "entity");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(IMultiTenant.TenantId));
                var tenantId = System.Linq.Expressions.Expression.Constant(_tenantAccessor.TenantId);
                var equals = System.Linq.Expressions.Expression.Equal(property, tenantId);
                var lambda = System.Linq.Expressions.Expression.Lambda(equals, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically set TenantId on new multi-tenant entities
        foreach (var entry in ChangeTracker.Entries<IMultiTenant>())
        {
            if (entry.State == EntityState.Added && entry.Entity.TenantId == null)
            {
                entry.Entity.TenantId = _tenantAccessor.TenantId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
