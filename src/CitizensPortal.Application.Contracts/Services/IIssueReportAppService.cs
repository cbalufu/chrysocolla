using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.IssueReport;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IIssueReportAppService : ICrudAppService<IssueReportDto, Guid, IssueReportDto, CreateUpdateIssueReportDto>
    {
        Task<List<IssueReportDto>> GetMyCitizenIssuesAsync();
        Task<IssueReportDto> GetByReferenceNumberAsync(string referenceNumber);
        Task AddCommentAsync(Guid id, string comment);
    }
}
