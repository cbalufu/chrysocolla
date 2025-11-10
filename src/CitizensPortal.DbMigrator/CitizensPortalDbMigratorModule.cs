using CitizensPortal.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CitizensPortal.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CitizensPortalEntityFrameworkCoreModule),
    typeof(CitizensPortalApplicationContractsModule)
)]
public class CitizensPortalDbMigratorModule : AbpModule
{
}
