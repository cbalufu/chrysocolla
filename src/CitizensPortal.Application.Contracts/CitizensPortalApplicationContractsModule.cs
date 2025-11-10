using Volo.Abp.Application;
using Volo.Abp.Modularity;
using CitizensPortal.Domain.Shared;

namespace CitizensPortal.Application.Contracts
{
    [DependsOn(
        typeof(CitizensPortalDomainSharedModule),
        typeof(AbpDddApplicationContractsModule)
    )]
    public class CitizensPortalApplicationContractsModule : AbpModule
    {
    }
}
