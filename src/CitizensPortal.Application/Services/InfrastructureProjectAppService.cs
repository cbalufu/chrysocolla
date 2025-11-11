using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using CitizensPortal.Application.Contracts.DTOs.Infrastructure;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Enums;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.InfrastructureProjects.Default)]
public class InfrastructureProjectAppService : CrudAppService<InfrastructureProject, InfrastructureProjectDto, Guid, InfrastructureProjectDto, CreateUpdateInfrastructureProjectDto>, IInfrastructureProjectAppService
{
    private readonly IRepository<InfrastructureProject, Guid> _projectRepository;

    public InfrastructureProjectAppService(
        IRepository<InfrastructureProject, Guid> repository) : base(repository)
    {
        _projectRepository = repository;

        GetPolicyName = CitizensPortalPermissions.InfrastructureProjects.Default;
        GetListPolicyName = CitizensPortalPermissions.InfrastructureProjects.Default;
        CreatePolicyName = CitizensPortalPermissions.InfrastructureProjects.Create;
        UpdatePolicyName = CitizensPortalPermissions.InfrastructureProjects.Edit;
        DeletePolicyName = CitizensPortalPermissions.InfrastructureProjects.Delete;
    }

    [AllowAnonymous]
    public async Task<List<InfrastructureProjectDto>> GetActiveProjectsAsync()
    {
        var projects = await _projectRepository.GetListAsync();
        var activeProjects = projects.Where(p => p.Status == ProjectStatus.InProgress ||
                                                p.Status == ProjectStatus.Planning)
                                     .OrderByDescending(p => p.StartDate)
                                     .ToList();

        return ObjectMapper.Map<List<InfrastructureProject>, List<InfrastructureProjectDto>>(activeProjects);
    }

    [AllowAnonymous]
    public async Task<List<InfrastructureProjectDto>> GetProjectsByCategoryAsync(ProjectCategory category)
    {
        var projects = await _projectRepository.GetListAsync();
        var categoryProjects = projects.Where(p => p.Category == category)
                                      .OrderByDescending(p => p.StartDate)
                                      .ToList();

        return ObjectMapper.Map<List<InfrastructureProject>, List<InfrastructureProjectDto>>(categoryProjects);
    }

    [AllowAnonymous]
    public async Task<List<InfrastructureProjectDto>> GetProjectsInAreaAsync(string ward)
    {
        if (string.IsNullOrWhiteSpace(ward))
        {
            throw new Volo.Abp.BusinessException("WARD_REQUIRED")
                .WithData("message", "Ward is required");
        }

        var projects = await _projectRepository.GetListAsync();
        var areaProjects = projects.Where(p => p.Ward?.Equals(ward, StringComparison.OrdinalIgnoreCase) == true)
                                  .OrderByDescending(p => p.StartDate)
                                  .ToList();

        return ObjectMapper.Map<List<InfrastructureProject>, List<InfrastructureProjectDto>>(areaProjects);
    }
}
