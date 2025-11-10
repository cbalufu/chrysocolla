using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;

namespace CitizensPortal.EntityFrameworkCore.Repositories
{
    public class CitizenRepository : EfCoreRepository<CitizensPortalDbContext, Citizen, Guid>, ICitizenRepository
    {
        public CitizenRepository(IDbContextProvider<CitizensPortalDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Citizen> FindByEmailAsync(string email)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Citizen> FindByNationalIdAsync(string nationalId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(c => c.NationalId == nationalId);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null)
        {
            var dbSet = await GetDbSetAsync();
            var query = dbSet.Where(c => c.Email == email);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}
