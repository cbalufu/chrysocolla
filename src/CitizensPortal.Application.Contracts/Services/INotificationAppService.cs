using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using CitizensPortal.Application.Contracts.DTOs.Notification;

namespace CitizensPortal.Application.Contracts.Services
{
    public interface INotificationAppService : IApplicationService
    {
        Task<List<NotificationDto>> GetMyNotificationsAsync();
        Task<List<NotificationDto>> GetUnreadNotificationsAsync();
        Task<int> GetUnreadCountAsync();
        Task MarkAsReadAsync(Guid id);
        Task MarkAllAsReadAsync();
    }
}
