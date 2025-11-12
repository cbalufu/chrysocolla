## MODIFIED Requirements

### Requirement: Ticket Creation
The system SHALL allow citizens to create support tickets with subject, description, category, and priority.

#### Scenario: Create technical support ticket
- **WHEN** citizen creates ticket with category "Technical" and priority "Medium"
- **THEN** the system creates ticket with status "Open"
- **AND** generates unique ticket number in format TKT-{CATEGORY}-{YYYYMMDD}-{GUID}
- **AND** links to citizen
- **AND** validates category is valid (Technical, Account, Payment, General)
- **AND** validates priority is valid (Low, Medium, High)
- **AND** returns ticket details

### Requirement: Ticket Categories
The system SHALL support ticket categories: Technical, Account, Payment, General.

#### Scenario: Categorize as payment inquiry
- **WHEN** creating ticket about payment with category "Payment"
- **THEN** the system generates ticket number starting with "TKT-PAY-"
- **AND** saves category as "Payment"

### Requirement: Ticket Priority
The system SHALL support priority levels: Low, Medium, High.

#### Scenario: Set high priority ticket
- **WHEN** citizen creates ticket with priority "High"
- **THEN** the system saves priority as "High"
- **AND** staff can filter by priority

### Requirement: Ticket Status Workflow
The system SHALL track tickets through statuses: Open, InProgress, Resolved, Closed.

#### Scenario: Progress ticket to resolution
- **WHEN** ticket is created (status: Open)
- **THEN** staff assigns it (sets AssignedToUserId and AssignedAt)
- **AND** staff works on it (status: "InProgress")
- **AND** staff resolves it (status: "Resolved", sets ResolvedAt)
- **AND** staff closes it (status: "Closed", sets ClosedAt)

### Requirement: Threaded Messages
The system SHALL support threaded message conversations between citizens and staff.

#### Scenario: Add citizen message to ticket
- **WHEN** citizen adds message to their ticket
- **THEN** the system creates TicketMessage record
- **AND** links to ticket via TicketId
- **AND** sets SenderId to citizen ID
- **AND** sets SenderName to citizen full name
- **AND** sets IsStaff to false
- **AND** timestamps message with CreatedAt
- **AND** updates ticket UpdatedAt timestamp
- **AND** allows optional AttachmentUrls as JSON array

### Requirement: Ticket Message Validation
The system SHALL validate ticket messages have required content.

#### Scenario: Validate message content
- **WHEN** adding message to ticket
- **THEN** the system validates message is not empty
- **AND** validates message does not exceed 5000 characters
- **AND** validates AttachmentUrls is valid JSON if provided

### Requirement: Citizen Ticket Management
The system SHALL allow citizens to view and interact with their own tickets.

#### Scenario: List my tickets
- **WHEN** citizen requests their tickets
- **THEN** the system returns only tickets belonging to that citizen
- **AND** allows filtering by status (Open, InProgress, Resolved, Closed)
- **AND** allows filtering by category
- **AND** includes message count for each ticket
- **AND** orders by CreatedAt descending

#### Scenario: View ticket with messages
- **WHEN** citizen requests specific ticket by ID
- **THEN** the system verifies ownership
- **AND** returns full ticket details
- **AND** includes all messages ordered by CreatedAt ascending
- **AND** shows sender name and IsStaff flag for each message

### Requirement: Ticket Ownership Verification
The system SHALL ensure citizens can only access their own tickets.

#### Scenario: Access denied for other citizen's ticket
- **WHEN** citizen attempts to view ticket belonging to another citizen
- **THEN** the system returns 403 Forbidden error
- **AND** does not reveal ticket details

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying tickets from Tenant A
- **THEN** the system returns only tickets from Tenant A
- **AND** applies global query filter on TenantId

## MODIFIED Data Model

### SupportTicket Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy, indexed)
- **CitizenId**: Guid (Foreign Key to Citizen, indexed)
- **Citizen**: Navigation property to Citizen
- **TicketNumber**: string (Required, unique, auto-generated, indexed)
- **Subject**: string (Required, max 200 chars)
- **Description**: string (Required, max 5000 chars)
- **Category**: string (Required, max 50 chars)
- **Priority**: string (Required, max 50 chars)
- **Status**: string (Required, max 50 chars, default "Open")
- **AssignedToUserId**: Guid? (nullable)
- **AssignedAt**: DateTime? (UTC, nullable)
- **CreatedAt**: DateTime (UTC)
- **UpdatedAt**: DateTime? (UTC, nullable)
- **ResolvedAt**: DateTime? (UTC, nullable)
- **ClosedAt**: DateTime? (UTC, nullable)
- **Messages**: ICollection<TicketMessage> navigation property

### TicketMessage Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy, indexed)
- **TicketId**: Guid (Foreign Key to SupportTicket, indexed)
- **Ticket**: Navigation property to SupportTicket
- **SenderId**: Guid (User who sent message)
- **SenderName**: string (Required, cached name)
- **IsStaff**: bool (False for citizens, true for staff)
- **Message**: string (Required, max 5000 chars)
- **AttachmentUrls**: string? (Optional, JSON array of URLs)
- **CreatedAt**: DateTime (UTC)

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/SupportTickets/`):
- **CreateSupportTicket**: POST /api/support-tickets
  - Files: CreateSupportTicketCommand.cs, CreateSupportTicketCommandValidator.cs, CreateSupportTicketCommandHandler.cs, CreateSupportTicketResponse.cs, CreateSupportTicket.cs
- **GetUserSupportTickets**: GET /api/support-tickets/my
  - Files: GetUserSupportTicketsQuery.cs, GetUserSupportTicketsQueryHandler.cs, GetUserSupportTicketsResponse.cs, GetUserSupportTickets.cs
- **GetSupportTicketById**: GET /api/support-tickets/{id}
  - Files: GetSupportTicketByIdQuery.cs, GetSupportTicketByIdQueryHandler.cs, GetSupportTicketByIdResponse.cs, GetSupportTicketById.cs
- **AddTicketMessage**: POST /api/support-tickets/{id}/messages
  - Files: AddTicketMessageCommand.cs, AddTicketMessageCommandValidator.cs, AddTicketMessageCommandHandler.cs, AddTicketMessageResponse.cs, AddTicketMessage.cs

**Infrastructure**:
- Entities: `Infrastructure/Database/Entities/SupportTicket.cs` (includes TicketMessage as nested class in same file)
- EF Configuration: `Infrastructure/Database/Configurations/SupportTicketConfiguration.cs`, `Infrastructure/Database/Configurations/TicketMessageConfiguration.cs`

**Patterns Used**:
- CQRS via MediatR
- ErrorOr for functional error handling
- FluentValidation for request validation
- Carter for Minimal API endpoint organization
- Ownership verification for citizen endpoints
- Message threading with parent-child relationship
