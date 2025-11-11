using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.Document;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/documents")]
    public class DocumentController : AbpControllerBase
    {
        private readonly IDocumentAppService _documentAppService;

        public DocumentController(IDocumentAppService documentAppService)
        {
            _documentAppService = documentAppService;
        }

        [HttpGet]
        public Task<Volo.Abp.Application.Dtos.PagedResultDto<DocumentDto>> GetListAsync(DocumentDto input)
        {
            return _documentAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public Task<DocumentDto> GetAsync(Guid id)
        {
            return _documentAppService.GetAsync(id);
        }

        [HttpGet("my-documents")]
        public Task<List<DocumentDto>> GetMyDocumentsAsync()
        {
            return _documentAppService.GetMyDocumentsAsync();
        }

        [HttpGet("category/{category}")]
        public Task<List<DocumentDto>> GetByCategoryAsync(DocumentCategory category)
        {
            return _documentAppService.GetByCategoryAsync(category);
        }

        [HttpGet("expiring/{daysAhead}")]
        public Task<List<DocumentDto>> GetExpiringDocumentsAsync(int daysAhead)
        {
            return _documentAppService.GetExpiringDocumentsAsync(daysAhead);
        }

        [HttpPost]
        public Task<DocumentDto> CreateAsync(CreateUpdateDocumentDto input)
        {
            return _documentAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<DocumentDto> UpdateAsync(Guid id, CreateUpdateDocumentDto input)
        {
            return _documentAppService.UpdateAsync(id, input);
        }

        [HttpPost("{id}/sign")]
        public Task<DocumentDto> SignDocumentAsync(Guid id)
        {
            return _documentAppService.SignDocumentAsync(id);
        }

        [HttpGet("{id}/download")]
        public Task DownloadDocumentAsync(Guid id)
        {
            return _documentAppService.DownloadDocumentAsync(id);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _documentAppService.DeleteAsync(id);
        }
    }
}
