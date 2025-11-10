using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Citizen;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Citizens.Default)]
public class CitizenAppService : CrudAppService<Citizen, CitizenDto, Guid, CitizenDto, CreateUpdateCitizenDto>, ICitizenAppService
{
    private readonly ICitizenRepository _citizenRepository;

    public CitizenAppService(ICitizenRepository repository) : base(repository)
    {
        _citizenRepository = repository;

        GetPolicyName = CitizensPortalPermissions.Citizens.Default;
        GetListPolicyName = CitizensPortalPermissions.Citizens.Default;
        CreatePolicyName = CitizensPortalPermissions.Citizens.Create;
        UpdatePolicyName = CitizensPortalPermissions.Citizens.Edit;
        DeletePolicyName = CitizensPortalPermissions.Citizens.Delete;
    }

    public async Task<CitizenDto> GetByEmailAsync(string email)
    {
        var citizen = await _citizenRepository.FindByEmailAsync(email);
        return ObjectMapper.Map<Citizen, CitizenDto>(citizen);
    }

    [Authorize]
    public async Task<CitizenDto> GetCurrentCitizenAsync()
    {
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null)
        {
            throw new Volo.Abp.BusinessException("CITIZEN_NOT_FOUND")
                .WithData("message", $"No citizen record found for email: {currentUserEmail}");
        }

        return ObjectMapper.Map<Citizen, CitizenDto>(citizen);
    }
}
