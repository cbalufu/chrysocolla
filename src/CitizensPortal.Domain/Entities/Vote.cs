using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a community vote on an initiative
    /// </summary>
    public class Vote : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public VoteType Type { get; set; }
        public VoteStatus Status { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool RequiresVerification { get; set; }

        public int TotalVotes { get; set; }

        public ICollection<VoteOption> Options { get; set; }
        public ICollection<CitizenVote> CitizenVotes { get; set; }

        protected Vote()
        {
            Options = new List<VoteOption>();
            CitizenVotes = new List<CitizenVote>();
        }

        public Vote(Guid id, string title, VoteType type, DateTime startDate, DateTime endDate, Guid? tenantId = null) : base(id)
        {
            Title = title;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            Status = VoteStatus.Upcoming;
            TenantId = tenantId;
            Options = new List<VoteOption>();
            CitizenVotes = new List<CitizenVote>();
        }
    }

    public class VoteOption : CreationAuditedEntity<Guid>
    {
        public Guid VoteId { get; set; }
        public Vote Vote { get; set; }

        public string OptionText { get; set; }
        public int VoteCount { get; set; }

        protected VoteOption() { }
    }

    public class CitizenVote : CreationAuditedEntity<Guid>
    {
        public Guid VoteId { get; set; }
        public Vote Vote { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public Guid VoteOptionId { get; set; }
        public VoteOption VoteOption { get; set; }

        public DateTime VotedDate { get; set; }

        protected CitizenVote() { }

        public CitizenVote(Guid id, Guid voteId, Guid citizenId, Guid voteOptionId, DateTime votedDate) : base(id)
        {
            VoteId = voteId;
            CitizenId = citizenId;
            VoteOptionId = voteOptionId;
            VotedDate = votedDate;
        }
    }
}
