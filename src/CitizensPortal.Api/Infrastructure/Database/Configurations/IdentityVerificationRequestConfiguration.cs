using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class IdentityVerificationRequestConfiguration
    : IEntityTypeConfiguration<IdentityVerificationRequest>
{
    public void Configure(EntityTypeBuilder<IdentityVerificationRequest> builder)
    {
        builder.ToTable("IdentityVerificationRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ReferenceNumber)
            .IsUnique();

        builder.Property(x => x.NationalIdValue)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.NationalIdType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.ReviewerName)
            .HasMaxLength(200);

        builder.Property(x => x.AssignedToName)
            .HasMaxLength(200);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(1000);

        // Indexes for queries
        builder.HasIndex(x => x.CitizenId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.SubmittedAt);
        builder.HasIndex(x => new { x.TenantId, x.Status });

        // Relationship with Citizen
        builder.HasOne(x => x.Citizen)
            .WithMany()
            .HasForeignKey(x => x.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Documents
        builder.HasMany(x => x.Documents)
            .WithOne(x => x.VerificationRequest)
            .HasForeignKey(x => x.VerificationRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
