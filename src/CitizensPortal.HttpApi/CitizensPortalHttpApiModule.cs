using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using CitizensPortal.Application.Contracts;

namespace CitizensPortal.HttpApi
{
    [DependsOn(
        typeof(CitizensPortalApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class CitizensPortalHttpApiModule : AbpModule
    {
    }
}
