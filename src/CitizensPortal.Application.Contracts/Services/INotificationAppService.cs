using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Notification;
using CitizensPortal.Domain.Shared.Enums;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface INotificationAppService : IApplicationService
    {
        Task<List<NotificationDto>> GetMyNotificationsAsync();
        Task<List<NotificationDto>> GetUnreadNotificationsAsync();
        Task<int> GetUnreadCountAsync();
        Task MarkAsReadAsync(Guid id);
        Task MarkAllAsReadAsync();
        Task<NotificationDto> CreateNotificationAsync(
            Guid citizenId,
            string title,
            string message,
            NotificationType type,
            NotificationPriority priority = NotificationPriority.Normal,
            bool sendEmail = true);
    }
}
