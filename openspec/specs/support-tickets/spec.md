# Support Tickets

## Overview
The Support Tickets capability provides a help desk system for citizens to get assistance with technical, billing, application, or general inquiries through threaded conversations.

## Requirements

### Requirement: Ticket Creation
The system SHALL allow citizens to create support tickets with subject, category, and description.

#### Scenario: Create technical support ticket
- **WHEN** citizen creates ticket for technical issue
- **THEN** the system creates ticket with status "Open" and priority "Medium"
- **AND** generates unique ticket reference number
- **AND** links to citizen
- **AND** returns ticket details

### Requirement: Ticket Categories
The system SHALL support ticket categories: Technical, Billing, Application, General.

#### Scenario: Categorize as billing inquiry
- **WHEN** creating ticket about bill question
- **THEN** the system sets category to "Billing"
- **AND** routes to billing support team

### Requirement: Ticket Priority
The system SHALL support priority levels: Low, Medium, High, Urgent.

#### Scenario: Mark ticket as urgent
- **WHEN** staff escalates ticket to urgent
- **THEN** the system updates priority to "Urgent"
- **AND** prioritizes in support queue

### Requirement: Ticket Status Workflow
The system SHALL track tickets through statuses: Open, InProgress, Pending, Resolved, Closed.

#### Scenario: Progress ticket to resolution
- **WHEN** ticket is created (status: Open)
- **THEN** staff takes it (status: "InProgress")
- **AND** may put on hold (status: "Pending")
- **AND** resolves issue (status: "Resolved")
- **AND** closes ticket (status: "Closed")

### Requirement: Threaded Messages
The system SHALL support threaded message conversations between citizens and staff.

#### Scenario: Add message to ticket
- **WHEN** citizen or staff adds message to ticket
- **THEN** the system creates `TicketMessage` record
- **AND** links to ticket
- **AND** timestamps message
- **AND** notifies other party

### Requirement: Ticket Attachments
The system SHALL allow file attachments to tickets for screenshots, documents, etc.

#### Scenario: Attach screenshot
- **WHEN** citizen attaches screenshot to ticket
- **THEN** the system stores file in blob storage
- **AND** creates `TicketAttachment` record
- **AND** links to ticket

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying tickets from Tenant A
- **THEN** the system returns only tickets from Tenant A

## Data Model

### Ticket Entity
- **Id**: Guid
- **TenantId**: Guid?
- **CitizenId**: Guid (FK)
- **Subject**: string
- **Category**: TicketCategory enum
- **Priority**: TicketPriority enum
- **Status**: TicketStatus enum
- **ReferenceNumber**: string
- **AssignedToUserId**: Guid?
- **ResolutionNotes**: string
- **Messages**: ICollection<TicketMessage>
- **Attachments**: ICollection<TicketAttachment>
- Audit fields

### TicketCategory Enum
- Technical, Billing, Application, General

### TicketPriority Enum
- Low, Medium, High, Urgent

### TicketStatus Enum
- Open, InProgress, Pending, Resolved, Closed

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/TicketAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Ticket.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/TicketEnums.cs`
- HTTP API: No controller yet (needs to be created)
