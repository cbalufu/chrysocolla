using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Appointment;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IAppointmentAppService : ICrudAppService<AppointmentDto, Guid, AppointmentDto, CreateUpdateAppointmentDto>
    {
        Task<List<AppointmentDto>> GetMyAppointmentsAsync();
        Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync();
        Task ConfirmAppointmentAsync(Guid id);
        Task CancelAppointmentAsync(Guid id, string reason);
        Task<List<DepartmentDto>> GetDepartmentsAsync();
    }
}
