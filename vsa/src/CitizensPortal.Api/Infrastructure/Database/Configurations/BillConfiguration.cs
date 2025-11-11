using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BillNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(b => new { b.TenantId, b.BillNumber })
            .IsUnique();

        builder.Property(b => b.BillType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.Amount)
            .HasPrecision(18, 2);

        builder.Property(b => b.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(b => b.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(b => b.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.PropertyReference)
            .HasMaxLength(100);

        builder.Property(b => b.AccountNumber)
            .HasMaxLength(50);

        builder.HasIndex(b => new { b.TenantId, b.CitizenId });
        builder.HasIndex(b => new { b.TenantId, b.Status });
        builder.HasIndex(b => b.DueDate);

        builder.HasOne(b => b.Citizen)
            .WithMany()
            .HasForeignKey(b => b.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Payments)
            .WithOne(p => p.Bill)
            .HasForeignKey(p => p.BillId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
