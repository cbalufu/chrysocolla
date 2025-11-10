using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using CitizensPortal.Application.Contracts.DTOs.IssueReport;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;

namespace CitizensPortal.Application.Services
{
    public class IssueReportAppService : CrudAppService<IssueReport, IssueReportDto, Guid, IssueReportDto, CreateUpdateIssueReportDto>, IIssueReportAppService
    {
        private readonly IIssueReportRepository _issueReportRepository;

        public IssueReportAppService(IIssueReportRepository repository) : base(repository)
        {
            _issueReportRepository = repository;
        }

        public async Task<List<IssueReportDto>> GetMyCitizenIssuesAsync()
        {
            // TODO: Get current citizen ID from authentication context
            throw new NotImplementedException("Implement based on your authentication setup");
        }

        public async Task<IssueReportDto> GetByReferenceNumberAsync(string referenceNumber)
        {
            var issue = await _issueReportRepository.FindByReferenceNumberAsync(referenceNumber);
            return ObjectMapper.Map<IssueReport, IssueReportDto>(issue);
        }

        public async Task AddCommentAsync(Guid id, string comment)
        {
            var issue = await _issueReportRepository.GetAsync(id);
            var issueComment = new IssueComment(Guid.NewGuid(), id, comment, false);
            issue.Comments.Add(issueComment);
            await _issueReportRepository.UpdateAsync(issue);
        }
    }
}
