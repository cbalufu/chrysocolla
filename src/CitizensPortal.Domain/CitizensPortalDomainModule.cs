using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using CitizensPortal.Domain.Shared;

namespace CitizensPortal.Domain
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(CitizensPortalDomainSharedModule)
    )]
    public class CitizensPortalDomainModule : AbpModule
    {
    }
}
