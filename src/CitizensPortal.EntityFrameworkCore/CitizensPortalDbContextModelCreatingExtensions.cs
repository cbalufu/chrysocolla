using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared;

namespace CitizensPortal.EntityFrameworkCore
{
    public static class CitizensPortalDbContextModelCreatingExtensions
    {
        public static void ConfigureCitizensPortal(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Citizen
            builder.Entity<Citizen>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "Citizens", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.FirstName).IsRequired().HasMaxLength(128);
                b.Property(x => x.LastName).IsRequired().HasMaxLength(128);
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.PhoneNumber).HasMaxLength(32);
                b.Property(x => x.NationalId).HasMaxLength(50);
                b.Property(x => x.Address).HasMaxLength(512);
                b.Property(x => x.PropertyReference).HasMaxLength(50);

                b.HasIndex(x => x.Email);
                b.HasIndex(x => x.NationalId);
            });

            // IssueReport
            builder.Entity<IssueReport>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "IssueReports", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Title).IsRequired().HasMaxLength(256);
                b.Property(x => x.Description).IsRequired().HasMaxLength(2048);
                b.Property(x => x.ReferenceNumber).IsRequired().HasMaxLength(50);
                b.Property(x => x.LocationAddress).HasMaxLength(512);

                b.HasOne(x => x.Citizen).WithMany().HasForeignKey(x => x.CitizenId).IsRequired();
                b.HasMany(x => x.Attachments).WithOne(x => x.IssueReport).HasForeignKey(x => x.IssueReportId);
                b.HasMany(x => x.Comments).WithOne(x => x.IssueReport).HasForeignKey(x => x.IssueReportId);

                b.HasIndex(x => x.ReferenceNumber);
                b.HasIndex(x => x.Status);
            });

            // IssueAttachment
            builder.Entity<IssueAttachment>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "IssueAttachments", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.FileName).IsRequired().HasMaxLength(256);
                b.Property(x => x.FileUrl).IsRequired().HasMaxLength(1024);
            });

            // IssueComment
            builder.Entity<IssueComment>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "IssueComments", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Comment).IsRequired().HasMaxLength(2048);
            });

            // Application
            builder.Entity<Domain.Entities.Application>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "Applications", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.ApplicationNumber).IsRequired().HasMaxLength(50);
                b.Property(x => x.Title).IsRequired().HasMaxLength(256);
                b.Property(x => x.Description).IsRequired().HasMaxLength(2048);

                b.HasOne(x => x.Citizen).WithMany().HasForeignKey(x => x.CitizenId).IsRequired();
                b.HasMany(x => x.Documents).WithOne(x => x.Application).HasForeignKey(x => x.ApplicationId);
                b.HasMany(x => x.StatusHistory).WithOne(x => x.Application).HasForeignKey(x => x.ApplicationId);

                b.HasIndex(x => x.ApplicationNumber);
                b.HasIndex(x => x.Status);
            });

            // ApplicationDocument
            builder.Entity<ApplicationDocument>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "ApplicationDocuments", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.DocumentName).IsRequired().HasMaxLength(256);
                b.Property(x => x.FileUrl).IsRequired().HasMaxLength(1024);
            });

            // ApplicationStatusHistory
            builder.Entity<ApplicationStatusHistory>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "ApplicationStatusHistories", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Notes).HasMaxLength(1024);
            });

            // Bill
            builder.Entity<Bill>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "Bills", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.BillNumber).IsRequired().HasMaxLength(50);
                b.Property(x => x.Description).HasMaxLength(512);
                b.Property(x => x.Period).HasMaxLength(50);
                b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
                b.Property(x => x.AmountPaid).HasColumnType("decimal(18,2)");
                b.Property(x => x.Balance).HasColumnType("decimal(18,2)");

                b.HasOne(x => x.Citizen).WithMany().HasForeignKey(x => x.CitizenId).IsRequired();

                b.HasIndex(x => x.BillNumber);
                b.HasIndex(x => x.Status);
            });

            // Notification
            builder.Entity<Notification>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "Notifications", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Title).IsRequired().HasMaxLength(256);
                b.Property(x => x.Message).IsRequired().HasMaxLength(2048);

                b.HasOne(x => x.Citizen).WithMany().HasForeignKey(x => x.CitizenId).IsRequired();

                b.HasIndex(x => x.IsRead);
                b.HasIndex(x => x.CreationTime);
            });

            // Ticket
            builder.Entity<Ticket>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "Tickets", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.TicketNumber).IsRequired().HasMaxLength(50);
                b.Property(x => x.Subject).IsRequired().HasMaxLength(256);
                b.Property(x => x.Description).IsRequired().HasMaxLength(2048);

                b.HasOne(x => x.Citizen).WithMany().HasForeignKey(x => x.CitizenId).IsRequired();
                b.HasMany(x => x.Messages).WithOne(x => x.Ticket).HasForeignKey(x => x.TicketId);
                b.HasMany(x => x.Attachments).WithOne(x => x.Ticket).HasForeignKey(x => x.TicketId);

                b.HasIndex(x => x.TicketNumber);
                b.HasIndex(x => x.Status);
            });

            // TicketMessage
            builder.Entity<TicketMessage>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "TicketMessages", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.Message).IsRequired().HasMaxLength(2048);
            });

            // TicketAttachment
            builder.Entity<TicketAttachment>(b =>
            {
                b.ToTable(CitizensPortalConsts.DbTablePrefix + "TicketAttachments", CitizensPortalConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(x => x.FileName).IsRequired().HasMaxLength(256);
                b.Property(x => x.FileUrl).IsRequired().HasMaxLength(1024);
            });
        }
    }
}
