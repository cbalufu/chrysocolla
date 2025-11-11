using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents an appointment booked by a citizen
    /// </summary>
    public class Appointment : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public Guid? AppointmentSlotId { get; set; }
        public AppointmentSlot AppointmentSlot { get; set; }

        public string AppointmentNumber { get; set; }
        public string Purpose { get; set; }
        public string Notes { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }

        // For virtual appointments
        public string MeetingLink { get; set; }
        public string MeetingPassword { get; set; }

        // Reminder settings
        public bool SendReminder { get; set; }
        public DateTime? ReminderSentDate { get; set; }

        // Staff assignment
        public Guid? AssignedToUserId { get; set; }
        public string AssignedToName { get; set; }

        // Cancellation/Rescheduling
        public DateTime? CancelledDate { get; set; }
        public string CancellationReason { get; set; }
        public Guid? RescheduledFromId { get; set; }

        protected Appointment() { }

        public Appointment(
            Guid id,
            Guid citizenId,
            Guid departmentId,
            DateTime appointmentDate,
            TimeSpan startTime,
            TimeSpan endTime,
            AppointmentType type,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            DepartmentId = departmentId;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
            EndTime = endTime;
            Type = type;
            Status = AppointmentStatus.Scheduled;
            TenantId = tenantId;
            AppointmentNumber = GenerateAppointmentNumber();
            SendReminder = true;
        }

        private string GenerateAppointmentNumber()
        {
            return $"APT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public void Confirm()
        {
            Status = AppointmentStatus.Confirmed;
        }

        public void Cancel(string reason)
        {
            Status = AppointmentStatus.Cancelled;
            CancellationReason = reason;
            CancelledDate = DateTime.UtcNow;
        }

        public void MarkAsCompleted()
        {
            Status = AppointmentStatus.Completed;
        }
    }

    /// <summary>
    /// Defines available appointment time slots for departments
    /// </summary>
    public class AppointmentSlot : CreationAuditedEntity<Guid>
    {
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public int MaxBookings { get; set; }

        public bool IsActive { get; set; }

        protected AppointmentSlot() { }

        public AppointmentSlot(
            Guid id,
            Guid departmentId,
            DayOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            int durationMinutes)
        {
            Id = id;
            DepartmentId = departmentId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            DurationMinutes = durationMinutes;
            MaxBookings = 1;
            IsActive = true;
        }
    }
}
