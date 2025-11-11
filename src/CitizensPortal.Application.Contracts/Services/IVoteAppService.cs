using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Community;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IVoteAppService : ICrudAppService<VoteDto, Guid>
    {
        Task<List<VoteDto>> GetActiveVotesAsync();
        Task CastVoteAsync(Guid voteId, Guid optionId);
    }
}
