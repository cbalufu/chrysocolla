using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Community;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Votes.Default)]
public class VoteAppService : CrudAppService<Vote, VoteDto, Guid>, IVoteAppService
{
    private readonly IRepository<Vote, Guid> _voteRepository;
    private readonly IRepository<VoteOption, Guid> _optionRepository;
    private readonly IRepository<CitizenVote, Guid> _citizenVoteRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public VoteAppService(
        IRepository<Vote, Guid> repository,
        IRepository<VoteOption, Guid> optionRepository,
        IRepository<CitizenVote, Guid> citizenVoteRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _voteRepository = repository;
        _optionRepository = optionRepository;
        _citizenVoteRepository = citizenVoteRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Votes.Default;
        GetListPolicyName = CitizensPortalPermissions.Votes.Default;
        CreatePolicyName = CitizensPortalPermissions.Votes.Create;
        UpdatePolicyName = CitizensPortalPermissions.Votes.Edit;
        DeletePolicyName = CitizensPortalPermissions.Votes.Delete;
    }

    [AllowAnonymous]
    public async Task<List<VoteDto>> GetActiveVotesAsync()
    {
        var votes = await _voteRepository.GetListAsync();
        var activeVotes = votes.Where(v => v.Status == VoteStatus.Active &&
                                          v.StartDate <= DateTime.UtcNow &&
                                          v.EndDate >= DateTime.UtcNow)
                               .OrderByDescending(v => v.StartDate)
                               .ToList();

        return ObjectMapper.Map<List<Vote>, List<VoteDto>>(activeVotes);
    }

    [Authorize(CitizensPortalPermissions.Votes.Vote)]
    public async Task CastVoteAsync(Guid voteId, Guid optionId)
    {
        var vote = await _voteRepository.GetAsync(voteId);

        if (vote.Status != VoteStatus.Active)
        {
            throw new Volo.Abp.BusinessException("VOTE_NOT_ACTIVE")
                .WithData("message", "Vote is not active");
        }

        if (vote.StartDate > DateTime.UtcNow)
        {
            throw new Volo.Abp.BusinessException("VOTE_NOT_STARTED")
                .WithData("message", "Voting has not started yet");
        }

        if (vote.EndDate < DateTime.UtcNow)
        {
            throw new Volo.Abp.BusinessException("VOTE_ENDED")
                .WithData("message", "Voting has ended");
        }

        // Verify option belongs to this vote
        var option = await _optionRepository.GetAsync(optionId);
        if (option.VoteId != voteId)
        {
            throw new Volo.Abp.BusinessException("INVALID_OPTION")
                .WithData("message", "Option does not belong to this vote");
        }

        // Get current citizen
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null)
        {
            throw new Volo.Abp.BusinessException("CITIZEN_NOT_FOUND")
                .WithData("message", "Citizen record not found");
        }

        // Check if already voted
        var existingVotes = await _citizenVoteRepository.GetListAsync();
        var hasVoted = existingVotes.Any(cv => cv.VoteId == voteId && cv.CitizenId == citizen.Id);

        if (hasVoted)
        {
            throw new Volo.Abp.BusinessException("ALREADY_VOTED")
                .WithData("message", "You have already voted");
        }

        // Cast vote
        var citizenVote = new CitizenVote(
            _guidGenerator.Create(),
            voteId,
            citizen.Id,
            optionId,
            DateTime.UtcNow
        );

        await _citizenVoteRepository.InsertAsync(citizenVote);

        // Update vote count
        option.VoteCount++;
        await _optionRepository.UpdateAsync(option);
    }
}
