using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Payment;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<PaymentDto> GetAsync(Guid id);
        Task<PaymentDto> CreateAsync(CreatePaymentDto input);
        Task<List<PaymentDto>> GetMyPaymentsAsync();
        Task<PaymentDto> GetByPaymentNumberAsync(string paymentNumber);
        Task<byte[]> GetReceiptPdfAsync(Guid id);
    }
}
