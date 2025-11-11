using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Application;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Applications.Default)]
public class ApplicationAppService : CrudAppService<Domain.Entities.Application, ApplicationDto, Guid, ApplicationDto, CreateUpdateApplicationDto>, IApplicationAppService
{
    private readonly IRepository<Domain.Entities.Application, Guid> _applicationRepository;
    private readonly ICitizenRepository _citizenRepository;

    public ApplicationAppService(
        IRepository<Domain.Entities.Application, Guid> repository,
        ICitizenRepository citizenRepository) : base(repository)
    {
        _applicationRepository = repository;
        _citizenRepository = citizenRepository;

        GetPolicyName = CitizensPortalPermissions.Applications.Default;
        GetListPolicyName = CitizensPortalPermissions.Applications.Default;
        CreatePolicyName = CitizensPortalPermissions.Applications.Create;
        UpdatePolicyName = CitizensPortalPermissions.Applications.Edit;
        DeletePolicyName = CitizensPortalPermissions.Applications.Delete;
    }

    [Authorize]
    public async Task<List<ApplicationDto>> GetMyApplicationsAsync()
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
            return new List<ApplicationDto>();
        }

        var applications = await _applicationRepository.GetListAsync();
        var myApplications = applications.Where(a => a.CitizenId == citizen.Id).ToList();

        return ObjectMapper.Map<List<Domain.Entities.Application>, List<ApplicationDto>>(myApplications);
    }

    public async Task<ApplicationDto> GetByApplicationNumberAsync(string applicationNumber)
    {
        var applications = await _applicationRepository.GetListAsync();
        var application = applications.FirstOrDefault(a => a.ApplicationNumber == applicationNumber);

        if (application == null)
        {
            throw new Volo.Abp.BusinessException("APPLICATION_NOT_FOUND")
                .WithData("ApplicationNumber", applicationNumber);
        }

        return ObjectMapper.Map<Domain.Entities.Application, ApplicationDto>(application);
    }

    [Authorize(CitizensPortalPermissions.Applications.Edit)]
    public async Task SubmitApplicationAsync(Guid id)
    {
        var application = await _applicationRepository.GetAsync(id);

        if (application.Status != ApplicationStatus.Draft)
        {
            throw new Volo.Abp.BusinessException("APPLICATION_ALREADY_SUBMITTED")
                .WithData("message", "Application has already been submitted");
        }

        application.Status = ApplicationStatus.Submitted;
        application.SubmittedDate = DateTime.UtcNow;

        // Add status history
        application.StatusHistory.Add(new ApplicationStatusHistory(
            Guid.NewGuid(),
            id,
            ApplicationStatus.Submitted,
            DateTime.UtcNow,
            "Application submitted by citizen"
        ));

        await _applicationRepository.UpdateAsync(application);
    }

    [Authorize(CitizensPortalPermissions.Applications.Edit)]
    public async Task WithdrawApplicationAsync(Guid id)
    {
        var application = await _applicationRepository.GetAsync(id);

        if (application.Status == ApplicationStatus.Approved ||
            application.Status == ApplicationStatus.Rejected ||
            application.Status == ApplicationStatus.Withdrawn)
        {
            throw new Volo.Abp.BusinessException("APPLICATION_CANNOT_BE_WITHDRAWN")
                .WithData("message", $"Application with status {application.Status} cannot be withdrawn");
        }

        application.Status = ApplicationStatus.Withdrawn;

        // Add status history
        application.StatusHistory.Add(new ApplicationStatusHistory(
            Guid.NewGuid(),
            id,
            ApplicationStatus.Withdrawn,
            DateTime.UtcNow,
            "Application withdrawn by citizen"
        ));

        await _applicationRepository.UpdateAsync(application);
    }
}
