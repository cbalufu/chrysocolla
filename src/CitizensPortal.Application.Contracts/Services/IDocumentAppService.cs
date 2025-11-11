using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using CitizensPortal.Application.Contracts.DTOs.Document;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IDocumentAppService : ICrudAppService<DocumentDto, Guid, DocumentDto, CreateUpdateDocumentDto>
    {
        Task<List<DocumentDto>> GetMyDocumentsAsync();
        Task<List<DocumentDto>> GetByCategoryAsync(DocumentCategory category);
        Task<List<DocumentDto>> GetExpiringDocumentsAsync(int daysAhead);
        Task<DocumentDto> SignDocumentAsync(Guid id);
        Task<DocumentDto> UploadDocumentAsync(
            string title,
            string description,
            DocumentCategory category,
            IRemoteStreamContent fileContent);
        Task<IRemoteStreamContent> DownloadDocumentAsync(Guid id);
    }
}
