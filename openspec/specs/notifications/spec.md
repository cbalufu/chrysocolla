# Notifications

## Overview
The Notifications capability provides email notification system with MailKit integration for sending HTML emails about bills, applications, issues, tickets, and general announcements.

## Requirements

### Requirement: Send Email Notifications
The system SHALL send HTML email notifications with priority-based styling to citizens about system events.

#### Scenario: Send bill notification
- **WHEN** a new bill is generated for a citizen
- **THEN** the system sends email notification with bill details
- **AND** includes amount due, due date, and payment link
- **AND** applies priority-based color styling

#### Scenario: Send application status update
- **WHEN** application status changes
- **THEN** the system sends notification to applicant
- **AND** includes application number and new status

### Requirement: Notification Types
The system SHALL support notification types: Bill, Application, Issue, Ticket, Announcement, EmergencyAlert.

#### Scenario: Send emergency notification
- **WHEN** emergency alert is created
- **THEN** the system sends notification type "EmergencyAlert"
- **AND** uses high priority styling
- **AND** includes alert details and instructions

### Requirement: Notification Priority
The system SHALL support priority levels (Low, Medium, High, Urgent) with color-coded HTML templates.

#### Scenario: Urgent notification styling
- **WHEN** sending urgent priority notification
- **THEN** the system uses red color scheme in HTML template
- **AND** marks email as high priority

### Requirement: Async Email Sending
The system SHALL send emails asynchronously without blocking main operations.

#### Scenario: Non-blocking email send
- **WHEN** sending notification email
- **THEN** the system queues email for async sending
- **AND** continues processing without waiting for email completion
- **AND** logs any send failures without interrupting service

### Requirement: Notification History
The system SHALL maintain history of all sent notifications with delivery status.

#### Scenario: Track sent notification
- **WHEN** notification is sent
- **THEN** the system records notification record with sent timestamp
- **AND** tracks IsRead status for in-app notifications

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all notification records.

#### Scenario: Tenant isolation
- **WHEN** querying notifications from Tenant A
- **THEN** the system returns only notifications from Tenant A

## Data Model

### Notification Entity
- **Id**: Guid
- **TenantId**: Guid?
- **CitizenId**: Guid (Recipient)
- **Type**: NotificationType enum
- **Priority**: NotificationPriority enum
- **Subject**: string
- **Message**: string
- **IsRead**: bool (Default: false)
- **SentDate**: DateTime
- Audit fields

### NotificationType Enum
- Bill, Application, Issue, Ticket, Announcement, EmergencyAlert

### NotificationPriority Enum
- Low, Medium, High, Urgent

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/NotificationAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Notification.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/NotificationEnums.cs`
- Uses MailKit + ABP Emailing module
- HTTP API: No controller yet (needs to be created)
- HTML email templates with priority-based colors
