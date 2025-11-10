using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a request for an official certificate/record
    /// </summary>
    public class CertificateRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string RequestNumber { get; set; }
        public CertificateType CertificateType { get; set; }
        public CertificateRequestStatus Status { get; set; }

        public string Purpose { get; set; }
        public string RequestDetails { get; set; } // JSON data specific to certificate type

        // Subject of the certificate (might be different from requester)
        public string SubjectFullName { get; set; }
        public string SubjectNationalId { get; set; }
        public DateTime? SubjectDateOfBirth { get; set; }

        // Delivery
        public bool CollectInPerson { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryContactNumber { get; set; }

        // Processing
        public DateTime? ProcessedDate { get; set; }
        public Guid? ProcessedByUserId { get; set; }
        public DateTime? ReadyDate { get; set; }
        public DateTime? CollectedDate { get; set; }

        // Payment
        public decimal Fee { get; set; }
        public bool IsPaid { get; set; }
        public Guid? PaymentId { get; set; }

        // Supporting documents
        public ICollection<CertificateRequestDocument> SupportingDocuments { get; set; }

        // Generated certificate
        public Guid? GeneratedCertificateId { get; set; }
        public Certificate GeneratedCertificate { get; set; }

        protected CertificateRequest()
        {
            SupportingDocuments = new List<CertificateRequestDocument>();
        }

        public CertificateRequest(
            Guid id,
            Guid citizenId,
            CertificateType certificateType,
            string purpose,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            CertificateType = certificateType;
            Purpose = purpose;
            Status = CertificateRequestStatus.Draft;
            TenantId = tenantId;
            RequestNumber = GenerateRequestNumber();
            SupportingDocuments = new List<CertificateRequestDocument>();
        }

        private string GenerateRequestNumber()
        {
            return $"CERT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public void Submit()
        {
            if (Status == CertificateRequestStatus.Draft)
            {
                Status = CertificateRequestStatus.Submitted;
            }
        }
    }

    /// <summary>
    /// Supporting documents for certificate request
    /// </summary>
    public class CertificateRequestDocument : CreationAuditedEntity<Guid>
    {
        public Guid CertificateRequestId { get; set; }
        public CertificateRequest CertificateRequest { get; set; }

        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        protected CertificateRequestDocument() { }

        public CertificateRequestDocument(Guid id, Guid certificateRequestId, string documentType, string fileName, string fileUrl)
        {
            Id = id;
            CertificateRequestId = certificateRequestId;
            DocumentType = documentType;
            FileName = fileName;
            FileUrl = fileUrl;
        }
    }

    /// <summary>
    /// The actual generated/issued certificate
    /// </summary>
    public class Certificate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CertificateRequestId { get; set; }
        public CertificateRequest CertificateRequest { get; set; }

        public string CertificateNumber { get; set; }
        public CertificateType Type { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string CertificateData { get; set; } // JSON data
        public string CertificateUrl { get; set; } // PDF/document URL

        // Digital signature/verification
        public string DigitalSignature { get; set; }
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }

        // Issued by
        public Guid IssuedByUserId { get; set; }
        public string IssuedByName { get; set; }

        protected Certificate() { }

        public Certificate(
            Guid id,
            Guid certificateRequestId,
            CertificateType type,
            Guid issuedByUserId,
            Guid? tenantId = null) : base(id)
        {
            CertificateRequestId = certificateRequestId;
            Type = type;
            IssuedByUserId = issuedByUserId;
            IssueDate = DateTime.UtcNow;
            TenantId = tenantId;
            CertificateNumber = GenerateCertificateNumber();
            VerificationCode = GenerateVerificationCode();
            IsVerified = true;
        }

        private string GenerateCertificateNumber()
        {
            return $"CERT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString().Substring(0, 10).ToUpper()}";
        }

        private string GenerateVerificationCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();
        }
    }
}
