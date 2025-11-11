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
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Forums.Default)]
public class ForumAppService : CrudAppService<ForumPost, ForumPostDto, Guid>, IForumAppService
{
    private readonly IRepository<ForumTopic, Guid> _topicRepository;
    private readonly IRepository<ForumPost, Guid> _postRepository;
    private readonly IRepository<ForumReply, Guid> _replyRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public ForumAppService(
        IRepository<ForumPost, Guid> repository,
        IRepository<ForumTopic, Guid> topicRepository,
        IRepository<ForumReply, Guid> replyRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _postRepository = repository;
        _topicRepository = topicRepository;
        _replyRepository = replyRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Forums.Default;
        GetListPolicyName = CitizensPortalPermissions.Forums.Default;
        CreatePolicyName = CitizensPortalPermissions.Forums.Post;
        UpdatePolicyName = CitizensPortalPermissions.Forums.Edit;
        DeletePolicyName = CitizensPortalPermissions.Forums.Delete;
    }

    [AllowAnonymous]
    public async Task<List<ForumPostDto>> GetPostsByTopicAsync(Guid topicId)
    {
        var topic = await _topicRepository.GetAsync(topicId);

        var posts = await _postRepository.GetListAsync();
        var topicPosts = posts.Where(p => p.TopicId == topicId)
                             .OrderByDescending(p => p.IsPinned)
                             .ThenByDescending(p => p.PostDate)
                             .ToList();

        return ObjectMapper.Map<List<ForumPost>, List<ForumPostDto>>(topicPosts);
    }

    [Authorize(CitizensPortalPermissions.Forums.Post)]
    public async Task LikePostAsync(Guid postId)
    {
        var post = await _postRepository.GetAsync(postId);

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

        // In a real system, track who liked what to prevent duplicates
        // For now, just increment the like count
        post.LikeCount++;
        await _postRepository.UpdateAsync(post);
    }

    [Authorize(CitizensPortalPermissions.Forums.Post)]
    public async Task AddReplyAsync(Guid postId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new Volo.Abp.BusinessException("CONTENT_REQUIRED")
                .WithData("message", "Reply content is required");
        }

        var post = await _postRepository.GetAsync(postId);

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

        // Create reply
        var reply = new ForumReply(
            _guidGenerator.Create(),
            postId,
            citizen.Id,
            content,
            DateTime.UtcNow
        );

        await _replyRepository.InsertAsync(reply);

        // Update post reply count
        post.ReplyCount++;
        await _postRepository.UpdateAsync(post);
    }

    public override async Task<ForumPostDto> CreateAsync(ForumPostDto input)
    {
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

        // Verify topic exists
        await _topicRepository.GetAsync(input.TopicId);

        // Create forum post
        var post = new ForumPost(
            _guidGenerator.Create(),
            input.TopicId,
            citizen.Id,
            input.Title,
            input.Content,
            DateTime.UtcNow
        );

        var createdPost = await _postRepository.InsertAsync(post);

        return ObjectMapper.Map<ForumPost, ForumPostDto>(createdPost);
    }
}
