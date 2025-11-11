using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a payment made by a citizen
    /// </summary>
    public class Payment : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string PaymentNumber { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentGateway? Gateway { get; set; }

        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string GatewayResponse { get; set; }

        // What bills are being paid
        public ICollection<PaymentAllocation> Allocations { get; set; }

        // Payment details
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerPhone { get; set; }

        // Receipt information
        public string ReceiptNumber { get; set; }
        public string ReceiptUrl { get; set; }

        protected Payment()
        {
            Allocations = new List<PaymentAllocation>();
        }

        public Payment(
            Guid id,
            Guid citizenId,
            decimal amount,
            PaymentMethod method,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Amount = amount;
            Method = method;
            Status = PaymentStatus.Pending;
            PaymentDate = DateTime.UtcNow;
            TenantId = tenantId;
            PaymentNumber = GeneratePaymentNumber();
            Allocations = new List<PaymentAllocation>();
        }

        private string GeneratePaymentNumber()
        {
            return $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public void MarkAsCompleted(string transactionId, string receiptNumber)
        {
            Status = PaymentStatus.Completed;
            TransactionId = transactionId;
            ReceiptNumber = receiptNumber;
        }

        public void MarkAsFailed(string reason)
        {
            Status = PaymentStatus.Failed;
            GatewayResponse = reason;
        }
    }

    /// <summary>
    /// Maps payment to specific bills
    /// </summary>
    public class PaymentAllocation : CreationAuditedEntity<Guid>
    {
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }

        public Guid BillId { get; set; }
        public Bill Bill { get; set; }

        public decimal AllocatedAmount { get; set; }

        protected PaymentAllocation() { }

        public PaymentAllocation(Guid id, Guid paymentId, Guid billId, decimal allocatedAmount)
        {
            Id = id;
            PaymentId = paymentId;
            BillId = billId;
            AllocatedAmount = allocatedAmount;
        }
    }
}
