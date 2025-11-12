using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Properties");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.PropertyNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => new { p.TenantId, p.PropertyNumber })
            .IsUnique();

        builder.Property(p => p.PropertyType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.LandArea)
            .HasPrecision(18, 2);

        builder.Property(p => p.BuildingArea)
            .HasPrecision(18, 2);

        builder.Property(p => p.AssessedValue)
            .HasPrecision(18, 2);

        builder.Property(p => p.AnnualTaxAmount)
            .HasPrecision(18, 2);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.ZoningClassification)
            .HasMaxLength(100);

        builder.HasIndex(p => new { p.TenantId, p.OwnerId });
        builder.HasIndex(p => p.CreatedAt);

        builder.HasOne(p => p.Owner)
            .WithMany()
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
