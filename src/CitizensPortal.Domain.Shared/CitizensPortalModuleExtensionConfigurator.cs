using Volo.Abp.Threading;

namespace CitizensPortal.Domain.Shared;

public static class CitizensPortalModuleExtensionConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            // Configure module extensions here
            // Example: ObjectExtensionManager.Instance.AddOrUpdateProperty<IdentityUser, string>("SocialSecurityNumber");
        });
    }
}
