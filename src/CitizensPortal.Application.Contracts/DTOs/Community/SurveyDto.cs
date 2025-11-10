using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Community
{
    public class SurveyDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SurveyStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAnonymous { get; set; }
        public int TotalResponses { get; set; }
        public List<SurveyQuestionDto> Questions { get; set; }
    }

    public class SurveyQuestionDto : EntityDto<Guid>
    {
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public int OrderIndex { get; set; }
        public bool IsRequired { get; set; }
        public string Options { get; set; }
    }

    public class CreateUpdateSurveyDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAnonymous { get; set; }
    }

    public class ConsultationDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DetailedInformation { get; set; }
        public ConsultationStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
    }

    public class VoteDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public VoteType Type { get; set; }
        public VoteStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalVotes { get; set; }
        public List<VoteOptionDto> Options { get; set; }
    }

    public class VoteOptionDto : EntityDto<Guid>
    {
        public string OptionText { get; set; }
        public int VoteCount { get; set; }
    }

    public class ForumPostDto : FullAuditedEntityDto<Guid>
    {
        public Guid ForumTopicId { get; set; }
        public string TopicName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ForumPostStatus Status { get; set; }
        public int ViewCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
    }
}
