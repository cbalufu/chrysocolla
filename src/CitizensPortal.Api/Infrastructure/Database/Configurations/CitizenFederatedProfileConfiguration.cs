using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class CitizenFederatedProfileConfiguration : IEntityTypeConfiguration<CitizenFederatedProfile>
{
    public void Configure(EntityTypeBuilder<CitizenFederatedProfile> builder)
    {
        builder.ToTable("CitizenFederatedProfiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.NationalIdType)
            .IsRequired()
            .HasConversion<int>(); // Store enum as int

        builder.Property(p => p.NationalIdEncrypted)
            .IsRequired()
            .HasMaxLength(500); // Base64 encrypted string

        builder.Property(p => p.NationalIdHash)
            .IsRequired()
            .HasMaxLength(64); // SHA-256 hash in hex = 64 characters

        // Create unique index on hash for fast lookups
        builder.HasIndex(p => p.NationalIdHash)
            .IsUnique();

        builder.Property(p => p.VerificationStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.VerificationMethod)
            .HasConversion<int>();

        builder.Property(p => p.ConsentStatus)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        // Configure one-to-many relationship with LinkedCitizenProfiles
        builder.HasMany(p => p.LinkedProfiles)
            .WithOne(l => l.FederatedProfile)
            .HasForeignKey(l => l.FederatedProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
