using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Document;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Documents.Default)]
public class DocumentAppService : CrudAppService<Document, DocumentDto, Guid, DocumentDto, CreateUpdateDocumentDto>, IDocumentAppService
{
    private readonly IRepository<Document, Guid> _documentRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IBlobContainer _blobContainer;

    public DocumentAppService(
        IRepository<Document, Guid> repository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator,
        IBlobContainer blobContainer) : base(repository)
    {
        _documentRepository = repository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;
        _blobContainer = blobContainer;

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
        var signatureData = $"SIG-{Guid.NewGuid().ToString().ToUpper()}"; // Placeholder
        document.Sign(citizen.Id, signatureData);

        await _documentRepository.UpdateAsync(document);

        return ObjectMapper.Map<Document, DocumentDto>(document);
    }

    /// <summary>
    /// Uploads a document file to blob storage and creates a document record
    /// </summary>
    [Authorize(CitizensPortalPermissions.Documents.Create)]
    public async Task<DocumentDto> UploadDocumentAsync(
        string title,
        string description,
        DocumentCategory category,
        IRemoteStreamContent fileContent)
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
                .WithData("message", "Citizen not found");
        }

        // Generate blob name
        var blobName = $"{Guid.NewGuid()}{Path.GetExtension(fileContent.FileName)}";

        // Read file stream and calculate hash
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await fileContent.GetStream().CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }

        string fileHash = CalculateMD5Hash(fileBytes);

        // Save to blob storage
        await _blobContainer.SaveAsync(blobName, fileBytes, true);

        // Create document record
        var document = new Document(
            _guidGenerator.Create(),
            citizen.Id,
            title,
            category,
            fileContent.FileName,
            blobName, // Store blob name in FileUrl
            CurrentTenant.Id);

        document.Description = description;
        document.ContentType = fileContent.ContentType;
        document.FileSize = fileBytes.Length;
        document.FileHash = fileHash;
        document.Status = DocumentStatus.Active;
        document.AccessLevel = DocumentAccessLevel.Private;

        await _documentRepository.InsertAsync(document);

        Logger.LogInformation($"Document uploaded: {document.Id} - {fileContent.FileName} ({fileBytes.Length} bytes)");

        return ObjectMapper.Map<Document, DocumentDto>(document);
    }

    /// <summary>
    /// Downloads a document file from blob storage
    /// </summary>
    [Authorize(CitizensPortalPermissions.Documents.Download)]
    public async Task<IRemoteStreamContent> DownloadDocumentAsync(Guid id)
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

        if (!hasAccess)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to download this document");
        }

        // Get file from blob storage
        var fileBytes = await _blobContainer.GetAllBytesOrNullAsync(document.FileUrl);
        if (fileBytes == null)
        {
            throw new Volo.Abp.BusinessException("FILE_NOT_FOUND")
                .WithData("message", "Document file not found in storage");
        }

        // Log the access
        Logger.LogInformation($"Document downloaded: {document.Id} by user {currentUserEmail}");

        // Return file stream
        var stream = new MemoryStream(fileBytes);
        return new RemoteStreamContent(stream, document.FileName, document.ContentType);
    }

    /// <summary>
    /// Deletes a document and its file from blob storage
    /// </summary>
    public override async Task DeleteAsync(Guid id)
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
                "You are not authorized to delete this document");
        }

        // Delete from blob storage
        try
        {
            await _blobContainer.DeleteAsync(document.FileUrl);
            Logger.LogInformation($"Deleted blob: {document.FileUrl}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Failed to delete blob: {document.FileUrl}");
            // Continue with document deletion even if blob deletion fails
        }

        // Delete document record
        await base.DeleteAsync(id);
        Logger.LogInformation($"Document deleted: {id}");
    }

    /// <summary>
    /// Calculates MD5 hash for file integrity verification
    /// </summary>
    private string CalculateMD5Hash(byte[] fileBytes)
    {
        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(fileBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
