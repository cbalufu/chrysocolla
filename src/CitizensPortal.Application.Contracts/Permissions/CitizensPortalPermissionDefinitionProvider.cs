using CitizensPortal.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CitizensPortal.Permissions;

public class CitizensPortalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var citizensPortalGroup = context.AddGroup(CitizensPortalPermissions.GroupName, L("Permission:CitizensPortal"));

        // Dashboard
        var dashboardPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Dashboard.Default, L("Permission:Dashboard"));

        // Citizens
        var citizensPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Citizens.Default, L("Permission:Citizens"));
        citizensPermission.AddChild(CitizensPortalPermissions.Citizens.Create, L("Permission:Citizens.Create"));
        citizensPermission.AddChild(CitizensPortalPermissions.Citizens.Edit, L("Permission:Citizens.Edit"));
        citizensPermission.AddChild(CitizensPortalPermissions.Citizens.Delete, L("Permission:Citizens.Delete"));

        // Issue Reports
        var issueReportsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.IssueReports.Default, L("Permission:IssueReports"));
        issueReportsPermission.AddChild(CitizensPortalPermissions.IssueReports.Create, L("Permission:IssueReports.Create"));
        issueReportsPermission.AddChild(CitizensPortalPermissions.IssueReports.Edit, L("Permission:IssueReports.Edit"));
        issueReportsPermission.AddChild(CitizensPortalPermissions.IssueReports.Delete, L("Permission:IssueReports.Delete"));
        issueReportsPermission.AddChild(CitizensPortalPermissions.IssueReports.Resolve, L("Permission:IssueReports.Resolve"));
        issueReportsPermission.AddChild(CitizensPortalPermissions.IssueReports.Assign, L("Permission:IssueReports.Assign"));

        // Applications
        var applicationsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Applications.Default, L("Permission:Applications"));
        applicationsPermission.AddChild(CitizensPortalPermissions.Applications.Create, L("Permission:Applications.Create"));
        applicationsPermission.AddChild(CitizensPortalPermissions.Applications.Edit, L("Permission:Applications.Edit"));
        applicationsPermission.AddChild(CitizensPortalPermissions.Applications.Delete, L("Permission:Applications.Delete"));
        applicationsPermission.AddChild(CitizensPortalPermissions.Applications.Approve, L("Permission:Applications.Approve"));
        applicationsPermission.AddChild(CitizensPortalPermissions.Applications.Reject, L("Permission:Applications.Reject"));

        // Bills
        var billsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Bills.Default, L("Permission:Bills"));
        billsPermission.AddChild(CitizensPortalPermissions.Bills.Create, L("Permission:Bills.Create"));
        billsPermission.AddChild(CitizensPortalPermissions.Bills.Edit, L("Permission:Bills.Edit"));
        billsPermission.AddChild(CitizensPortalPermissions.Bills.Delete, L("Permission:Bills.Delete"));
        billsPermission.AddChild(CitizensPortalPermissions.Bills.View, L("Permission:Bills.View"));

        // Payments
        var paymentsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Payments.Default, L("Permission:Payments"));
        paymentsPermission.AddChild(CitizensPortalPermissions.Payments.Create, L("Permission:Payments.Create"));
        paymentsPermission.AddChild(CitizensPortalPermissions.Payments.Process, L("Permission:Payments.Process"));
        paymentsPermission.AddChild(CitizensPortalPermissions.Payments.Refund, L("Permission:Payments.Refund"));
        paymentsPermission.AddChild(CitizensPortalPermissions.Payments.View, L("Permission:Payments.View"));

        // Notifications
        var notificationsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Notifications.Default, L("Permission:Notifications"));
        notificationsPermission.AddChild(CitizensPortalPermissions.Notifications.Create, L("Permission:Notifications.Create"));
        notificationsPermission.AddChild(CitizensPortalPermissions.Notifications.Send, L("Permission:Notifications.Send"));
        notificationsPermission.AddChild(CitizensPortalPermissions.Notifications.Delete, L("Permission:Notifications.Delete"));

        // Tickets
        var ticketsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Tickets.Default, L("Permission:Tickets"));
        ticketsPermission.AddChild(CitizensPortalPermissions.Tickets.Create, L("Permission:Tickets.Create"));
        ticketsPermission.AddChild(CitizensPortalPermissions.Tickets.Edit, L("Permission:Tickets.Edit"));
        ticketsPermission.AddChild(CitizensPortalPermissions.Tickets.Delete, L("Permission:Tickets.Delete"));
        ticketsPermission.AddChild(CitizensPortalPermissions.Tickets.Assign, L("Permission:Tickets.Assign"));
        ticketsPermission.AddChild(CitizensPortalPermissions.Tickets.Close, L("Permission:Tickets.Close"));

        // Documents
        var documentsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Documents.Default, L("Permission:Documents"));
        documentsPermission.AddChild(CitizensPortalPermissions.Documents.Create, L("Permission:Documents.Create"));
        documentsPermission.AddChild(CitizensPortalPermissions.Documents.Edit, L("Permission:Documents.Edit"));
        documentsPermission.AddChild(CitizensPortalPermissions.Documents.Delete, L("Permission:Documents.Delete"));
        documentsPermission.AddChild(CitizensPortalPermissions.Documents.Download, L("Permission:Documents.Download"));
        documentsPermission.AddChild(CitizensPortalPermissions.Documents.Sign, L("Permission:Documents.Sign"));
        documentsPermission.AddChild(CitizensPortalPermissions.Documents.Share, L("Permission:Documents.Share"));

        // Properties
        var propertiesPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Properties.Default, L("Permission:Properties"));
        propertiesPermission.AddChild(CitizensPortalPermissions.Properties.Create, L("Permission:Properties.Create"));
        propertiesPermission.AddChild(CitizensPortalPermissions.Properties.Edit, L("Permission:Properties.Edit"));
        propertiesPermission.AddChild(CitizensPortalPermissions.Properties.Delete, L("Permission:Properties.Delete"));
        propertiesPermission.AddChild(CitizensPortalPermissions.Properties.ManageOwnership, L("Permission:Properties.ManageOwnership"));
        propertiesPermission.AddChild(CitizensPortalPermissions.Properties.ManageValuation, L("Permission:Properties.ManageValuation"));

        // Appointments
        var appointmentsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Appointments.Default, L("Permission:Appointments"));
        appointmentsPermission.AddChild(CitizensPortalPermissions.Appointments.Create, L("Permission:Appointments.Create"));
        appointmentsPermission.AddChild(CitizensPortalPermissions.Appointments.Edit, L("Permission:Appointments.Edit"));
        appointmentsPermission.AddChild(CitizensPortalPermissions.Appointments.Delete, L("Permission:Appointments.Delete"));
        appointmentsPermission.AddChild(CitizensPortalPermissions.Appointments.Approve, L("Permission:Appointments.Approve"));
        appointmentsPermission.AddChild(CitizensPortalPermissions.Appointments.ManageSlots, L("Permission:Appointments.ManageSlots"));

        // Certificates
        var certificatesPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Certificates.Default, L("Permission:Certificates"));
        certificatesPermission.AddChild(CitizensPortalPermissions.Certificates.Request, L("Permission:Certificates.Request"));
        certificatesPermission.AddChild(CitizensPortalPermissions.Certificates.Issue, L("Permission:Certificates.Issue"));
        certificatesPermission.AddChild(CitizensPortalPermissions.Certificates.Revoke, L("Permission:Certificates.Revoke"));
        certificatesPermission.AddChild(CitizensPortalPermissions.Certificates.Download, L("Permission:Certificates.Download"));

        // Infrastructure Projects
        var infrastructureProjectsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.InfrastructureProjects.Default, L("Permission:InfrastructureProjects"));
        infrastructureProjectsPermission.AddChild(CitizensPortalPermissions.InfrastructureProjects.Create, L("Permission:InfrastructureProjects.Create"));
        infrastructureProjectsPermission.AddChild(CitizensPortalPermissions.InfrastructureProjects.Edit, L("Permission:InfrastructureProjects.Edit"));
        infrastructureProjectsPermission.AddChild(CitizensPortalPermissions.InfrastructureProjects.Delete, L("Permission:InfrastructureProjects.Delete"));
        infrastructureProjectsPermission.AddChild(CitizensPortalPermissions.InfrastructureProjects.AddUpdate, L("Permission:InfrastructureProjects.AddUpdate"));
        infrastructureProjectsPermission.AddChild(CitizensPortalPermissions.InfrastructureProjects.ManageDocuments, L("Permission:InfrastructureProjects.ManageDocuments"));

        // Surveys
        var surveysPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Surveys.Default, L("Permission:Surveys"));
        surveysPermission.AddChild(CitizensPortalPermissions.Surveys.Create, L("Permission:Surveys.Create"));
        surveysPermission.AddChild(CitizensPortalPermissions.Surveys.Edit, L("Permission:Surveys.Edit"));
        surveysPermission.AddChild(CitizensPortalPermissions.Surveys.Delete, L("Permission:Surveys.Delete"));
        surveysPermission.AddChild(CitizensPortalPermissions.Surveys.Respond, L("Permission:Surveys.Respond"));
        surveysPermission.AddChild(CitizensPortalPermissions.Surveys.ViewResults, L("Permission:Surveys.ViewResults"));

        // Consultations
        var consultationsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Consultations.Default, L("Permission:Consultations"));
        consultationsPermission.AddChild(CitizensPortalPermissions.Consultations.Create, L("Permission:Consultations.Create"));
        consultationsPermission.AddChild(CitizensPortalPermissions.Consultations.Edit, L("Permission:Consultations.Edit"));
        consultationsPermission.AddChild(CitizensPortalPermissions.Consultations.Delete, L("Permission:Consultations.Delete"));
        consultationsPermission.AddChild(CitizensPortalPermissions.Consultations.Participate, L("Permission:Consultations.Participate"));
        consultationsPermission.AddChild(CitizensPortalPermissions.Consultations.ViewFeedback, L("Permission:Consultations.ViewFeedback"));

        // Votes
        var votesPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Votes.Default, L("Permission:Votes"));
        votesPermission.AddChild(CitizensPortalPermissions.Votes.Create, L("Permission:Votes.Create"));
        votesPermission.AddChild(CitizensPortalPermissions.Votes.Edit, L("Permission:Votes.Edit"));
        votesPermission.AddChild(CitizensPortalPermissions.Votes.Delete, L("Permission:Votes.Delete"));
        votesPermission.AddChild(CitizensPortalPermissions.Votes.Vote, L("Permission:Votes.Vote"));
        votesPermission.AddChild(CitizensPortalPermissions.Votes.ViewResults, L("Permission:Votes.ViewResults"));

        // Forums
        var forumsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.Forums.Default, L("Permission:Forums"));
        forumsPermission.AddChild(CitizensPortalPermissions.Forums.Create, L("Permission:Forums.Create"));
        forumsPermission.AddChild(CitizensPortalPermissions.Forums.Edit, L("Permission:Forums.Edit"));
        forumsPermission.AddChild(CitizensPortalPermissions.Forums.Delete, L("Permission:Forums.Delete"));
        forumsPermission.AddChild(CitizensPortalPermissions.Forums.Post, L("Permission:Forums.Post"));
        forumsPermission.AddChild(CitizensPortalPermissions.Forums.Moderate, L("Permission:Forums.Moderate"));

        // Emergency Alerts
        var emergencyAlertsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.EmergencyAlerts.Default, L("Permission:EmergencyAlerts"));
        emergencyAlertsPermission.AddChild(CitizensPortalPermissions.EmergencyAlerts.Create, L("Permission:EmergencyAlerts.Create"));
        emergencyAlertsPermission.AddChild(CitizensPortalPermissions.EmergencyAlerts.Edit, L("Permission:EmergencyAlerts.Edit"));
        emergencyAlertsPermission.AddChild(CitizensPortalPermissions.EmergencyAlerts.Delete, L("Permission:EmergencyAlerts.Delete"));
        emergencyAlertsPermission.AddChild(CitizensPortalPermissions.EmergencyAlerts.Broadcast, L("Permission:EmergencyAlerts.Broadcast"));
        emergencyAlertsPermission.AddChild(CitizensPortalPermissions.EmergencyAlerts.ManageRoutes, L("Permission:EmergencyAlerts.ManageRoutes"));

        // Service Requests
        var serviceRequestsPermission = citizensPortalGroup.AddPermission(CitizensPortalPermissions.ServiceRequests.Default, L("Permission:ServiceRequests"));
        serviceRequestsPermission.AddChild(CitizensPortalPermissions.ServiceRequests.Create, L("Permission:ServiceRequests.Create"));
        serviceRequestsPermission.AddChild(CitizensPortalPermissions.ServiceRequests.Edit, L("Permission:ServiceRequests.Edit"));
        serviceRequestsPermission.AddChild(CitizensPortalPermissions.ServiceRequests.Delete, L("Permission:ServiceRequests.Delete"));
        serviceRequestsPermission.AddChild(CitizensPortalPermissions.ServiceRequests.Assign, L("Permission:ServiceRequests.Assign"));
        serviceRequestsPermission.AddChild(CitizensPortalPermissions.ServiceRequests.Complete, L("Permission:ServiceRequests.Complete"));
        serviceRequestsPermission.AddChild(CitizensPortalPermissions.ServiceRequests.Rate, L("Permission:ServiceRequests.Rate"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CitizensPortalResource>(name);
    }
}
