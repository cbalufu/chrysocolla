using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Property;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Properties.Default)]
public class PropertyAppService : CrudAppService<Property, PropertyDto, Guid, PropertyDto, CreateUpdatePropertyDto>, IPropertyAppService
{
    private readonly IRepository<Property, Guid> _propertyRepository;
    private readonly IRepository<PropertyOwnership, Guid> _ownershipRepository;
    private readonly ICitizenRepository _citizenRepository;

    public PropertyAppService(
        IRepository<Property, Guid> repository,
        IRepository<PropertyOwnership, Guid> ownershipRepository,
        ICitizenRepository citizenRepository) : base(repository)
    {
        _propertyRepository = repository;
        _ownershipRepository = ownershipRepository;
        _citizenRepository = citizenRepository;

        GetPolicyName = CitizensPortalPermissions.Properties.Default;
        GetListPolicyName = CitizensPortalPermissions.Properties.Default;
        CreatePolicyName = CitizensPortalPermissions.Properties.Create;
        UpdatePolicyName = CitizensPortalPermissions.Properties.Edit;
        DeletePolicyName = CitizensPortalPermissions.Properties.Delete;
    }

    [Authorize]
    public async Task<List<PropertyDto>> GetMyPropertiesAsync()
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
            return new List<PropertyDto>();
        }

        // Get properties where citizen has ownership
        var ownerships = await _ownershipRepository.GetListAsync();
        var myOwnerships = ownerships.Where(o => o.CitizenId == citizen.Id && o.IsActive)
                                    .Select(o => o.PropertyId)
                                    .ToList();

        var properties = await _propertyRepository.GetListAsync();
        var myProperties = properties.Where(p => myOwnerships.Contains(p.Id))
                                    .OrderBy(p => p.PropertyReference)
                                    .ToList();

        return ObjectMapper.Map<List<Property>, List<PropertyDto>>(myProperties);
    }

    public async Task<PropertyDto> GetByPropertyReferenceAsync(string propertyReference)
    {
        if (string.IsNullOrWhiteSpace(propertyReference))
        {
            throw new Volo.Abp.BusinessException("PROPERTY_REFERENCE_REQUIRED")
                .WithData("message", "Property reference is required");
        }

        var properties = await _propertyRepository.GetListAsync();
        var property = properties.FirstOrDefault(p => p.PropertyReference == propertyReference);

        if (property == null)
        {
            throw new Volo.Abp.BusinessException("PROPERTY_NOT_FOUND")
                .WithData("PropertyReference", propertyReference);
        }

        return ObjectMapper.Map<Property, PropertyDto>(property);
    }

    public async Task<List<PropertyDto>> GetPropertiesInWardAsync(string ward)
    {
        if (string.IsNullOrWhiteSpace(ward))
        {
            throw new Volo.Abp.BusinessException("WARD_REQUIRED")
                .WithData("message", "Ward is required");
        }

        var properties = await _propertyRepository.GetListAsync();
        var wardProperties = properties.Where(p => p.Ward?.Equals(ward, StringComparison.OrdinalIgnoreCase) == true)
                                      .OrderBy(p => p.PropertyReference)
                                      .ToList();

        return ObjectMapper.Map<List<Property>, List<PropertyDto>>(wardProperties);
    }
}
