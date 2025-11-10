using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a payment receipt/proof of payment
    /// </summary>
    public class PaymentReceipt : CreationAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }

        public string ReceiptNumber { get; set; }
        public string ReceiptUrl { get; set; }
        public byte[] ReceiptPdf { get; set; }

        public DateTime IssueDate { get; set; }

        protected PaymentReceipt() { }

        public PaymentReceipt(Guid id, Guid paymentId, string receiptNumber, Guid? tenantId = null)
        {
            Id = id;
            PaymentId = paymentId;
            ReceiptNumber = receiptNumber;
            IssueDate = DateTime.UtcNow;
            TenantId = tenantId;
        }
    }
}
