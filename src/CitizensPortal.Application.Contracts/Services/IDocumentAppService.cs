using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Document;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IDocumentAppService : ICrudAppService<DocumentDto, Guid, DocumentDto, CreateUpdateDocumentDto>
    {
        Task<List<DocumentDto>> GetMyDocumentsAsync();
        Task<List<DocumentDto>> GetByCategoryAsync(Domain.Enums.DocumentCategory category);
        Task<List<DocumentDto>> GetExpiringDocumentsAsync(int daysAhead);
        Task<DocumentDto> SignDocumentAsync(Guid id);
        Task DownloadDocumentAsync(Guid id);
    }
}
