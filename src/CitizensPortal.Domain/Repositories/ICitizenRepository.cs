using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using CitizensPortal.Domain.Entities;

namespace CitizensPortal.Domain.Repositories
{
    public interface ICitizenRepository : IRepository<Citizen, Guid>
    {
        Task<Citizen> FindByEmailAsync(string email);
        Task<Citizen> FindByNationalIdAsync(string nationalId);
        Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null);
    }
}
