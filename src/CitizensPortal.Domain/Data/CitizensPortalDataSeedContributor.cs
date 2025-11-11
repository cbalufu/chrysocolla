using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace CitizensPortal.Domain.Data;

public class CitizensPortalDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IConfiguration _configuration;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IIdentityDataSeeder _identityDataSeeder;

    public CitizensPortalDataSeedContributor(
        IConfiguration configuration,
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IPermissionDataSeeder permissionDataSeeder,
        IIdentityDataSeeder identityDataSeeder)
    {
        _configuration = configuration;
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _permissionDataSeeder = permissionDataSeeder;
        _identityDataSeeder = identityDataSeeder;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        /* Instead of returning the Task.CompletedTask, you can insert your test data
         * at this point!
         */

        using (_currentTenant.Change(context?.TenantId))
        {
            await _identityDataSeeder.SeedAsync(
                "admin@abp.io",
                "1q2w3E*",
                context?.TenantId
            );

            // Seed default admin role permissions
            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "admin",
                await GetDefaultAdminPermissionsAsync()
            );
        }
    }

    private async Task<string[]> GetDefaultAdminPermissionsAsync()
    {
        // Grant all Citizens Portal permissions to admin role
        return new[]
        {
            "CitizensPortal.Dashboard",
            "CitizensPortal.Citizens",
            "CitizensPortal.Citizens.Create",
            "CitizensPortal.Citizens.Edit",
            "CitizensPortal.Citizens.Delete",
            "CitizensPortal.IssueReports",
            "CitizensPortal.IssueReports.Create",
            "CitizensPortal.IssueReports.Edit",
            "CitizensPortal.IssueReports.Delete",
            "CitizensPortal.IssueReports.Resolve",
            "CitizensPortal.IssueReports.Assign",
            "CitizensPortal.Applications",
            "CitizensPortal.Applications.Create",
            "CitizensPortal.Applications.Edit",
            "CitizensPortal.Applications.Delete",
            "CitizensPortal.Applications.Approve",
            "CitizensPortal.Applications.Reject",
            "CitizensPortal.Bills",
            "CitizensPortal.Bills.Create",
            "CitizensPortal.Bills.Edit",
            "CitizensPortal.Bills.Delete",
            "CitizensPortal.Bills.View",
            "CitizensPortal.Payments",
            "CitizensPortal.Payments.Create",
            "CitizensPortal.Payments.Process",
            "CitizensPortal.Payments.Refund",
            "CitizensPortal.Payments.View",
            "CitizensPortal.Notifications",
            "CitizensPortal.Notifications.Create",
            "CitizensPortal.Notifications.Send",
            "CitizensPortal.Notifications.Delete",
            "CitizensPortal.Tickets",
            "CitizensPortal.Tickets.Create",
            "CitizensPortal.Tickets.Edit",
            "CitizensPortal.Tickets.Delete",
            "CitizensPortal.Tickets.Assign",
            "CitizensPortal.Tickets.Close",
            "CitizensPortal.Documents",
            "CitizensPortal.Documents.Create",
            "CitizensPortal.Documents.Edit",
            "CitizensPortal.Documents.Delete",
            "CitizensPortal.Documents.Download",
            "CitizensPortal.Documents.Sign",
            "CitizensPortal.Documents.Share",
            "CitizensPortal.Properties",
            "CitizensPortal.Properties.Create",
            "CitizensPortal.Properties.Edit",
            "CitizensPortal.Properties.Delete",
            "CitizensPortal.Properties.ManageOwnership",
            "CitizensPortal.Properties.ManageValuation",
            "CitizensPortal.Appointments",
            "CitizensPortal.Appointments.Create",
            "CitizensPortal.Appointments.Edit",
            "CitizensPortal.Appointments.Delete",
            "CitizensPortal.Appointments.Approve",
            "CitizensPortal.Appointments.ManageSlots",
            "CitizensPortal.Certificates",
            "CitizensPortal.Certificates.Request",
            "CitizensPortal.Certificates.Issue",
            "CitizensPortal.Certificates.Revoke",
            "CitizensPortal.Certificates.Download"
        };
    }
}
