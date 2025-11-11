using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Bill;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Bills.Default)]
public class BillAppService : ApplicationService, IBillAppService
{
    private readonly IRepository<Bill, Guid> _billRepository;
    private readonly ICitizenRepository _citizenRepository;

    public BillAppService(
        IRepository<Bill, Guid> billRepository,
        ICitizenRepository citizenRepository)
    {
        _billRepository = billRepository;
        _citizenRepository = citizenRepository;
    }

    [Authorize(CitizensPortalPermissions.Bills.View)]
    public async Task<BillDto> GetAsync(Guid id)
    {
        var bill = await _billRepository.GetAsync(id);
        return ObjectMapper.Map<Bill, BillDto>(bill);
    }

    [Authorize(CitizensPortalPermissions.Bills.View)]
    public async Task<PagedResultDto<BillDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var totalCount = await _billRepository.GetCountAsync();
        var bills = await _billRepository.GetPagedListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting ?? "BillDate DESC"
        );

        return new PagedResultDto<BillDto>(
            totalCount,
            ObjectMapper.Map<List<Bill>, List<BillDto>>(bills)
        );
    }

    [Authorize]
    public async Task<List<BillDto>> GetMyBillsAsync()
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
            return new List<BillDto>();
        }

        var bills = await _billRepository.GetListAsync();
        var myBills = bills.Where(b => b.CitizenId == citizen.Id)
                          .OrderByDescending(b => b.BillDate)
                          .ToList();

        return ObjectMapper.Map<List<Bill>, List<BillDto>>(myBills);
    }

    [Authorize]
    public async Task<List<BillDto>> GetUnpaidBillsAsync()
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
            return new List<BillDto>();
        }

        var bills = await _billRepository.GetListAsync();
        var unpaidBills = bills.Where(b => b.CitizenId == citizen.Id && b.Status == BillStatus.Unpaid)
                               .OrderBy(b => b.DueDate)
                               .ToList();

        return ObjectMapper.Map<List<Bill>, List<BillDto>>(unpaidBills);
    }

    [Authorize]
    public async Task<decimal> GetTotalBalanceAsync()
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
            return 0;
        }

        var bills = await _billRepository.GetListAsync();
        var totalBalance = bills.Where(b => b.CitizenId == citizen.Id && b.Status != BillStatus.Paid)
                                .Sum(b => b.AmountDue - b.AmountPaid);

        return totalBalance;
    }
}
