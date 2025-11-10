using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.ServiceRequest;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IServiceRequestAppService : ICrudAppService<ServiceRequestDto, Guid, ServiceRequestDto, CreateUpdateServiceRequestDto>
    {
        Task<List<ServiceRequestDto>> GetMyServiceRequestsAsync();
        Task<ServiceRequestDto> GetByRequestNumberAsync(string requestNumber);
        Task RateServiceAsync(Guid id, int rating, string feedback);
    }
}
