using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.ServiceRequest;
using CitizensPortal.Application.Contracts.Services;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/service-requests")]
    public class ServiceRequestController : AbpControllerBase
    {
        private readonly IServiceRequestAppService _serviceRequestAppService;

        public ServiceRequestController(IServiceRequestAppService serviceRequestAppService)
        {
            _serviceRequestAppService = serviceRequestAppService;
        }

        [HttpGet]
        public Task<Volo.Abp.Application.Dtos.PagedResultDto<ServiceRequestDto>> GetListAsync(ServiceRequestDto input)
        {
            return _serviceRequestAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public Task<ServiceRequestDto> GetAsync(Guid id)
        {
            return _serviceRequestAppService.GetAsync(id);
        }

        [HttpGet("my-requests")]
        public Task<List<ServiceRequestDto>> GetMyServiceRequestsAsync()
        {
            return _serviceRequestAppService.GetMyServiceRequestsAsync();
        }

        [HttpGet("by-number/{requestNumber}")]
        public Task<ServiceRequestDto> GetByRequestNumberAsync(string requestNumber)
        {
            return _serviceRequestAppService.GetByRequestNumberAsync(requestNumber);
        }

        [HttpPost]
        public Task<ServiceRequestDto> CreateAsync(CreateUpdateServiceRequestDto input)
        {
            return _serviceRequestAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<ServiceRequestDto> UpdateAsync(Guid id, CreateUpdateServiceRequestDto input)
        {
            return _serviceRequestAppService.UpdateAsync(id, input);
        }

        [HttpPost("{id}/rate")]
        public Task RateServiceAsync(Guid id, [FromBody] RateServiceInput input)
        {
            return _serviceRequestAppService.RateServiceAsync(id, input.Rating, input.Feedback);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _serviceRequestAppService.DeleteAsync(id);
        }
    }

    public class RateServiceInput
    {
        public int Rating { get; set; }
        public string Feedback { get; set; }
    }
}
