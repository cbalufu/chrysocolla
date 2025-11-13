using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("Applications");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ApplicationType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ApplicationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(a => new { a.TenantId, a.ApplicationNumber })
            .IsUnique();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(a => a.FormDataJson)
            .IsRequired();

        builder.Property(a => a.ReviewNotes)
            .HasMaxLength(2000);

        builder.HasIndex(a => new { a.TenantId, a.Status });
        builder.HasIndex(a => new { a.TenantId, a.CitizenId });
        builder.HasIndex(a => a.CreatedAt);

        builder.HasOne(a => a.Citizen)
            .WithMany()
            .HasForeignKey(a => a.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Documents)
            .WithOne(d => d.Application)
            .HasForeignKey(d => d.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class ApplicationDocumentConfiguration : IEntityTypeConfiguration<ApplicationDocument>
{
    public void Configure(EntityTypeBuilder<ApplicationDocument> builder)
    {
        builder.ToTable("ApplicationDocuments");
        builder.HasKey(ad => ad.Id);

        builder.Property(ad => ad.DocumentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ad => ad.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ad => ad.FileUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(ad => ad.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(ad => new { ad.TenantId, ad.ApplicationId });
    }
}
