using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Appointment;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Appointments.Default)]
public class AppointmentAppService : CrudAppService<Appointment, AppointmentDto, Guid, AppointmentDto, CreateUpdateAppointmentDto>, IAppointmentAppService
{
    private readonly IRepository<Appointment, Guid> _appointmentRepository;
    private readonly IRepository<Department, Guid> _departmentRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IGuidGenerator _guidGenerator;

    public AppointmentAppService(
        IRepository<Appointment, Guid> repository,
        IRepository<Department, Guid> departmentRepository,
        ICitizenRepository citizenRepository,
        IGuidGenerator guidGenerator) : base(repository)
    {
        _appointmentRepository = repository;
        _departmentRepository = departmentRepository;
        _citizenRepository = citizenRepository;
        _guidGenerator = guidGenerator;

        GetPolicyName = CitizensPortalPermissions.Appointments.Default;
        GetListPolicyName = CitizensPortalPermissions.Appointments.Default;
        CreatePolicyName = CitizensPortalPermissions.Appointments.Create;
        UpdatePolicyName = CitizensPortalPermissions.Appointments.Edit;
        DeletePolicyName = CitizensPortalPermissions.Appointments.Delete;
    }

    [Authorize]
    public async Task<List<AppointmentDto>> GetMyAppointmentsAsync()
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
            return new List<AppointmentDto>();
        }

        var appointments = await _appointmentRepository.GetListAsync();
        var myAppointments = appointments.Where(a => a.CitizenId == citizen.Id)
                                        .OrderByDescending(a => a.AppointmentDate)
                                        .ToList();

        return ObjectMapper.Map<List<Appointment>, List<AppointmentDto>>(myAppointments);
    }

    [Authorize]
    public async Task<List<AppointmentDto>> GetUpcomingAppointmentsAsync()
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
            return new List<AppointmentDto>();
        }

        var appointments = await _appointmentRepository.GetListAsync();
        var upcomingAppointments = appointments.Where(a => a.CitizenId == citizen.Id &&
                                                          a.AppointmentDate >= DateTime.UtcNow &&
                                                          a.Status != AppointmentStatus.Cancelled &&
                                                          a.Status != AppointmentStatus.Completed)
                                                .OrderBy(a => a.AppointmentDate)
                                                .ToList();

        return ObjectMapper.Map<List<Appointment>, List<AppointmentDto>>(upcomingAppointments);
    }

    [Authorize(CitizensPortalPermissions.Appointments.Approve)]
    public async Task ConfirmAppointmentAsync(Guid id)
    {
        var appointment = await _appointmentRepository.GetAsync(id);

        if (appointment.Status != AppointmentStatus.Requested)
        {
            throw new Volo.Abp.BusinessException("APPOINTMENT_CANNOT_BE_CONFIRMED")
                .WithData("message", $"Appointment with status {appointment.Status} cannot be confirmed");
        }

        if (appointment.AppointmentDate <= DateTime.UtcNow)
        {
            throw new Volo.Abp.BusinessException("APPOINTMENT_DATE_PASSED")
                .WithData("message", "Cannot confirm past appointments");
        }

        appointment.Status = AppointmentStatus.Confirmed;
        await _appointmentRepository.UpdateAsync(appointment);
    }

    [Authorize(CitizensPortalPermissions.Appointments.Edit)]
    public async Task CancelAppointmentAsync(Guid id, string reason)
    {
        var appointment = await _appointmentRepository.GetAsync(id);

        // Verify ownership
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null || appointment.CitizenId != citizen.Id)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to cancel this appointment");
        }

        if (appointment.Status == AppointmentStatus.Completed)
        {
            throw new Volo.Abp.BusinessException("APPOINTMENT_ALREADY_COMPLETED")
                .WithData("message", "Cannot cancel completed appointments");
        }

        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new Volo.Abp.BusinessException("APPOINTMENT_ALREADY_CANCELLED")
                .WithData("message", "Appointment is already cancelled");
        }

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.Notes = $"Cancelled: {reason}";
        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task<List<DepartmentDto>> GetDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetListAsync();
        return ObjectMapper.Map<List<Department>, List<DepartmentDto>>(departments.ToList());
    }

    public override async Task<AppointmentDto> CreateAsync(CreateUpdateAppointmentDto input)
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

        // Create appointment
        var appointment = new Appointment(
            _guidGenerator.Create(),
            citizen.Id,
            input.DepartmentId,
            input.AppointmentDate,
            input.Purpose
        );

        appointment.Status = AppointmentStatus.Requested;
        appointment.Notes = input.Notes;

        var createdAppointment = await _appointmentRepository.InsertAsync(appointment);

        return ObjectMapper.Map<Appointment, AppointmentDto>(createdAppointment);
    }
}
