using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using CitizensPortal.Domain.Shared;

namespace CitizensPortal.Application.Contracts;

[DependsOn(
    typeof(CitizensPortalDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
)]
public class CitizensPortalApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        CitizensPortalDtoExtensions.Configure();
    }
}
