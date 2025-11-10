using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Community;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface ISurveyAppService : ICrudAppService<SurveyDto, Guid, SurveyDto, CreateUpdateSurveyDto>
    {
        Task<List<SurveyDto>> GetActiveSurveysAsync();
        Task SubmitResponseAsync(Guid surveyId, Dictionary<Guid, string> answers);
    }

    public interface IConsultationAppService : ICrudAppService<ConsultationDto, Guid>
    {
        Task<List<ConsultationDto>> GetActiveConsultationsAsync();
        Task AddCommentAsync(Guid id, string comment);
    }

    public interface IVoteAppService : ICrudAppService<VoteDto, Guid>
    {
        Task<List<VoteDto>> GetActiveVotesAsync();
        Task CastVoteAsync(Guid voteId, Guid optionId);
    }

    public interface IForumAppService : ICrudAppService<ForumPostDto, Guid>
    {
        Task<List<ForumPostDto>> GetPostsByTopicAsync(Guid topicId);
        Task LikePostAsync(Guid postId);
        Task AddReplyAsync(Guid postId, string content);
    }
}
