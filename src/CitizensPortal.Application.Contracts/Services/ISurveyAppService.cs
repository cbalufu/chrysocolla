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
}
