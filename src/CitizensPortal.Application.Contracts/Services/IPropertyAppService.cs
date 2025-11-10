using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Property;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IPropertyAppService : ICrudAppService<PropertyDto, Guid, PropertyDto, CreateUpdatePropertyDto>
    {
        Task<List<PropertyDto>> GetMyPropertiesAsync();
        Task<PropertyDto> GetByPropertyReferenceAsync(string propertyReference);
        Task<List<PropertyDto>> GetPropertiesInWardAsync(string ward);
    }
}
