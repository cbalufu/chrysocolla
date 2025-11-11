using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("Issues");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(i => i.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.Priority)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.Location)
            .HasMaxLength(500);

        builder.Property(i => i.ImageUrls)
            .HasMaxLength(4000);

        builder.HasIndex(i => new { i.TenantId, i.Status });
        builder.HasIndex(i => new { i.TenantId, i.CitizenId });
        builder.HasIndex(i => i.CreatedAt);

        builder.HasOne(i => i.Citizen)
            .WithMany()
            .HasForeignKey(i => i.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.Comments)
            .WithOne(c => c.Issue)
            .HasForeignKey(c => c.IssueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class IssueCommentConfiguration : IEntityTypeConfiguration<IssueComment>
{
    public void Configure(EntityTypeBuilder<IssueComment> builder)
    {
        builder.ToTable("IssueComments");
        builder.HasKey(ic => ic.Id);

        builder.Property(ic => ic.UserName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ic => ic.Comment)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasIndex(ic => new { ic.TenantId, ic.IssueId });
        builder.HasIndex(ic => ic.CreatedAt);
    }
}
