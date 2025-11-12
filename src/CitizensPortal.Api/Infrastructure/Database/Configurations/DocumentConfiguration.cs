using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DocumentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.FileUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.VerificationCode)
            .HasMaxLength(50);

        builder.HasIndex(d => new { d.TenantId, d.CitizenId });
        builder.HasIndex(d => new { d.TenantId, d.VerificationCode });
        builder.HasIndex(d => d.CreatedAt);

        builder.HasOne(d => d.Citizen)
            .WithMany()
            .HasForeignKey(d => d.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
