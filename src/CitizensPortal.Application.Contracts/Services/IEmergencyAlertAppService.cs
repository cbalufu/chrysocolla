using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Emergency;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IEmergencyAlertAppService : ICrudAppService<EmergencyAlertDto, Guid, EmergencyAlertDto, CreateUpdateEmergencyAlertDto>
    {
        Task<List<EmergencyAlertDto>> GetActiveAlertsAsync();
        Task<List<EmergencyAlertDto>> GetMyAreaAlertsAsync();
        Task AcknowledgeAlertAsync(Guid id, bool isSafe, string notes);
        Task<List<EvacuationRouteDto>> GetEvacuationRoutesAsync();
    }
}
