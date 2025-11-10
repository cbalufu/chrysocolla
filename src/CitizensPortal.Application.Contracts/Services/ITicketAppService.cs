using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Ticket;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface ITicketAppService : ICrudAppService<TicketDto, Guid, TicketDto, CreateUpdateTicketDto>
    {
        Task<List<TicketDto>> GetMyTicketsAsync();
        Task AddMessageAsync(Guid id, string message);
        Task CloseTicketAsync(Guid id);
    }
}
