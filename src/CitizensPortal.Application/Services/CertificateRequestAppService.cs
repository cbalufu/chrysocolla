using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Certificate;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Certificates.Default)]
public class CertificateRequestAppService : CrudAppService<CertificateRequest, CertificateRequestDto, Guid, CertificateRequestDto, CreateUpdateCertificateRequestDto>, ICertificateRequestAppService
{
    private readonly IRepository<CertificateRequest, Guid> _requestRepository;
    private readonly IRepository<Certificate, Guid> _certificateRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public CertificateRequestAppService(
        IRepository<CertificateRequest, Guid> repository,
        IRepository<Certificate, Guid> certificateRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _requestRepository = repository;
        _certificateRepository = certificateRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Certificates.Default;
        GetListPolicyName = CitizensPortalPermissions.Certificates.Default;
        CreatePolicyName = CitizensPortalPermissions.Certificates.Request;
        UpdatePolicyName = CitizensPortalPermissions.Certificates.Request;
        DeletePolicyName = CitizensPortalPermissions.Certificates.Request;
    }

    [Authorize]
    public async Task<List<CertificateRequestDto>> GetMyRequestsAsync()
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
            return new List<CertificateRequestDto>();
        }

        var requests = await _requestRepository.GetListAsync();
        var myRequests = requests.Where(r => r.CitizenId == citizen.Id)
                                .OrderByDescending(r => r.RequestDate)
                                .ToList();

        return ObjectMapper.Map<List<CertificateRequest>, List<CertificateRequestDto>>(myRequests);
    }

    [Authorize(CitizensPortalPermissions.Certificates.Request)]
    public async Task SubmitRequestAsync(Guid id)
    {
        var request = await _requestRepository.GetAsync(id);

        // Verify ownership
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null || request.CitizenId != citizen.Id)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to submit this request");
        }

        if (request.Status != CertificateRequestStatus.Draft)
        {
            throw new Volo.Abp.BusinessException("REQUEST_ALREADY_SUBMITTED")
                .WithData("message", "Request has already been submitted");
        }

        request.Status = CertificateRequestStatus.Submitted;
        request.RequestDate = DateTime.UtcNow;

        await _requestRepository.UpdateAsync(request);
    }

    [Authorize(CitizensPortalPermissions.Certificates.Download)]
    public async Task<CertificateDto> GetCertificateAsync(Guid requestId)
    {
        var request = await _requestRepository.GetAsync(requestId);

        // Verify ownership
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null || request.CitizenId != citizen.Id)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to access this certificate");
        }

        if (request.Status != CertificateRequestStatus.Issued)
        {
            throw new Volo.Abp.BusinessException("CERTIFICATE_NOT_ISSUED")
                .WithData("message", "Certificate has not been issued yet");
        }

        // Get the certificate
        var certificates = await _certificateRepository.GetListAsync();
        var certificate = certificates.FirstOrDefault(c => c.RequestId == requestId);

        if (certificate == null)
        {
            throw new Volo.Abp.BusinessException("CERTIFICATE_NOT_FOUND")
                .WithData("message", "Certificate not found");
        }

        return ObjectMapper.Map<Certificate, CertificateDto>(certificate);
    }

    [AllowAnonymous]
    public async Task<bool> VerifyCertificateAsync(string certificateNumber, string verificationCode)
    {
        if (string.IsNullOrWhiteSpace(certificateNumber))
        {
            throw new Volo.Abp.BusinessException("CERTIFICATE_NUMBER_REQUIRED")
                .WithData("message", "Certificate number is required");
        }

        if (string.IsNullOrWhiteSpace(verificationCode))
        {
            throw new Volo.Abp.BusinessException("VERIFICATION_CODE_REQUIRED")
                .WithData("message", "Verification code is required");
        }

        var certificates = await _certificateRepository.GetListAsync();
        var certificate = certificates.FirstOrDefault(c =>
            c.CertificateNumber == certificateNumber &&
            c.VerificationCode == verificationCode);

        if (certificate == null)
        {
            return false;
        }

        // Check if certificate is valid and not expired
        if (certificate.Status != CertificateRequestStatus.Issued)
        {
            return false;
        }

        if (certificate.ExpiryDate.HasValue && certificate.ExpiryDate.Value < DateTime.UtcNow)
        {
            return false;
        }

        return true;
    }

    public override async Task<CertificateRequestDto> CreateAsync(CreateUpdateCertificateRequestDto input)
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

        // Create certificate request
        var request = new CertificateRequest(
            _guidGenerator.Create(),
            citizen.Id,
            input.CertificateType,
            input.Purpose
        );

        request.Status = CertificateRequestStatus.Draft;
        request.AdditionalInfo = input.AdditionalInfo;

        var createdRequest = await _requestRepository.InsertAsync(request);

        return ObjectMapper.Map<CertificateRequest, CertificateRequestDto>(createdRequest);
    }
}
