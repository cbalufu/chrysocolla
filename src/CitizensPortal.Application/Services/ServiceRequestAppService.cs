using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.ServiceRequest;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.ServiceRequests.Default)]
public class ServiceRequestAppService : CrudAppService<ServiceRequest, ServiceRequestDto, Guid, ServiceRequestDto, CreateUpdateServiceRequestDto>, IServiceRequestAppService
{
    private readonly IRepository<ServiceRequest, Guid> _serviceRequestRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public ServiceRequestAppService(
        IRepository<ServiceRequest, Guid> repository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _serviceRequestRepository = repository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.ServiceRequests.Default;
        GetListPolicyName = CitizensPortalPermissions.ServiceRequests.Default;
        CreatePolicyName = CitizensPortalPermissions.ServiceRequests.Create;
        UpdatePolicyName = CitizensPortalPermissions.ServiceRequests.Edit;
        DeletePolicyName = CitizensPortalPermissions.ServiceRequests.Delete;
    }

    [Authorize]
    public async Task<List<ServiceRequestDto>> GetMyServiceRequestsAsync()
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
            return new List<ServiceRequestDto>();
        }

        var requests = await _serviceRequestRepository.GetListAsync();
        var myRequests = requests.Where(r => r.CitizenId == citizen.Id)
                                .OrderByDescending(r => r.RequestDate)
                                .ToList();

        return ObjectMapper.Map<List<ServiceRequest>, List<ServiceRequestDto>>(myRequests);
    }

    public async Task<ServiceRequestDto> GetByRequestNumberAsync(string requestNumber)
    {
        if (string.IsNullOrWhiteSpace(requestNumber))
        {
            throw new Volo.Abp.BusinessException("REQUEST_NUMBER_REQUIRED")
                .WithData("message", "Request number is required");
        }

        var requests = await _serviceRequestRepository.GetListAsync();
        var request = requests.FirstOrDefault(r => r.RequestNumber == requestNumber);

        if (request == null)
        {
            throw new Volo.Abp.BusinessException("REQUEST_NOT_FOUND")
                .WithData("RequestNumber", requestNumber);
        }

        return ObjectMapper.Map<ServiceRequest, ServiceRequestDto>(request);
    }

    [Authorize(CitizensPortalPermissions.ServiceRequests.Rate)]
    public async Task RateServiceAsync(Guid id, int rating, string feedback)
    {
        if (rating < 1 || rating > 5)
        {
            throw new Volo.Abp.BusinessException("INVALID_RATING")
                .WithData("message", "Rating must be between 1 and 5");
        }

        var request = await _serviceRequestRepository.GetAsync(id);

        // Verify ownership
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null || request.CitizenId != citizen.Id)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to rate this service request");
        }

        if (request.Status != ServiceRequestStatus.Completed)
        {
            throw new Volo.Abp.BusinessException("REQUEST_NOT_COMPLETED")
                .WithData("message", "Only completed requests can be rated");
        }

        if (request.Rating.HasValue)
        {
            throw new Volo.Abp.BusinessException("REQUEST_ALREADY_RATED")
                .WithData("message", "This request has already been rated");
        }

        request.Rating = rating;
        request.Feedback = feedback;

        await _serviceRequestRepository.UpdateAsync(request);
    }

    public override async Task<ServiceRequestDto> CreateAsync(CreateUpdateServiceRequestDto input)
    {
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

        // Create service request
        var request = new ServiceRequest(
            _guidGenerator.Create(),
            citizen.Id,
            input.ServiceType,
            input.Title,
            input.Description
        );

        request.RequestNumber = $"SR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        request.Status = ServiceRequestStatus.Submitted;
        request.Priority = input.Priority;
        request.RequestDate = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(input.Location))
        {
            request.Location = input.Location;
        }

        var createdRequest = await _serviceRequestRepository.InsertAsync(request);

        return ObjectMapper.Map<ServiceRequest, ServiceRequestDto>(createdRequest);
    }
}
