using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Ticket;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Tickets.Default)]
public class TicketAppService : CrudAppService<Ticket, TicketDto, Guid, TicketDto, CreateUpdateTicketDto>, ITicketAppService
{
    private readonly IRepository<Ticket, Guid> _ticketRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public TicketAppService(
        IRepository<Ticket, Guid> repository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _ticketRepository = repository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Tickets.Default;
        GetListPolicyName = CitizensPortalPermissions.Tickets.Default;
        CreatePolicyName = CitizensPortalPermissions.Tickets.Create;
        UpdatePolicyName = CitizensPortalPermissions.Tickets.Edit;
        DeletePolicyName = CitizensPortalPermissions.Tickets.Delete;
    }

    [Authorize]
    public async Task<List<TicketDto>> GetMyTicketsAsync()
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
            return new List<TicketDto>();
        }

        var tickets = await _ticketRepository.GetListAsync();
        var myTickets = tickets.Where(t => t.CitizenId == citizen.Id)
                              .OrderByDescending(t => t.CreationTime)
                              .ToList();

        return ObjectMapper.Map<List<Ticket>, List<TicketDto>>(myTickets);
    }

    [Authorize(CitizensPortalPermissions.Tickets.Edit)]
    public async Task AddMessageAsync(Guid id, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new Volo.Abp.BusinessException("MESSAGE_REQUIRED")
                .WithData("message", "Message text is required");
        }

        var ticket = await _ticketRepository.GetAsync(id);

        if (ticket.Status == TicketStatus.Closed)
        {
            throw new Volo.Abp.BusinessException("TICKET_CLOSED")
                .WithData("message", "Cannot add messages to closed tickets");
        }

        // Verify the ticket belongs to the current user
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        bool isFromStaff = false;

        // If not from citizen, must be from staff
        if (citizen == null || ticket.CitizenId != citizen.Id)
        {
            isFromStaff = true;
        }

        var ticketMessage = new TicketMessage(
            _guidGenerator.Create(),
            id,
            message,
            isFromStaff
        );

        ticket.Messages.Add(ticketMessage);

        // Update ticket status if it was pending
        if (ticket.Status == TicketStatus.Pending)
        {
            ticket.Status = TicketStatus.InProgress;
        }

        await _ticketRepository.UpdateAsync(ticket);
    }

    [Authorize(CitizensPortalPermissions.Tickets.Close)]
    public async Task CloseTicketAsync(Guid id)
    {
        var ticket = await _ticketRepository.GetAsync(id);

        if (ticket.Status == TicketStatus.Closed)
        {
            throw new Volo.Abp.BusinessException("TICKET_ALREADY_CLOSED")
                .WithData("message", "Ticket is already closed");
        }

        ticket.Status = TicketStatus.Closed;
        await _ticketRepository.UpdateAsync(ticket);
    }

    public override async Task<TicketDto> CreateAsync(CreateUpdateTicketDto input)
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

        // Create ticket
        var ticket = new Ticket(
            _guidGenerator.Create(),
            citizen.Id,
            input.Subject,
            input.Description,
            input.Category,
            input.Priority
        );

        ticket.Status = TicketStatus.Pending;
        ticket.TicketNumber = $"TKT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

        var createdTicket = await _ticketRepository.InsertAsync(ticket);

        return ObjectMapper.Map<Ticket, TicketDto>(createdTicket);
    }
}
