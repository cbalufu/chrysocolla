using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace CitizensPortal.Domain.Shared
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class CitizensPortalDomainSharedModule : AbpModule
    {
    }
}
