using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using CitizensPortal.Application.Contracts;
using CitizensPortal.Domain;

namespace CitizensPortal.Application
{
    [DependsOn(
        typeof(CitizensPortalDomainModule),
        typeof(CitizensPortalApplicationContractsModule),
        typeof(AbpAutoMapperModule)
    )]
    public class CitizensPortalApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CitizensPortalApplicationModule>();
            });
        }
    }
}
