using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class CouncilRegistrationRequestConfiguration
    : IEntityTypeConfiguration<CouncilRegistrationRequest>
{
    public void Configure(EntityTypeBuilder<CouncilRegistrationRequest> builder)
    {
        builder.ToTable("CouncilRegistrationRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ReferenceNumber)
            .IsUnique();

        builder.Property(x => x.CouncilName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.RegistrationNumber);

        builder.Property(x => x.Region)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ContactName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContactEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ContactPhone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.AdminName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.AdminEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.PhysicalAddress)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.ReviewerName)
            .HasMaxLength(200);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(1000);

        // Indexes for queries
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.SubmittedAt);
        builder.HasIndex(x => x.ContactEmail);
        builder.HasIndex(x => x.AdminEmail);
    }
}
