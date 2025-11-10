using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.Citizen;
using CitizensPortal.Application.Contracts.Services;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/citizens")]
    public class CitizenController : AbpControllerBase, ICitizenAppService
    {
        private readonly ICitizenAppService _citizenAppService;

        public CitizenController(ICitizenAppService citizenAppService)
        {
            _citizenAppService = citizenAppService;
        }

        [HttpGet("{id}")]
        public Task<CitizenDto> GetAsync(Guid id)
        {
            return _citizenAppService.GetAsync(id);
        }

        [HttpGet]
        public Task<Volo.Abp.Application.Dtos.PagedResultDto<CitizenDto>> GetListAsync(CitizenDto input)
        {
            return _citizenAppService.GetListAsync(input);
        }

        [HttpPost]
        public Task<CitizenDto> CreateAsync(CreateUpdateCitizenDto input)
        {
            return _citizenAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<CitizenDto> UpdateAsync(Guid id, CreateUpdateCitizenDto input)
        {
            return _citizenAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _citizenAppService.DeleteAsync(id);
        }

        [HttpGet("by-email/{email}")]
        public Task<CitizenDto> GetByEmailAsync(string email)
        {
            return _citizenAppService.GetByEmailAsync(email);
        }

        [HttpGet("me")]
        public Task<CitizenDto> GetCurrentCitizenAsync()
        {
            return _citizenAppService.GetCurrentCitizenAsync();
        }
    }
}
