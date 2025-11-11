using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Domain.Entities
{
    /// <summary>
    /// Represents a bill/statement for a citizen
    /// </summary>
    public class Bill : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        public string BillNumber { get; set; }
        public BillType Type { get; set; }
        public BillStatus Status { get; set; }

        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountDue => Balance; // Alias for Balance

        public DateTime IssueDate { get; set; }
        public DateTime BillDate { get; set; } // Alias for IssueDate, used by some services
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }

        public string Description { get; set; }
        public string Period { get; set; } // e.g., "Q1 2024" or "January 2024"

        // Property reference for rates/utilities
        public string PropertyReference { get; set; }

        protected Bill()
        {
        }

        public Bill(
            Guid id,
            Guid citizenId,
            BillType type,
            decimal amount,
            DateTime dueDate,
            string period,
            Guid? tenantId = null) : base(id)
        {
            CitizenId = citizenId;
            Type = type;
            Amount = amount;
            Balance = amount;
            AmountPaid = 0;
            IssueDate = DateTime.UtcNow;
            BillDate = DateTime.UtcNow;
            DueDate = dueDate;
            Period = period;
            Status = BillStatus.Unpaid;
            TenantId = tenantId;
            BillNumber = GenerateBillNumber(type);
        }

        private string GenerateBillNumber(BillType type)
        {
            var prefix = type switch
            {
                BillType.PropertyRates => "RATE",
                BillType.Water => "WAT",
                BillType.Electricity => "ELEC",
                BillType.Waste => "WST",
                BillType.Other => "BILL"
            };
            return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public void RecordPayment(decimal paymentAmount)
        {
            AmountPaid += paymentAmount;
            Balance = Amount - AmountPaid;

            if (Balance <= 0)
            {
                Status = BillStatus.Paid;
                PaidDate = DateTime.UtcNow;
            }
            else if (AmountPaid > 0)
            {
                Status = BillStatus.PartiallyPaid;
            }
        }
    }
}
