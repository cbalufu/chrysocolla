# Service Requests

## Overview
The Service Requests capability provides a general-purpose system for citizens to request municipal services with categorization, priority tracking, and status workflow.

## Requirements

### Requirement: Service Request Submission
The system SHALL allow citizens to submit service requests with category, description, and priority.

#### Scenario: Submit service request
- **WHEN** citizen submits service request
- **THEN** the system creates request with status "New"
- **AND** generates unique reference number
- **AND** assigns priority level
- **AND** returns request details

### Requirement: Request Status Workflow
The system SHALL track requests through statuses: New, Assigned, InProgress, Completed, Cancelled.

#### Scenario: Progress request to completion
- **WHEN** service request is created (status: New)
- **THEN** staff assigns it (status: "Assigned")
- **AND** work begins (status: "InProgress")
- **AND** work completes (status: "Completed")

### Requirement: Priority Levels
The system SHALL support priority levels: Low, Medium, High, Urgent.

#### Scenario: Set high priority
- **WHEN** service request is urgent
- **THEN** staff sets priority to "High" or "Urgent"
- **AND** prioritizes in queue

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying service requests from Tenant A
- **THEN** the system returns only requests from Tenant A

## Data Model

### ServiceRequest Entity
- **Id**: Guid
- **TenantId**: Guid?
- **CitizenId**: Guid (FK)
- **Category**: ServiceRequestCategory enum
- **Description**: string
- **Priority**: ServiceRequestPriority enum
- **Status**: ServiceRequestStatus enum
- **ReferenceNumber**: string
- **AssignedToUserId**: Guid?
- **CompletionNotes**: string
- Audit fields

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/ServiceRequestAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/ServiceRequest.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/ServiceRequestEnums.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/ServiceRequestController.cs`
- Endpoints: `GET/POST/PUT/DELETE /api/service-requests`
