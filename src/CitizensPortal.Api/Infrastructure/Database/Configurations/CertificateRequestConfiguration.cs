using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class CertificateRequestConfiguration : IEntityTypeConfiguration<CertificateRequest>
{
    public void Configure(EntityTypeBuilder<CertificateRequest> builder)
    {
        builder.ToTable("CertificateRequests");

        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cr => cr.CertificateType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(cr => cr.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(cr => cr.Purpose)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(cr => cr.DeliveryMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(cr => cr.ApplicationFee)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(cr => cr.IsPaid)
            .IsRequired();

        builder.Property(cr => cr.RejectionReason)
            .HasMaxLength(1000);

        builder.Property(cr => cr.CreatedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(cr => cr.Citizen)
            .WithMany()
            .HasForeignKey(cr => cr.CitizenId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(cr => new { cr.TenantId, cr.CitizenId });
        builder.HasIndex(cr => cr.ReferenceNumber).IsUnique();
        builder.HasIndex(cr => new { cr.Status, cr.IsPaid });
        builder.HasIndex(cr => cr.CreatedAt);
    }
}
