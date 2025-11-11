using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Payment
{
    public class PaymentDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public string PaymentNumber { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentGateway? Gateway { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceiptUrl { get; set; }
        public List<PaymentAllocationDto> Allocations { get; set; }
    }

    public class PaymentAllocationDto : EntityDto<Guid>
    {
        public Guid BillId { get; set; }
        public string BillNumber { get; set; }
        public decimal AllocatedAmount { get; set; }
    }

    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentGateway? Gateway { get; set; }

        // Aliases for backward compatibility with services
        public PaymentMethod PaymentMethod { get => Method; set => Method = value; }
        public PaymentGateway? PaymentGateway { get => Gateway; set => Gateway = value; }

        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerPhone { get; set; }
        public List<CreatePaymentAllocationDto> Allocations { get; set; }

        // Alias for BillIds (computed from Allocations)
        public List<Guid> BillIds
        {
            get => Allocations?.Select(a => a.BillId).ToList();
            set
            {
                Allocations = value?.Select(id => new CreatePaymentAllocationDto { BillId = id }).ToList();
            }
        }
    }

    public class CreatePaymentAllocationDto
    {
        public Guid BillId { get; set; }
        public decimal AllocatedAmount { get; set; }
    }
}
