using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a forum topic/category
    /// </summary>
    public class ForumTopic : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int PostCount { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ForumPost> Posts { get; set; }

        protected ForumTopic()
        {
            Posts = new List<ForumPost>();
        }

        public ForumTopic(Guid id, string name, Guid? tenantId = null) : base(id)
        {
            Name = name;
            IsActive = true;
            TenantId = tenantId;
            Posts = new List<ForumPost>();
        }
    }

    /// <summary>
    /// Represents a forum post
    /// </summary>
    public class ForumPost : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid ForumTopicId { get; set; }
        public ForumTopic ForumTopic { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public ForumPostStatus Status { get; set; }

        public int ViewCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }

        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }

        public ICollection<ForumReply> Replies { get; set; }

        protected ForumPost()
        {
            Replies = new List<ForumReply>();
        }

        public ForumPost(Guid id, Guid topicId, Guid citizenId, string title, string content, Guid? tenantId = null) : base(id)
        {
            ForumTopicId = topicId;
            CitizenId = citizenId;
            Title = title;
            Content = content;
            Status = ForumPostStatus.Active;
            TenantId = tenantId;
            Replies = new List<ForumReply>();
        }
    }

    public class ForumReply : CreationAuditedEntity<Guid>
    {
        public Guid ForumPostId { get; set; }
        public ForumPost ForumPost { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string Content { get; set; }
        public int LikeCount { get; set; }
        public ForumPostStatus Status { get; set; }

        protected ForumReply() { }

        public ForumReply(Guid id, Guid forumPostId, Guid citizenId, string content, ForumPostStatus status = ForumPostStatus.Active) : base(id)
        {
            ForumPostId = forumPostId;
            CitizenId = citizenId;
            Content = content;
            Status = status;
        }
    }
}
