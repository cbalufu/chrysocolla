using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class CitizenConfiguration : IEntityTypeConfiguration<Citizen>
{
    public void Configure(EntityTypeBuilder<Citizen> builder)
    {
        builder.ToTable("Citizens");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(c => new { c.TenantId, c.Email })
            .IsUnique();

        builder.Property(c => c.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.NationalId)
            .HasMaxLength(50);

        builder.HasIndex(c => new { c.TenantId, c.NationalId });

        builder.Property(c => c.NationalIdType)
            .HasConversion<int>(); // Store enum as int

        builder.Property(c => c.HasFederatedProfile)
            .IsRequired();

        builder.Property(c => c.FederatedProfileId);

        builder.HasIndex(c => c.FederatedProfileId);

        builder.Property(c => c.DateOfBirth)
            .IsRequired();

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.RefreshToken)
            .HasMaxLength(500);

        builder.Property(c => c.RefreshTokenExpiryTime);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.LastLoginAt);

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => new { c.TenantId, c.Role });
    }
}
