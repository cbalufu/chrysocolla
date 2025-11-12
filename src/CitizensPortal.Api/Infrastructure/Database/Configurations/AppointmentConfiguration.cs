using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CitizensPortal.Api.Infrastructure.Database.Configurations;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.AppointmentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Department)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Purpose)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Location)
            .HasMaxLength(200);

        builder.Property(a => a.RoomNumber)
            .HasMaxLength(50);

        builder.Property(a => a.AssignedToUserName)
            .HasMaxLength(200);

        builder.Property(a => a.Notes)
            .HasMaxLength(2000);

        builder.Property(a => a.CancellationReason)
            .HasMaxLength(500);

        builder.HasIndex(a => new { a.TenantId, a.CitizenId });
        builder.HasIndex(a => new { a.TenantId, a.ScheduledDate });
        builder.HasIndex(a => a.Status);

        builder.HasOne(a => a.Citizen)
            .WithMany()
            .HasForeignKey(a => a.CitizenId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
