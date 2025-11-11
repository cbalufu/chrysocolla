using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Community;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IForumAppService : ICrudAppService<ForumPostDto, Guid>
    {
        Task<List<ForumPostDto>> GetPostsByTopicAsync(Guid topicId);
        Task LikePostAsync(Guid postId);
        Task AddReplyAsync(Guid postId, string content);
    }
}
