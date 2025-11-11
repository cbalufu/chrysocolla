using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Notification;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Notifications.Default)]
public class NotificationAppService : ApplicationService, INotificationAppService
{
    private readonly IRepository<Notification, Guid> _notificationRepository;
    private readonly ICitizenRepository _citizenRepository;

    public NotificationAppService(
        IRepository<Notification, Guid> notificationRepository,
        ICitizenRepository citizenRepository)
    {
        _notificationRepository = notificationRepository;
        _citizenRepository = citizenRepository;
    }

    [Authorize]
    public async Task<List<NotificationDto>> GetMyNotificationsAsync()
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
            return new List<NotificationDto>();
        }

        var notifications = await _notificationRepository.GetListAsync();
        var myNotifications = notifications.Where(n => n.CitizenId == citizen.Id)
                                          .OrderByDescending(n => n.CreationTime)
                                          .ToList();

        return ObjectMapper.Map<List<Notification>, List<NotificationDto>>(myNotifications);
    }

    [Authorize]
    public async Task<List<NotificationDto>> GetUnreadNotificationsAsync()
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
            return new List<NotificationDto>();
        }

        var notifications = await _notificationRepository.GetListAsync();
        var unreadNotifications = notifications.Where(n => n.CitizenId == citizen.Id && !n.IsRead)
                                              .OrderByDescending(n => n.CreationTime)
                                              .ToList();

        return ObjectMapper.Map<List<Notification>, List<NotificationDto>>(unreadNotifications);
    }

    [Authorize]
    public async Task<int> GetUnreadCountAsync()
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

        var notifications = await _notificationRepository.GetListAsync();
        return notifications.Count(n => n.CitizenId == citizen.Id && !n.IsRead);
    }

    [Authorize]
    public async Task MarkAsReadAsync(Guid id)
    {
        var notification = await _notificationRepository.GetAsync(id);

        // Verify the notification belongs to the current user
        var currentUserEmail = CurrentUser.Email;
        if (string.IsNullOrEmpty(currentUserEmail))
        {
            throw new Volo.Abp.BusinessException("USER_EMAIL_NOT_FOUND")
                .WithData("message", "Current user email not found");
        }

        var citizen = await _citizenRepository.FindByEmailAsync(currentUserEmail);
        if (citizen == null || notification.CitizenId != citizen.Id)
        {
            throw new Volo.Abp.Authorization.AbpAuthorizationException(
                "You are not authorized to access this notification");
        }

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            await _notificationRepository.UpdateAsync(notification);
        }
    }

    [Authorize]
    public async Task MarkAllAsReadAsync()
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
            return;
        }

        var notifications = await _notificationRepository.GetListAsync();
        var unreadNotifications = notifications.Where(n => n.CitizenId == citizen.Id && !n.IsRead).ToList();

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            await _notificationRepository.UpdateAsync(notification);
        }
    }
}
