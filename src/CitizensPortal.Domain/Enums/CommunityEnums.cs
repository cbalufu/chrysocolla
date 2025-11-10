namespace CitizensPortal.Domain.Enums
{
    public enum SurveyStatus
    {
        Draft,
        Active,
        Closed,
        Archived
    }

    public enum QuestionType
    {
        SingleChoice,
        MultipleChoice,
        Text,
        Rating,
        YesNo
    }

    public enum ConsultationStatus
    {
        Upcoming,
        Open,
        Closed,
        Cancelled
    }

    public enum VoteType
    {
        Initiative,
        Budget,
        ProjectPriority,
        PolicyChange,
        Other
    }

    public enum VoteStatus
    {
        Upcoming,
        Active,
        Closed,
        Cancelled
    }

    public enum ForumPostStatus
    {
        Active,
        Flagged,
        Hidden,
        Deleted
    }
}
