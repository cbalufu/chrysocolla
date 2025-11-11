using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.TenantManagement;
using CitizensPortal.Application.Contracts;

namespace CitizensPortal.HttpApi;

[DependsOn(
    typeof(CitizensPortalApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule)
)]
public class CitizensPortalHttpApiModule : AbpModule
{
}
