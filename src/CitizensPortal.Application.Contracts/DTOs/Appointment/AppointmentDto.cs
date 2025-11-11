using System;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Appointment
{
    public class AppointmentDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string AppointmentNumber { get; set; }
        public string Purpose { get; set; }
        public string Notes { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }
        public string MeetingLink { get; set; }
        public bool SendReminder { get; set; }
        public string AssignedToName { get; set; }
    }

    public class CreateUpdateAppointmentDto
    {
        public Guid DepartmentId { get; set; }
        public string Purpose { get; set; }
        public string Notes { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public AppointmentType Type { get; set; }
        public bool SendReminder { get; set; }
    }

    public class DepartmentDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public bool AllowsAppointments { get; set; }
    }
}
