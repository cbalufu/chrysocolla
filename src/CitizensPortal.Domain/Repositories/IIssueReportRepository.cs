using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Repositories
{
    public interface IIssueReportRepository : IRepository<IssueReport, Guid>
    {
        Task<List<IssueReport>> GetByCitizenIdAsync(Guid citizenId);
        Task<List<IssueReport>> GetByStatusAsync(IssueStatus status);
        Task<IssueReport> FindByReferenceNumberAsync(string referenceNumber);
    }
}
