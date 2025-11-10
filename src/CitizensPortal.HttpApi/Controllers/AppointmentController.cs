using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.Appointment;
using CitizensPortal.Application.Contracts.Services;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/appointments")]
    public class AppointmentController : AbpControllerBase
    {
        private readonly IAppointmentAppService _appointmentAppService;

        public AppointmentController(IAppointmentAppService appointmentAppService)
        {
            _appointmentAppService = appointmentAppService;
        }

        [HttpGet]
        public Task<Volo.Abp.Application.Dtos.PagedResultDto<AppointmentDto>> GetListAsync(AppointmentDto input)
        {
            return _appointmentAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public Task<AppointmentDto> GetAsync(Guid id)
        {
            return _appointmentAppService.GetAsync(id);
        }

        [HttpGet("my-appointments")]
        public Task<List<AppointmentDto>> GetMyAppointmentsAsync()
        {
            return _appointmentAppService.GetMyAppointmentsAsync();
        }

        [HttpGet("upcoming")]
        public Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync()
        {
            return _appointmentAppService.GetUpcomingAppointmentsAsync();
        }

        [HttpPost]
        public Task<AppointmentDto> CreateAsync(CreateUpdateAppointmentDto input)
        {
            return _appointmentAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<AppointmentDto> UpdateAsync(Guid id, CreateUpdateAppointmentDto input)
        {
            return _appointmentAppService.UpdateAsync(id, input);
        }

        [HttpPost("{id}/confirm")]
        public Task ConfirmAppointmentAsync(Guid id)
        {
            return _appointmentAppService.ConfirmAppointmentAsync(id);
        }

        [HttpPost("{id}/cancel")]
        public Task CancelAppointmentAsync(Guid id, [FromBody] string reason)
        {
            return _appointmentAppService.CancelAppointmentAsync(id, reason);
        }

        [HttpGet("departments")]
        public Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            return _appointmentAppService.GetDepartmentsAsync();
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _appointmentAppService.DeleteAsync(id);
        }
    }
}
