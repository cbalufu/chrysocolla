using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.IssueReport;
using CitizensPortal.Application.Contracts.Services;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/issue-reports")]
    public class IssueReportController : AbpControllerBase, IIssueReportAppService
    {
        private readonly IIssueReportAppService _issueReportAppService;

        public IssueReportController(IIssueReportAppService issueReportAppService)
        {
            _issueReportAppService = issueReportAppService;
        }

        [HttpGet("{id}")]
        public Task<IssueReportDto> GetAsync(Guid id)
        {
            return _issueReportAppService.GetAsync(id);
        }

        [HttpGet]
        public Task<Volo.Abp.Application.Dtos.PagedResultDto<IssueReportDto>> GetListAsync(IssueReportDto input)
        {
            return _issueReportAppService.GetListAsync(input);
        }

        [HttpPost]
        public Task<IssueReportDto> CreateAsync(CreateUpdateIssueReportDto input)
        {
            return _issueReportAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<IssueReportDto> UpdateAsync(Guid id, CreateUpdateIssueReportDto input)
        {
            return _issueReportAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _issueReportAppService.DeleteAsync(id);
        }

        [HttpGet("my-issues")]
        public Task<List<IssueReportDto>> GetMyCitizenIssuesAsync()
        {
            return _issueReportAppService.GetMyCitizenIssuesAsync();
        }

        [HttpGet("by-reference/{referenceNumber}")]
        public Task<IssueReportDto> GetByReferenceNumberAsync(string referenceNumber)
        {
            return _issueReportAppService.GetByReferenceNumberAsync(referenceNumber);
        }

        [HttpPost("{id}/comments")]
        public Task AddCommentAsync(Guid id, [FromBody] string comment)
        {
            return _issueReportAppService.AddCommentAsync(id, comment);
        }
    }
}
