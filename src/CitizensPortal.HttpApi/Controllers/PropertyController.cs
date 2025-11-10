using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.Property;
using CitizensPortal.Application.Contracts.Services;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/properties")]
    public class PropertyController : AbpControllerBase
    {
        private readonly IPropertyAppService _propertyAppService;

        public PropertyController(IPropertyAppService propertyAppService)
        {
            _propertyAppService = propertyAppService;
        }

        [HttpGet]
        public Task<Volo.Abp.Application.Dtos.PagedResultDto<PropertyDto>> GetListAsync(PropertyDto input)
        {
            return _propertyAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public Task<PropertyDto> GetAsync(Guid id)
        {
            return _propertyAppService.GetAsync(id);
        }

        [HttpGet("my-properties")]
        public Task<List<PropertyDto>> GetMyPropertiesAsync()
        {
            return _propertyAppService.GetMyPropertiesAsync();
        }

        [HttpGet("by-reference/{propertyReference}")]
        public Task<PropertyDto> GetByPropertyReferenceAsync(string propertyReference)
        {
            return _propertyAppService.GetByPropertyReferenceAsync(propertyReference);
        }

        [HttpGet("ward/{ward}")]
        public Task<List<PropertyDto>> GetPropertiesInWardAsync(string ward)
        {
            return _propertyAppService.GetPropertiesInWardAsync(ward);
        }

        [HttpPost]
        public Task<PropertyDto> CreateAsync(CreateUpdatePropertyDto input)
        {
            return _propertyAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<PropertyDto> UpdateAsync(Guid id, CreateUpdatePropertyDto input)
        {
            return _propertyAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _propertyAppService.DeleteAsync(id);
        }
    }
}
