using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Repositories
{
    public interface IApplicationRepository : IRepository<Application, Guid>
    {
        Task<List<Application>> GetByCitizenIdAsync(Guid citizenId);
        Task<List<Application>> GetByStatusAsync(ApplicationStatus status);
        Task<Application> FindByApplicationNumberAsync(string applicationNumber);
    }
}
