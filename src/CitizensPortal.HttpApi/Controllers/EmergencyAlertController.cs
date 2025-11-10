using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.Emergency;
using CitizensPortal.Application.Contracts.Services;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/emergency-alerts")]
    public class EmergencyAlertController : AbpControllerBase
    {
        private readonly IEmergencyAlertAppService _emergencyAlertAppService;

        public EmergencyAlertController(IEmergencyAlertAppService emergencyAlertAppService)
        {
            _emergencyAlertAppService = emergencyAlertAppService;
        }

        [HttpGet]
        public Task<Volo.Abp.Application.Dtos.PagedResultDto<EmergencyAlertDto>> GetListAsync(EmergencyAlertDto input)
        {
            return _emergencyAlertAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public Task<EmergencyAlertDto> GetAsync(Guid id)
        {
            return _emergencyAlertAppService.GetAsync(id);
        }

        [HttpGet("active")]
        public Task<List<EmergencyAlertDto>> GetActiveAlertsAsync()
        {
            return _emergencyAlertAppService.GetActiveAlertsAsync();
        }

        [HttpGet("my-area")]
        public Task<List<EmergencyAlertDto>> GetMyAreaAlertsAsync()
        {
            return _emergencyAlertAppService.GetMyAreaAlertsAsync();
        }

        [HttpPost]
        public Task<EmergencyAlertDto> CreateAsync(CreateUpdateEmergencyAlertDto input)
        {
            return _emergencyAlertAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<EmergencyAlertDto> UpdateAsync(Guid id, CreateUpdateEmergencyAlertDto input)
        {
            return _emergencyAlertAppService.UpdateAsync(id, input);
        }

        [HttpPost("{id}/acknowledge")]
        public Task AcknowledgeAlertAsync(Guid id, [FromBody] AcknowledgeAlertInput input)
        {
            return _emergencyAlertAppService.AcknowledgeAlertAsync(id, input.IsSafe, input.Notes);
        }

        [HttpGet("evacuation-routes")]
        public Task<List<EvacuationRouteDto>> GetEvacuationRoutesAsync()
        {
            return _emergencyAlertAppService.GetEvacuationRoutesAsync();
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _emergencyAlertAppService.DeleteAsync(id);
        }
    }

    public class AcknowledgeAlertInput
    {
        public bool IsSafe { get; set; }
        public string Notes { get; set; }
    }
}
