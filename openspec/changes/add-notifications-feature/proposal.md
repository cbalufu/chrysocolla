# Change: Add Notifications Feature

## Why

Citizens need to be notified about important events such as new bills, application status changes, issue updates, ticket responses, and emergency alerts. The Notifications capability provides an email notification system with in-app notification tracking.

## What Changes

- Implement email notification system using SMTP
- Add notification history tracking (in-app notifications)
- Support notification types: Bill, Application, Issue, Ticket, Announcement, EmergencyAlert
- Support priority levels: Low, Medium, High, Urgent
- Async email sending to avoid blocking operations
- HTML email templates with priority-based styling
- Citizen endpoints to view notification history
- Admin endpoints to send custom notifications

**Features to implement:**
- Citizen: View notification history (GET /api/notifications/my)
- Citizen: Mark notification as read (PUT /api/notifications/{id}/read)
- Admin: Send custom notification (POST /api/admin/notifications)
- Background service: Email sending with retry logic

## Impact

- **Affected specs:** notifications (new implementation)
- **Affected code:** New feature slice in `src/CitizensPortal.Api/Features/Notifications/`
- **Dependencies:** Email service infrastructure (SMTP configuration)
- **Database changes:** Notification entity, NotificationConfiguration

This is a new feature implementation following VSA patterns established in previous phases.
