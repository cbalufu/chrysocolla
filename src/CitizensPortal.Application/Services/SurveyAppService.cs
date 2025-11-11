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
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Surveys.Default)]
public class SurveyAppService : CrudAppService<Survey, SurveyDto, Guid, SurveyDto, CreateUpdateSurveyDto>, ISurveyAppService
{
    private readonly IRepository<Survey, Guid> _surveyRepository;
    private readonly IRepository<SurveyResponse, Guid> _responseRepository;
    private readonly IRepository<SurveyAnswer, Guid> _answerRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public SurveyAppService(
        IRepository<Survey, Guid> repository,
        IRepository<SurveyResponse, Guid> responseRepository,
        IRepository<SurveyAnswer, Guid> answerRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _surveyRepository = repository;
        _responseRepository = responseRepository;
        _answerRepository = answerRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Surveys.Default;
        GetListPolicyName = CitizensPortalPermissions.Surveys.Default;
        CreatePolicyName = CitizensPortalPermissions.Surveys.Create;
        UpdatePolicyName = CitizensPortalPermissions.Surveys.Edit;
        DeletePolicyName = CitizensPortalPermissions.Surveys.Delete;
    }

    [AllowAnonymous]
    public async Task<List<SurveyDto>> GetActiveSurveysAsync()
    {
        var surveys = await _surveyRepository.GetListAsync();
        var activeSurveys = surveys.Where(s => s.Status == SurveyStatus.Active &&
                                              s.StartDate <= DateTime.UtcNow &&
                                              s.EndDate >= DateTime.UtcNow)
                                  .OrderByDescending(s => s.StartDate)
                                  .ToList();

        return ObjectMapper.Map<List<Survey>, List<SurveyDto>>(activeSurveys);
    }

    [Authorize(CitizensPortalPermissions.Surveys.Respond)]
    public async Task SubmitResponseAsync(Guid surveyId, Dictionary<Guid, string> answers)
    {
        var survey = await _surveyRepository.GetAsync(surveyId);

        if (survey.Status != SurveyStatus.Active)
        {
            throw new Volo.Abp.BusinessException("SURVEY_NOT_ACTIVE")
                .WithData("message", "Survey is not active");
        }

        if (survey.EndDate < DateTime.UtcNow)
        {
            throw new Volo.Abp.BusinessException("SURVEY_ENDED")
                .WithData("message", "Survey has ended");
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

        // Check if already responded
        var existingResponses = await _responseRepository.GetListAsync();
        var hasResponded = existingResponses.Any(r => r.SurveyId == surveyId && r.CitizenId == citizen.Id);

        if (hasResponded && !survey.AllowMultipleResponses)
        {
            throw new Volo.Abp.BusinessException("ALREADY_RESPONDED")
                .WithData("message", "You have already responded to this survey");
        }

        // Create response
        var response = new SurveyResponse(
            _guidGenerator.Create(),
            surveyId,
            citizen.Id,
            DateTime.UtcNow
        );

        await _responseRepository.InsertAsync(response);

        // Create answers
        foreach (var answer in answers)
        {
            var surveyAnswer = new SurveyAnswer(
                _guidGenerator.Create(),
                response.Id,
                answer.Key, // questionId
                answer.Value // answerText
            );

            await _answerRepository.InsertAsync(surveyAnswer);
        }
    }
}
