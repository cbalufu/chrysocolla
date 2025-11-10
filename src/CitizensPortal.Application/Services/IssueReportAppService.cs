using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.IssueReport;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.IssueReports.Default)]
public class IssueReportAppService : CrudAppService<IssueReport, IssueReportDto, Guid, IssueReportDto, CreateUpdateIssueReportDto>, IIssueReportAppService
{
    private readonly IIssueReportRepository _issueReportRepository;
    private readonly ICitizenRepository _citizenRepository;

    public IssueReportAppService(
        IIssueReportRepository repository,
        ICitizenRepository citizenRepository) : base(repository)
    {
        _issueReportRepository = repository;
        _citizenRepository = citizenRepository;

        GetPolicyName = CitizensPortalPermissions.IssueReports.Default;
        GetListPolicyName = CitizensPortalPermissions.IssueReports.Default;
        CreatePolicyName = CitizensPortalPermissions.IssueReports.Create;
        UpdatePolicyName = CitizensPortalPermissions.IssueReports.Edit;
        DeletePolicyName = CitizensPortalPermissions.IssueReports.Delete;
    }

    [Authorize]
    public async Task<List<IssueReportDto>> GetMyCitizenIssuesAsync()
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
            return new List<IssueReportDto>();
        }

        var issues = await _issueReportRepository.GetListAsync();
        var myIssues = issues.Where(i => i.CitizenId == citizen.Id).ToList();

        return ObjectMapper.Map<List<IssueReport>, List<IssueReportDto>>(myIssues);
    }

    public async Task<IssueReportDto> GetByReferenceNumberAsync(string referenceNumber)
    {
        var issue = await _issueReportRepository.FindByReferenceNumberAsync(referenceNumber);
        return ObjectMapper.Map<IssueReport, IssueReportDto>(issue);
    }

    [Authorize(CitizensPortalPermissions.IssueReports.Edit)]
    public async Task AddCommentAsync(Guid id, string comment)
    {
        var issue = await _issueReportRepository.GetAsync(id);
        var issueComment = new IssueComment(Guid.NewGuid(), id, comment, false);
        issue.Comments.Add(issueComment);
        await _issueReportRepository.UpdateAsync(issue);
    }
}
