using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Payment;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Payments.Default)]
public class PaymentAppService : ApplicationService, IPaymentAppService
{
    private readonly IRepository<Payment, Guid> _paymentRepository;
    private readonly IRepository<Bill, Guid> _billRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public PaymentAppService(
        IRepository<Payment, Guid> paymentRepository,
        IRepository<Bill, Guid> billRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator)
    {
        _paymentRepository = paymentRepository;
        _billRepository = billRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(CitizensPortalPermissions.Payments.View)]
    public async Task<PaymentDto> GetAsync(Guid id)
    {
        var payment = await _paymentRepository.GetAsync(id);
        return ObjectMapper.Map<Payment, PaymentDto>(payment);
    }

    [Authorize(CitizensPortalPermissions.Payments.Create)]
    public async Task<PaymentDto> CreateAsync(CreatePaymentDto input)
    {
        // Get current citizen
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null)
        {
            throw new Volo.Abp.BusinessException("CITIZEN_NOT_FOUND")
                .WithData("message", "Citizen record not found");
        }

        // Create payment
        var payment = new Payment(
            _guidGenerator.Create(),
            citizen.Id,
            input.Amount,
            input.PaymentMethod,
            input.PaymentGateway,
            $"PAY-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
        );

        payment.Status = PaymentStatus.Pending;
        payment.PaymentDate = DateTime.UtcNow;

        // Allocate payment to bills if bill IDs provided
        if (input.BillIds != null && input.BillIds.Any())
        {
            var bills = await _billRepository.GetListAsync();
            var billsToAllocate = bills.Where(b => input.BillIds.Contains(b.Id)).ToList();

            decimal remainingAmount = input.Amount;

            foreach (var bill in billsToAllocate.OrderBy(b => b.DueDate))
            {
                if (remainingAmount <= 0) break;

                var billBalance = bill.AmountDue - bill.AmountPaid;
                var allocationAmount = Math.Min(remainingAmount, billBalance);

                payment.Allocations.Add(new PaymentAllocation(
                    _guidGenerator.Create(),
                    payment.Id,
                    bill.Id,
                    allocationAmount
                ));

                // Update bill
                bill.AmountPaid += allocationAmount;
                if (bill.AmountPaid >= bill.AmountDue)
                {
                    bill.Status = BillStatus.Paid;
                    bill.PaidDate = DateTime.UtcNow;
                }
                else if (bill.AmountPaid > 0)
                {
                    bill.Status = BillStatus.PartiallyPaid;
                }

                await _billRepository.UpdateAsync(bill);

                remainingAmount -= allocationAmount;
            }
        }

        // In a real system, integrate with payment gateway here
        // For now, mark as pending
        payment.Status = PaymentStatus.Pending;

        var createdPayment = await _paymentRepository.InsertAsync(payment);

        return ObjectMapper.Map<Payment, PaymentDto>(createdPayment);
    }

    [Authorize]
    public async Task<List<PaymentDto>> GetMyPaymentsAsync()
    {
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null)
        {
            return new List<PaymentDto>();
        }

        var payments = await _paymentRepository.GetListAsync();
        var myPayments = payments.Where(p => p.CitizenId == citizen.Id)
                                 .OrderByDescending(p => p.PaymentDate)
                                 .ToList();

        return ObjectMapper.Map<List<Payment>, List<PaymentDto>>(myPayments);
    }

    [Authorize(CitizensPortalPermissions.Payments.View)]
    public async Task<PaymentDto> GetByPaymentNumberAsync(string paymentNumber)
    {
        var payments = await _paymentRepository.GetListAsync();
        var payment = payments.FirstOrDefault(p => p.PaymentNumber == paymentNumber);

        if (payment == null)
        {
            throw new Volo.Abp.BusinessException("PAYMENT_NOT_FOUND")
                .WithData("PaymentNumber", paymentNumber);
        }

        return ObjectMapper.Map<Payment, PaymentDto>(payment);
    }

    [Authorize(CitizensPortalPermissions.Payments.View)]
    public async Task<byte[]> GetReceiptPdfAsync(Guid id)
    {
        var payment = await _paymentRepository.GetAsync(id);

        if (payment.Status != PaymentStatus.Completed)
        {
            throw new Volo.Abp.BusinessException("PAYMENT_NOT_COMPLETED")
                .WithData("message", "Receipt can only be generated for completed payments");
        }

        // In a real system, generate PDF receipt here
        // For now, return placeholder
        var receiptText = $"Payment Receipt\n" +
                         $"Payment Number: {payment.PaymentNumber}\n" +
                         $"Amount: ${payment.Amount:N2}\n" +
                         $"Date: {payment.PaymentDate}\n" +
                         $"Method: {payment.PaymentMethod}\n" +
                         $"Status: {payment.Status}";

        return System.Text.Encoding.UTF8.GetBytes(receiptText);
    }
}
