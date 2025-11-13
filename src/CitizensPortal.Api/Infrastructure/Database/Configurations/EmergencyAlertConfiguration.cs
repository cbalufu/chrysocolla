using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class EmergencyAlertConfiguration : IEntityTypeConfiguration<EmergencyAlert>
{
    public void Configure(EntityTypeBuilder<EmergencyAlert> builder)
    {
        builder.ToTable("EmergencyAlerts");
        builder.HasKey(ea => ea.Id);

        builder.Property(ea => ea.AlertType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ea => ea.Severity)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ea => ea.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ea => ea.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(ea => ea.AffectedAreas)
            .HasMaxLength(4000);

        builder.Property(ea => ea.InstructionsJson)
            .HasMaxLength(4000);

        builder.Property(ea => ea.IssuedByUserName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(ea => new { ea.TenantId, ea.IsActive });
        builder.HasIndex(ea => ea.IssuedAt);
        builder.HasIndex(ea => ea.Severity);
    }
}

public sealed class AlertAcknowledgementConfiguration : IEntityTypeConfiguration<AlertAcknowledgement>
{
    public void Configure(EntityTypeBuilder<AlertAcknowledgement> builder)
    {
        builder.ToTable("AlertAcknowledgements");
        builder.HasKey(aa => aa.Id);

        builder.Property(aa => aa.Location)
            .HasMaxLength(500);

        builder.HasIndex(aa => new { aa.TenantId, aa.AlertId });
        builder.HasIndex(aa => new { aa.TenantId, aa.CitizenId, aa.AlertId })
            .IsUnique();

        builder.HasOne(aa => aa.Alert)
            .WithMany()
            .HasForeignKey(aa => aa.AlertId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(aa => aa.Citizen)
            .WithMany()
            .HasForeignKey(aa => aa.CitizenId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
