using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Volo.Abp.Users;
using CitizensPortal.Application.Contracts.DTOs.Notification;
using CitizensPortal.Application.Contracts.Services;
using CitizensPortal.Domain.Entities;
using CitizensPortal.Domain.Repositories;
using CitizensPortal.Domain.Shared.Enums;
using CitizensPortal.Permissions;

namespace CitizensPortal.Application.Services;

[Authorize(CitizensPortalPermissions.Notifications.Default)]
public class NotificationAppService : ApplicationService, INotificationAppService
{
    private readonly IRepository<Notification, Guid> _notificationRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IEmailSender _emailSender;

    public NotificationAppService(
        IRepository<Notification, Guid> notificationRepository,
        ICitizenRepository citizenRepository,
        IEmailSender emailSender)
    {
        _notificationRepository = notificationRepository;
        _citizenRepository = citizenRepository;
        _emailSender = emailSender;
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

    /// <summary>
    /// Creates a new notification and optionally sends an email
    /// </summary>
    [Authorize(CitizensPortalPermissions.Notifications.Create)]
    public async Task<NotificationDto> CreateNotificationAsync(
        Guid citizenId,
        string title,
        string message,
        NotificationType type,
        NotificationPriority priority = NotificationPriority.Normal,
        bool sendEmail = true)
    {
        var citizen = await _citizenRepository.GetAsync(citizenId);

        var notification = new Notification(
            GuidGenerator.Create(),
            citizenId,
            title,
            message,
            type,
            CurrentTenant.Id);

        notification.Priority = priority;

        await _notificationRepository.InsertAsync(notification);

        // Send email if requested and citizen has an email
        if (sendEmail && !string.IsNullOrEmpty(citizen.Email))
        {
            await SendEmailNotificationAsync(citizen, notification);
        }

        return ObjectMapper.Map<Notification, NotificationDto>(notification);
    }

    /// <summary>
    /// Sends an email notification to a citizen
    /// </summary>
    private async Task SendEmailNotificationAsync(Citizen citizen, Notification notification)
    {
        try
        {
            var emailBody = BuildEmailBody(citizen, notification);
            var subject = $"[Citizens Portal] {notification.Title}";

            await _emailSender.SendAsync(
                citizen.Email,
                subject,
                emailBody,
                isBodyHtml: true);

            Logger.LogInformation(
                $"Email notification sent to {citizen.Email} for notification {notification.Id}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex,
                $"Failed to send email notification to {citizen.Email} for notification {notification.Id}");
            // Don't throw - we don't want email failures to break the notification system
        }
    }

    /// <summary>
    /// Builds the HTML email body for a notification
    /// </summary>
    private string BuildEmailBody(Citizen citizen, Notification notification)
    {
        var priorityColor = notification.Priority switch
        {
            NotificationPriority.Urgent => "#dc3545",
            NotificationPriority.High => "#fd7e14",
            NotificationPriority.Normal => "#0d6efd",
            NotificationPriority.Low => "#6c757d",
            _ => "#0d6efd"
        };

        var sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("    <style>");
        sb.AppendLine("        body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
        sb.AppendLine("        .container { max-width: 600px; margin: 0 auto; padding: 20px; }");
        sb.AppendLine("        .header { background-color: #007bff; color: white; padding: 20px; border-radius: 5px 5px 0 0; }");
        sb.AppendLine("        .content { background-color: #f8f9fa; padding: 20px; border: 1px solid #dee2e6; }");
        sb.AppendLine("        .priority { display: inline-block; padding: 5px 10px; border-radius: 3px; color: white; font-weight: bold; margin-bottom: 10px; }");
        sb.AppendLine("        .footer { background-color: #e9ecef; padding: 15px; text-align: center; font-size: 12px; color: #6c757d; border-radius: 0 0 5px 5px; }");
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("    <div class=\"container\">");
        sb.AppendLine("        <div class=\"header\">");
        sb.AppendLine("            <h2>Citizens Portal Notification</h2>");
        sb.AppendLine("        </div>");
        sb.AppendLine("        <div class=\"content\">");
        sb.AppendLine($"            <p>Dear {citizen.FirstName} {citizen.LastName},</p>");
        sb.AppendLine($"            <div class=\"priority\" style=\"background-color: {priorityColor};\">{notification.Priority}</div>");
        sb.AppendLine($"            <h3>{notification.Title}</h3>");
        sb.AppendLine($"            <p>{notification.Message}</p>");
        sb.AppendLine($"            <p><strong>Notification Type:</strong> {FormatNotificationType(notification.Type)}</p>");
        sb.AppendLine($"            <p><small>Sent on: {notification.CreationTime:yyyy-MM-dd HH:mm}</small></p>");
        sb.AppendLine("        </div>");
        sb.AppendLine("        <div class=\"footer\">");
        sb.AppendLine("            <p>This is an automated message from the Citizens Portal. Please do not reply to this email.</p>");
        sb.AppendLine("            <p>Log in to the Citizens Portal to view all your notifications and manage your account.</p>");
        sb.AppendLine("        </div>");
        sb.AppendLine("    </div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    /// <summary>
    /// Formats the notification type for display
    /// </summary>
    private string FormatNotificationType(NotificationType type)
    {
        return type switch
        {
            NotificationType.BillGenerated => "Bill Generated",
            NotificationType.BillDue => "Bill Due",
            NotificationType.BillOverdue => "Bill Overdue",
            NotificationType.PaymentReceived => "Payment Received",
            NotificationType.ApplicationStatusChanged => "Application Status Changed",
            NotificationType.ApplicationApproved => "Application Approved",
            NotificationType.ApplicationRejected => "Application Rejected",
            NotificationType.IssueStatusChanged => "Issue Status Changed",
            NotificationType.IssueResolved => "Issue Resolved",
            NotificationType.TicketResponse => "Ticket Response",
            NotificationType.GeneralAnnouncement => "General Announcement",
            NotificationType.SystemAlert => "System Alert",
            _ => type.ToString()
        };
    }
}
