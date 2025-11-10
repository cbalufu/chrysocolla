using System;
using Volo.Abp.Application.Dtos;
using CitizensPortal.Domain.Enums;

namespace CitizensPortal.Application.Contracts.DTOs.Bill
{
    public class BillDto : FullAuditedEntityDto<Guid>
    {
        public Guid CitizenId { get; set; }
        public string CitizenName { get; set; }
        public string BillNumber { get; set; }
        public BillType Type { get; set; }
        public BillStatus Status { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Description { get; set; }
        public string Period { get; set; }
        public string PropertyReference { get; set; }
    }
}
