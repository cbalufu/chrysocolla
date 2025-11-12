using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.Priority)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(n => n.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(n => n.IsRead)
            .IsRequired();

        builder.Property(n => n.SentDate)
            .IsRequired();

        builder.Property(n => n.ReadDate);

        // Relationships
        builder.HasOne(n => n.Citizen)
            .WithMany()
            .HasForeignKey(n => n.CitizenId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(n => new { n.TenantId, n.CitizenId });
        builder.HasIndex(n => new { n.CitizenId, n.IsRead });
        builder.HasIndex(n => n.SentDate);
    }
}
