using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.ToTable("Certificates");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CertificateNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => new { c.TenantId, c.CertificateNumber })
            .IsUnique();

        builder.Property(c => c.CertificateType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.FileUrl)
            .HasMaxLength(1000);

        builder.Property(c => c.VerificationCode)
            .HasMaxLength(50);

        builder.HasIndex(c => new { c.TenantId, c.VerificationCode });

        builder.Property(c => c.IssuedByUserName)
            .HasMaxLength(200);

        builder.Property(c => c.RejectionReason)
            .HasMaxLength(1000);

        builder.HasIndex(c => new { c.TenantId, c.CitizenId });
        builder.HasIndex(c => c.CreatedAt);

        builder.HasOne(c => c.Citizen)
            .WithMany()
            .HasForeignKey(c => c.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
