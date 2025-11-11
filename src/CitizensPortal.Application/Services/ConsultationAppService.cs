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

[Authorize(CitizensPortalPermissions.Consultations.Default)]
public class ConsultationAppService : CrudAppService<Consultation, ConsultationDto, Guid>, IConsultationAppService
{
    private readonly IRepository<Consultation, Guid> _consultationRepository;
    private readonly IRepository<ConsultationComment, Guid> _commentRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public ConsultationAppService(
        IRepository<Consultation, Guid> repository,
        IRepository<ConsultationComment, Guid> commentRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _consultationRepository = repository;
        _commentRepository = commentRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Consultations.Default;
        GetListPolicyName = CitizensPortalPermissions.Consultations.Default;
        CreatePolicyName = CitizensPortalPermissions.Consultations.Create;
        UpdatePolicyName = CitizensPortalPermissions.Consultations.Edit;
        DeletePolicyName = CitizensPortalPermissions.Consultations.Delete;
    }

    [AllowAnonymous]
    public async Task<List<ConsultationDto>> GetActiveConsultationsAsync()
    {
        var consultations = await _consultationRepository.GetListAsync();
        var activeConsultations = consultations.Where(c => c.Status == ConsultationStatus.Open &&
                                                          c.StartDate <= DateTime.UtcNow &&
                                                          c.EndDate >= DateTime.UtcNow)
                                               .OrderByDescending(c => c.StartDate)
                                               .ToList();

        return ObjectMapper.Map<List<Consultation>, List<ConsultationDto>>(activeConsultations);
    }

    [Authorize(CitizensPortalPermissions.Consultations.Participate)]
    public async Task AddCommentAsync(Guid id, string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new Volo.Abp.BusinessException("COMMENT_REQUIRED")
                .WithData("message", "Comment text is required");
        }

        var consultation = await _consultationRepository.GetAsync(id);

        if (consultation.Status != ConsultationStatus.Open)
        {
            throw new Volo.Abp.BusinessException("CONSULTATION_NOT_ACTIVE")
                .WithData("message", "Consultation is not active");
        }

        if (consultation.EndDate < DateTime.UtcNow)
        {
            throw new Volo.Abp.BusinessException("CONSULTATION_ENDED")
                .WithData("message", "Consultation has ended");
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

        // Create comment
        var consultationComment = new ConsultationComment(
            _guidGenerator.Create(),
            id,
            citizen.Id,
            comment,
            true // isPublic
        );

        await _commentRepository.InsertAsync(consultationComment);
    }
}
