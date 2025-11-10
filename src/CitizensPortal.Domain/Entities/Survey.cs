using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a public survey/poll
    /// </summary>
    public class Survey : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public SurveyStatus Status { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsAnonymous { get; set; }
        public bool AllowMultipleResponses { get; set; }

        public int TotalResponses { get; set; }

        public ICollection<SurveyQuestion> Questions { get; set; }
        public ICollection<SurveyResponse> Responses { get; set; }

        protected Survey()
        {
            Questions = new List<SurveyQuestion>();
            Responses = new List<SurveyResponse>();
        }

        public Survey(Guid id, string title, DateTime startDate, DateTime endDate, Guid? tenantId = null) : base(id)
        {
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Status = SurveyStatus.Draft;
            TenantId = tenantId;
            Questions = new List<SurveyQuestion>();
            Responses = new List<SurveyResponse>();
        }
    }

    public class SurveyQuestion : CreationAuditedEntity<Guid>
    {
        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; }

        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public int OrderIndex { get; set; }
        public bool IsRequired { get; set; }

        // Options for choice questions (JSON array)
        public string Options { get; set; }

        public ICollection<SurveyAnswer> Answers { get; set; }

        protected SurveyQuestion()
        {
            Answers = new List<SurveyAnswer>();
        }
    }

    public class SurveyResponse : CreationAuditedEntity<Guid>
    {
        public Guid SurveyId { get; set; }
        public Survey Survey { get; set; }

        public Guid? CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public DateTime SubmittedDate { get; set; }

        public ICollection<SurveyAnswer> Answers { get; set; }

        protected SurveyResponse()
        {
            Answers = new List<SurveyAnswer>();
        }
    }

    public class SurveyAnswer : CreationAuditedEntity<Guid>
    {
        public Guid SurveyResponseId { get; set; }
        public SurveyResponse SurveyResponse { get; set; }

        public Guid SurveyQuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }

        public string AnswerText { get; set; }

        protected SurveyAnswer() { }
    }
}
