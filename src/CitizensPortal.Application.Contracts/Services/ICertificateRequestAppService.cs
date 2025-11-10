using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Certificate;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface ICertificateRequestAppService : ICrudAppService<CertificateRequestDto, Guid, CertificateRequestDto, CreateUpdateCertificateRequestDto>
    {
        Task<List<CertificateRequestDto>> GetMyRequestsAsync();
        Task SubmitRequestAsync(Guid id);
        Task<CertificateDto> GetCertificateAsync(Guid requestId);
        Task<bool> VerifyCertificateAsync(string certificateNumber, string verificationCode);
    }
}
