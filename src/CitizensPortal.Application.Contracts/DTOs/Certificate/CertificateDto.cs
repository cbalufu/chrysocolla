using System;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Certificate
{
    public class CertificateRequestDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public string RequestNumber { get; set; }
        public CertificateType CertificateType { get; set; }
        public CertificateRequestStatus Status { get; set; }
        public string Purpose { get; set; }
        public string SubjectFullName { get; set; }
        public string SubjectNationalId { get; set; }
        public DateTime? SubjectDateOfBirth { get; set; }
        public bool CollectInPerson { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? ReadyDate { get; set; }
        public decimal Fee { get; set; }
        public bool IsPaid { get; set; }
    }

    public class CreateUpdateCertificateRequestDto
    {
        public CertificateType CertificateType { get; set; }
        public string Purpose { get; set; }
        public string RequestDetails { get; set; }
        public string AdditionalInfo { get; set; }
        public string SubjectFullName { get; set; }
        public string SubjectNationalId { get; set; }
        public DateTime? SubjectDateOfBirth { get; set; }
        public bool CollectInPerson { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryContactNumber { get; set; }
    }

    public class CertificateDto : FullAuditedEntityDto<Guid>
    {
        public string CertificateNumber { get; set; }
        public CertificateType Type { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CertificateUrl { get; set; }
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }
        public string IssuedByName { get; set; }
    }
}
