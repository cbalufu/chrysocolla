using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Document;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Documents.Default)]
public class DocumentAppService : CrudAppService<Document, DocumentDto, Guid, DocumentDto, CreateUpdateDocumentDto>, IDocumentAppService
{
    private readonly IRepository<Document, Guid> _documentRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public DocumentAppService(
        IRepository<Document, Guid> repository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _documentRepository = repository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Documents.Default;
        GetListPolicyName = CitizensPortalPermissions.Documents.Default;
        CreatePolicyName = CitizensPortalPermissions.Documents.Create;
        UpdatePolicyName = CitizensPortalPermissions.Documents.Edit;
        DeletePolicyName = CitizensPortalPermissions.Documents.Delete;
    }

    [Authorize]
    public async Task<List<DocumentDto>> GetMyDocumentsAsync()
    {
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null)
        {
            return new List<DocumentDto>();
        }

        var documents = await _documentRepository.GetListAsync();
        var myDocuments = documents.Where(d => d.CitizenId == citizen.Id)
                                   .OrderByDescending(d => d.UploadDate)
                                   .ToList();

        return ObjectMapper.Map<List<Document>, List<DocumentDto>>(myDocuments);
    }

    [Authorize]
    public async Task<List<DocumentDto>> GetByCategoryAsync(DocumentCategory category)
    {
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null)
        {
            return new List<DocumentDto>();
        }

        var documents = await _documentRepository.GetListAsync();
        var categoryDocuments = documents.Where(d => d.CitizenId == citizen.Id && d.Category == category)
                                        .OrderByDescending(d => d.UploadDate)
                                        .ToList();

        return ObjectMapper.Map<List<Document>, List<DocumentDto>>(categoryDocuments);
    }

    [Authorize]
    public async Task<List<DocumentDto>> GetExpiringDocumentsAsync(int daysAhead)
    {
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null)
        {
            return new List<DocumentDto>();
        }

        var cutoffDate = DateTime.UtcNow.AddDays(daysAhead);
        var documents = await _documentRepository.GetListAsync();
        var expiringDocuments = documents.Where(d => d.CitizenId == citizen.Id &&
                                                    d.ExpiryDate.HasValue &&
                                                    d.ExpiryDate.Value <= cutoffDate &&
                                                    d.ExpiryDate.Value >= DateTime.UtcNow)
                                        .OrderBy(d => d.ExpiryDate)
                                        .ToList();

        return ObjectMapper.Map<List<Document>, List<DocumentDto>>(expiringDocuments);
    }

    [Authorize(CitizensPortalPermissions.Documents.Sign)]
    public async Task<DocumentDto> SignDocumentAsync(Guid id)
    {
        var document = await _documentRepository.GetAsync(id);

        // Verify ownership
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null || document.CitizenId != citizen.Id)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to sign this document");
        }

        if (document.Status != DocumentStatus.Active)
        {
            throw new Volo.Abp.BusinessException("DOCUMENT_NOT_ACTIVE")
                .WithData("message", "Only active documents can be signed");
        }

        if (document.IsSigned)
        {
            throw new Volo.Abp.BusinessException("DOCUMENT_ALREADY_SIGNED")
                .WithData("message", "Document is already signed");
        }

        // In a real system, implement digital signature here
        document.IsSigned = true;
        document.SignedDate = DateTime.UtcNow;
        document.SignedBy = $"{citizen.FirstName} {citizen.LastName}";
        document.DigitalSignature = $"SIG-{Guid.NewGuid().ToString().ToUpper()}"; // Placeholder

        await _documentRepository.UpdateAsync(document);

        return ObjectMapper.Map<Document, DocumentDto>(document);
    }

    [Authorize(CitizensPortalPermissions.Documents.Download)]
    public async Task DownloadDocumentAsync(Guid id)
    {
        var document = await _documentRepository.GetAsync(id);

        // Verify ownership or authorized access
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);

        // Check if user has access to this document
        bool hasAccess = false;
        if (citizen != null && document.CitizenId == citizen.Id)
        {
            hasAccess = true;
        }
        else if (document.AccessLevel == DocumentAccessLevel.Public)
        {
            hasAccess = true;
        }
        // In a real system, check DocumentAccess table for shared documents

        if (!hasAccess)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to download this document");
        }

        // Log the access
        var access = new DocumentAccess(
            _guidGenerator.Create(),
            id,
            citizen?.Id,
            DateTime.UtcNow,
            "Download"
        );

        // In a real system, add to DocumentAccess repository
        // For now, just mark as accessed in document
        await _documentRepository.UpdateAsync(document);
    }
}
