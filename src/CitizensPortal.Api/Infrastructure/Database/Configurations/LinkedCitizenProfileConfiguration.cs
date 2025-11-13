using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class LinkedCitizenProfileConfiguration : IEntityTypeConfiguration<LinkedCitizenProfile>
{
    public void Configure(EntityTypeBuilder<LinkedCitizenProfile> builder)
    {
        builder.ToTable("LinkedCitizenProfiles");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.FederatedProfileId)
            .IsRequired();

        builder.Property(l => l.TenantId)
            .IsRequired();

        builder.Property(l => l.CitizenId)
            .IsRequired();

        builder.Property(l => l.LinkedAt)
            .IsRequired();

        builder.Property(l => l.IsActive)
            .IsRequired();

        // Create composite index for efficient lookups
        builder.HasIndex(l => new { l.TenantId, l.CitizenId });

        // Create index on FederatedProfileId for relationship queries
        builder.HasIndex(l => l.FederatedProfileId);

        // Ensure one citizen can only be linked once per tenant (if active)
        builder.HasIndex(l => new { l.TenantId, l.CitizenId, l.IsActive })
            .IsUnique()
            .HasFilter("[IsActive] = 1"); // SQL Server syntax for filtered unique index
    }
}
