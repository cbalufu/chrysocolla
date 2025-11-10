using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using CitizensPortal.Domain.Entities;

namespace CitizensPortal.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class CitizensPortalDbContext : AbpDbContext<CitizensPortalDbContext>
    {
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

        public CitizensPortalDbContext(DbContextOptions<CitizensPortalDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCitizensPortal();
        }
    }
}
