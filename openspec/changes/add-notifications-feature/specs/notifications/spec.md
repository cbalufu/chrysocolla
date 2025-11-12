## MODIFIED Requirements

### Requirement: Send Email Notifications
The system SHALL send HTML email notifications with priority-based styling to citizens about system events using SMTP.

#### Scenario: Send bill notification
- **WHEN** notification is triggered for new bill
- **THEN** the system sends email notification with bill details
- **AND** includes amount due, due date, and payment reference
- **AND** applies priority-based color styling in HTML template
- **AND** records notification in database

#### Scenario: Send application status update
- **WHEN** application status changes to Approved or Rejected
- **THEN** the system sends notification to applicant
- **AND** includes application number and new status
- **AND** records notification with type "Application"

### Requirement: Async Email Sending
The system SHALL send emails asynchronously without blocking main operations.

#### Scenario: Non-blocking email send
- **WHEN** notification is created
- **THEN** the system records notification immediately
- **AND** queues email for async sending via background service
- **AND** continues processing without waiting for email delivery
- **AND** logs any send failures for retry

## ADDED Requirements

### Requirement: Notification History Retrieval
The system SHALL allow citizens to view their notification history with filtering.

#### Scenario: View my notifications
- **WHEN** citizen requests notification history
- **THEN** the system returns only notifications belonging to that citizen
- **AND** allows filtering by IsRead status
- **AND** allows filtering by notification type
- **AND** orders by SentDate descending
- **AND** includes pagination

#### Scenario: View unread notifications count
- **WHEN** citizen requests notifications
- **THEN** the system includes count of unread notifications
- **AND** highlights urgent priority notifications

### Requirement: Mark Notifications as Read
The system SHALL allow citizens to mark notifications as read.

#### Scenario: Mark notification as read
- **WHEN** citizen marks notification as read
- **THEN** the system verifies ownership
- **AND** sets IsRead to true
- **AND** records ReadDate timestamp
- **AND** returns updated notification

#### Scenario: Mark all as read
- **WHEN** citizen requests to mark all notifications as read
- **THEN** the system updates all unread notifications for that citizen
- **AND** sets IsRead to true for all
- **AND** returns count of updated notifications

### Requirement: Admin Custom Notifications
The system SHALL allow admin/staff to send custom notifications to citizens.

#### Scenario: Send announcement to all citizens
- **WHEN** admin sends announcement notification
- **THEN** the system creates notification for each citizen in tenant
- **AND** sends email to all recipients
- **AND** uses "Announcement" notification type
- **AND** requires StaffOrAdmin role

#### Scenario: Send notification to specific citizen
- **WHEN** admin sends notification to specific citizen ID
- **THEN** the system creates notification for that citizen only
- **AND** sends email to recipient
- **AND** validates citizen belongs to same tenant

## MODIFIED Data Model

### Notification Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy, indexed)
- **CitizenId**: Guid (Foreign Key to Citizen, indexed)
- **Citizen**: Navigation property to Citizen
- **Type**: string (Required, max 50 chars: Bill, Application, Issue, Ticket, Announcement, EmergencyAlert)
- **Priority**: string (Required, max 20 chars: Low, Medium, High, Urgent)
- **Subject**: string (Required, max 200 chars)
- **Message**: string (Required, max 2000 chars)
- **IsRead**: bool (Default false, indexed)
- **SentDate**: DateTime (UTC)
- **ReadDate**: DateTime? (UTC, nullable)

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/Notifications/`):
- **GetUserNotifications**: GET /api/notifications/my
  - Filter by IsRead and Type
  - Paginated results
  - Files: GetUserNotificationsQuery.cs, GetUserNotificationsQueryHandler.cs, GetUserNotificationsResponse.cs, GetUserNotifications.cs
- **MarkAsRead**: PUT /api/notifications/{id}/read
  - Files: MarkAsReadCommand.cs, MarkAsReadCommandHandler.cs, MarkAsReadResponse.cs, MarkAsRead.cs
- **MarkAllAsRead**: PUT /api/notifications/read-all
  - Files: MarkAllAsReadCommand.cs, MarkAllAsReadCommandHandler.cs, MarkAllAsReadResponse.cs, MarkAllAsRead.cs

**Admin Features** (located in `src/CitizensPortal.Api/Features/Admin/Notifications/`):
- **SendCustomNotification**: POST /api/admin/notifications (requires StaffOrAdmin role)
  - Files: SendCustomNotificationCommand.cs, SendCustomNotificationCommandValidator.cs, SendCustomNotificationCommandHandler.cs, SendCustomNotificationResponse.cs, SendCustomNotification.cs

**Infrastructure** (located in `src/CitizensPortal.Api/Infrastructure/`):
- Entity: `Database/Entities/Notification.cs`
- EF Configuration: `Database/Configurations/NotificationConfiguration.cs`
- Email Service: `Email/IEmailService.cs`, `Email/EmailService.cs`
- SMTP configuration in appsettings.json

**Patterns Used**:
- CQRS via MediatR
- ErrorOr for functional error handling
- FluentValidation for request validation
- Carter for Minimal API endpoint organization
- Background email sending (async)
- HTML email templates
