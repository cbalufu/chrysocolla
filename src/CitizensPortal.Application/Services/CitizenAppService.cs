using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using CitizensPortal.Application.Contracts.DTOs.Citizen;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;

namespace CitizensPortal.Application.Services
{
    public class CitizenAppService : CrudAppService<Citizen, CitizenDto, Guid, CitizenDto, CreateUpdateCitizenDto>, ICitizenAppService
    {
        private readonly ICitizenRepository _citizenRepository;

        public CitizenAppService(ICitizenRepository repository) : base(repository)
        {
            _citizenRepository = repository;
        }

        public async Task<CitizenDto> GetByEmailAsync(string email)
        {
            var citizen = await _citizenRepository.FindByEmailAsync(email);
            return ObjectMapper.Map<Citizen, CitizenDto>(citizen);
        }

        public async Task<CitizenDto> GetCurrentCitizenAsync()
        {
            // In a real application, get the current user's email from the authentication context
            // For now, this is a placeholder
            throw new NotImplementedException("Implement based on your authentication setup");
        }
    }
}
