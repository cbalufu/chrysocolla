namespace CitizensPortal.Permissions;

public static class CitizensPortalPermissions
{
    public const string GroupName = "CitizensPortal";

    public static class Dashboard
    {
        public const string Default = GroupName + ".Dashboard";
    }

    public static class Citizens
    {
        public const string Default = GroupName + ".Citizens";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class IssueReports
    {
        public const string Default = GroupName + ".IssueReports";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Resolve = Default + ".Resolve";
        public const string Assign = Default + ".Assign";
    }

    public static class Applications
    {
        public const string Default = GroupName + ".Applications";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Approve = Default + ".Approve";
        public const string Reject = Default + ".Reject";
    }

    public static class Bills
    {
        public const string Default = GroupName + ".Bills";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
    }

    public static class Payments
    {
        public const string Default = GroupName + ".Payments";
        public const string Create = Default + ".Create";
        public const string Process = Default + ".Process";
        public const string Refund = Default + ".Refund";
        public const string View = Default + ".View";
    }

    public static class Notifications
    {
        public const string Default = GroupName + ".Notifications";
        public const string Create = Default + ".Create";
        public const string Send = Default + ".Send";
        public const string Delete = Default + ".Delete";
    }

    public static class Tickets
    {
        public const string Default = GroupName + ".Tickets";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Assign = Default + ".Assign";
        public const string Close = Default + ".Close";
    }

    public static class Documents
    {
        public const string Default = GroupName + ".Documents";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Download = Default + ".Download";
        public const string Sign = Default + ".Sign";
        public const string Share = Default + ".Share";
    }

    public static class Properties
    {
        public const string Default = GroupName + ".Properties";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string ManageOwnership = Default + ".ManageOwnership";
        public const string ManageValuation = Default + ".ManageValuation";
    }

    public static class Appointments
    {
        public const string Default = GroupName + ".Appointments";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Approve = Default + ".Approve";
        public const string ManageSlots = Default + ".ManageSlots";
    }

    public static class Certificates
    {
        public const string Default = GroupName + ".Certificates";
        public const string Request = Default + ".Request";
        public const string Issue = Default + ".Issue";
        public const string Revoke = Default + ".Revoke";
        public const string Download = Default + ".Download";
    }

    public static class InfrastructureProjects
    {
        public const string Default = GroupName + ".InfrastructureProjects";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string AddUpdate = Default + ".AddUpdate";
        public const string ManageDocuments = Default + ".ManageDocuments";
    }

    public static class Surveys
    {
        public const string Default = GroupName + ".Surveys";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Respond = Default + ".Respond";
        public const string ViewResults = Default + ".ViewResults";
    }

    public static class Consultations
    {
        public const string Default = GroupName + ".Consultations";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Participate = Default + ".Participate";
        public const string ViewFeedback = Default + ".ViewFeedback";
    }

    public static class Votes
    {
        public const string Default = GroupName + ".Votes";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Vote = Default + ".Vote";
        public const string ViewResults = Default + ".ViewResults";
    }

    public static class Forums
    {
        public const string Default = GroupName + ".Forums";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Post = Default + ".Post";
        public const string Moderate = Default + ".Moderate";
    }

    public static class EmergencyAlerts
    {
        public const string Default = GroupName + ".EmergencyAlerts";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Broadcast = Default + ".Broadcast";
        public const string ManageRoutes = Default + ".ManageRoutes";
    }

    public static class ServiceRequests
    {
        public const string Default = GroupName + ".ServiceRequests";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Assign = Default + ".Assign";
        public const string Complete = Default + ".Complete";
        public const string Rate = Default + ".Rate";
    }
}
