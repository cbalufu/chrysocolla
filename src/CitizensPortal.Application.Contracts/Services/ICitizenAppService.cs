using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Citizen;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface ICitizenAppService : ICrudAppService<CitizenDto, Guid, CitizenDto, CreateUpdateCitizenDto>
    {
        Task<CitizenDto> GetByEmailAsync(string email);
        Task<CitizenDto> GetCurrentCitizenAsync();
    }
}
