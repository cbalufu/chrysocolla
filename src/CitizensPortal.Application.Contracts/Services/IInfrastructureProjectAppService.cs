using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Infrastructure;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IInfrastructureProjectAppService : ICrudAppService<InfrastructureProjectDto, Guid, InfrastructureProjectDto, CreateUpdateInfrastructureProjectDto>
    {
        Task<List<InfrastructureProjectDto>> GetActiveProjectsAsync();
        Task<List<InfrastructureProjectDto>> GetProjectsByCategoryAsync(ProjectCategory category);
        Task<List<InfrastructureProjectDto>> GetProjectsInAreaAsync(string ward);
    }
}
