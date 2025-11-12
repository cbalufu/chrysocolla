using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.PaymentNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => new { p.TenantId, p.PaymentNumber })
            .IsUnique();

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.TransactionId)
            .HasMaxLength(200);

        builder.Property(p => p.ReceiptNumber)
            .HasMaxLength(50);

        builder.Property(p => p.PaymentGateway)
            .HasMaxLength(100);

        builder.Property(p => p.PaymentGatewayResponse)
            .HasMaxLength(4000);

        builder.HasIndex(p => new { p.TenantId, p.CitizenId });
        builder.HasIndex(p => new { p.TenantId, p.BillId });
        builder.HasIndex(p => p.CreatedAt);

        builder.HasOne(p => p.Citizen)
            .WithMany()
            .HasForeignKey(p => p.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
