using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.TenantManagement;
using CitizensPortal.Domain.Shared;

namespace CitizensPortal.Domain;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpTenantManagementDomainModule),
    typeof(CitizensPortalDomainSharedModule)
)]
public class CitizensPortalDomainModule : AbpModule
{
}
