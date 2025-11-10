using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Bill;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface IBillAppService : IApplicationService
    {
        Task<BillDto> GetAsync(Guid id);
        Task<PagedResultDto<BillDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        Task<List<BillDto>> GetMyBillsAsync();
        Task<List<BillDto>> GetUnpaidBillsAsync();
        Task<decimal> GetTotalBalanceAsync();
    }
}
