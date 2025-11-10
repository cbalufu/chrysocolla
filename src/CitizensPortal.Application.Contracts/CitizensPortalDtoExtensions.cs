using Volo.Abp.Threading;

namespace CitizensPortal.Application.Contracts;

public static class CitizensPortalDtoExtensions
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            // Configure DTO extensions here if needed
            // Example: ObjectExtensionManager.Instance.MapEfCoreProperty<IdentityUser, string>("SocialSecurityNumber");
        });
    }
}
