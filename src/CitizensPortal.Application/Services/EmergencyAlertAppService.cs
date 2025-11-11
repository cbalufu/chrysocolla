using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Emergency;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.EmergencyAlerts.Default)]
public class EmergencyAlertAppService : CrudAppService<EmergencyAlert, EmergencyAlertDto, Guid, EmergencyAlertDto, CreateUpdateEmergencyAlertDto>, IEmergencyAlertAppService
{
    private readonly IRepository<EmergencyAlert, Guid> _alertRepository;
    private readonly IRepository<EvacuationRoute, Guid> _routeRepository;
    private readonly IRepository<AlertAcknowledgement, Guid> _acknowledgementRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public EmergencyAlertAppService(
        IRepository<EmergencyAlert, Guid> repository,
        IRepository<EvacuationRoute, Guid> routeRepository,
        IRepository<AlertAcknowledgement, Guid> acknowledgementRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _alertRepository = repository;
        _routeRepository = routeRepository;
        _acknowledgementRepository = acknowledgementRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.EmergencyAlerts.Default;
        GetListPolicyName = CitizensPortalPermissions.EmergencyAlerts.Default;
        CreatePolicyName = CitizensPortalPermissions.EmergencyAlerts.Create;
        UpdatePolicyName = CitizensPortalPermissions.EmergencyAlerts.Edit;
        DeletePolicyName = CitizensPortalPermissions.EmergencyAlerts.Delete;
    }

    [AllowAnonymous]
    public async Task<List<EmergencyAlertDto>> GetActiveAlertsAsync()
    {
        var alerts = await _alertRepository.GetListAsync();
        var activeAlerts = alerts.Where(a => a.Status == EmergencyAlertStatus.Active)
                                 .OrderByDescending(a => a.CreationTime)
                                 .ToList();

        return ObjectMapper.Map<List<EmergencyAlert>, List<EmergencyAlertDto>>(activeAlerts);
    }

    [Authorize]
    public async Task<List<EmergencyAlertDto>> GetMyAreaAlertsAsync()
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
            return new List<EmergencyAlertDto>();
        }

        // Get alerts for citizen's ward/area
        var alerts = await _alertRepository.GetListAsync();
        var areaAlerts = alerts.Where(a => a.Status == EmergencyAlertStatus.Active &&
                                         (string.IsNullOrEmpty(a.AffectedArea) ||
                                          a.AffectedArea.Contains(citizen.Address ?? string.Empty)))
                              .OrderByDescending(a => a.CreationTime)
                              .ToList();

        return ObjectMapper.Map<List<EmergencyAlert>, List<EmergencyAlertDto>>(areaAlerts);
    }

    [Authorize]
    public async Task AcknowledgeAlertAsync(Guid id, bool isSafe, string notes)
    {
        var alert = await _alertRepository.GetAsync(id);

        // Get current citizen
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
                .WithData("message", "Citizen record not found");
        }

        // Check if already acknowledged
        var acknowledgements = await _acknowledgementRepository.GetListAsync();
        var existing = acknowledgements.FirstOrDefault(a => a.AlertId == id && a.CitizenId == citizen.Id);

        if (existing != null)
        {
            // Update existing acknowledgement
            existing.IsSafe = isSafe;
            existing.Notes = notes;
            existing.AcknowledgedAt = DateTime.UtcNow;
            await _acknowledgementRepository.UpdateAsync(existing);
        }
        else
        {
            // Create new acknowledgement
            var acknowledgement = new AlertAcknowledgement(
                _guidGenerator.Create(),
                id,
                citizen.Id,
                DateTime.UtcNow,
                isSafe,
                notes
            );

            await _acknowledgementRepository.InsertAsync(acknowledgement);
        }
    }

    [AllowAnonymous]
    public async Task<List<EvacuationRouteDto>> GetEvacuationRoutesAsync()
    {
        var routes = await _routeRepository.GetListAsync();
        var activeRoutes = routes.Where(r => r.IsActive)
                                 .OrderBy(r => r.RouteName)
                                 .ToList();

        return ObjectMapper.Map<List<EvacuationRoute>, List<EvacuationRouteDto>>(activeRoutes);
    }

    public override async Task<EmergencyAlertDto> CreateAsync(CreateUpdateEmergencyAlertDto input)
    {
        // Creating emergency alerts requires special permission
        await CheckCreatePolicyAsync();

        var alert = new EmergencyAlert(
            _guidGenerator.Create(),
            input.AlertType,
            input.Severity,
            input.Title,
            input.Message
        );

        alert.AffectedArea = input.AffectedArea;
        alert.Status = EmergencyAlertStatus.Active;

        var createdAlert = await _alertRepository.InsertAsync(alert);

        // In a real system, trigger notifications/SMS/push notifications here
        // await _notificationService.BroadcastEmergencyAlert(createdAlert);

        return ObjectMapper.Map<EmergencyAlert, EmergencyAlertDto>(createdAlert);
    }
}
