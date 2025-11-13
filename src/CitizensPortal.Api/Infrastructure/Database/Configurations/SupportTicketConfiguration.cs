using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.ToTable("SupportTickets");
        builder.HasKey(st => st.Id);

        builder.Property(st => st.TicketNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(st => new { st.TenantId, st.TicketNumber })
            .IsUnique();

        builder.Property(st => st.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(st => st.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(st => st.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(st => st.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(st => st.Priority)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(st => new { st.TenantId, st.CitizenId });
        builder.HasIndex(st => new { st.TenantId, st.Status });
        builder.HasIndex(st => st.CreatedAt);

        builder.HasOne(st => st.Citizen)
            .WithMany()
            .HasForeignKey(st => st.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(st => st.Messages)
            .WithOne(tm => tm.Ticket)
            .HasForeignKey(tm => tm.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class TicketMessageConfiguration : IEntityTypeConfiguration<TicketMessage>
{
    public void Configure(EntityTypeBuilder<TicketMessage> builder)
    {
        builder.ToTable("TicketMessages");
        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.SenderName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(tm => tm.Message)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(tm => tm.AttachmentUrls)
            .HasMaxLength(4000);

        builder.HasIndex(tm => new { tm.TenantId, tm.TicketId });
        builder.HasIndex(tm => tm.CreatedAt);
    }
}
