using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using CitizensPortal.Application.Contracts.DTOs.Payment;
using CitizensPortal.Application.Contracts.Services;

namespace CitizensPortal.HttpApi.Controllers
{
    [Route("api/payments")]
    public class PaymentController : AbpControllerBase
    {
        private readonly IPaymentAppService _paymentAppService;

        public PaymentController(IPaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }

        [HttpGet("{id}")]
        public Task<PaymentDto> GetAsync(Guid id)
        {
            return _paymentAppService.GetAsync(id);
        }

        [HttpGet("my-payments")]
        public Task<List<PaymentDto>> GetMyPaymentsAsync()
        {
            return _paymentAppService.GetMyPaymentsAsync();
        }

        [HttpGet("by-number/{paymentNumber}")]
        public Task<PaymentDto> GetByPaymentNumberAsync(string paymentNumber)
        {
            return _paymentAppService.GetByPaymentNumberAsync(paymentNumber);
        }

        [HttpPost]
        public Task<PaymentDto> CreateAsync(CreatePaymentDto input)
        {
            return _paymentAppService.CreateAsync(input);
        }

        [HttpGet("{id}/receipt")]
        public Task<byte[]> GetReceiptPdfAsync(Guid id)
        {
            return _paymentAppService.GetReceiptPdfAsync(id);
        }
    }
}
