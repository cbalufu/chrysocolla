using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using CitizensPortal.Domain;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.EntityFrameworkCore.Repositories;

namespace CitizensPortal.EntityFrameworkCore
{
    [DependsOn(
        typeof(CitizensPortalDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class CitizensPortalEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CitizensPortalDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            // Register custom repositories
            context.Services.AddTransient<ICitizenRepository, CitizenRepository>();
        }
    }
}
