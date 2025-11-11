using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("ServiceRequests");
        builder.HasKey(sr => sr.Id);

        builder.Property(sr => sr.RequestNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(sr => new { sr.TenantId, sr.RequestNumber })
            .IsUnique();

        builder.Property(sr => sr.ServiceType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sr => sr.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sr => sr.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(sr => sr.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(sr => sr.Priority)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(sr => sr.Location)
            .HasMaxLength(500);

        builder.HasIndex(sr => new { sr.TenantId, sr.CitizenId });
        builder.HasIndex(sr => new { sr.TenantId, sr.Status });
        builder.HasIndex(sr => sr.CreatedAt);

        builder.HasOne(sr => sr.Citizen)
            .WithMany()
            .HasForeignKey(sr => sr.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
