using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Application;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IApplicationAppService : ICrudAppService<ApplicationDto, Guid, ApplicationDto, CreateUpdateApplicationDto>
    {
        Task<List<ApplicationDto>> GetMyApplicationsAsync();
        Task<ApplicationDto> GetByApplicationNumberAsync(string applicationNumber);
        Task SubmitApplicationAsync(Guid id);
        Task WithdrawApplicationAsync(Guid id);
    }
}
