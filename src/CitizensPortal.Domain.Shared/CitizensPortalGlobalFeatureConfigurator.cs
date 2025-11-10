using Volo.Abp.Threading;

namespace CitizensPortal.Domain.Shared;

public static class CitizensPortalGlobalFeatureConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            // Enable global features here
            // Example: GlobalFeatureManager.Instance.Modules.Ecommerce().EnableAll();
        });
    }
}
