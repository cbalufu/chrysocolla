using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Subdomain)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(t => t.Subdomain)
            .IsUnique();

        builder.Property(t => t.ConnectionString)
            .HasMaxLength(500);

        builder.Property(t => t.IsActive)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();
    }
}
