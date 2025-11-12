## MODIFIED Requirements

### Requirement: Service Request Submission
The system SHALL allow citizens to submit service requests with service type, title, description, priority, location, and preferred service date.

#### Scenario: Submit service request
- **WHEN** citizen submits service request with required fields
- **THEN** the system creates request with status "Submitted"
- **AND** generates unique request number in format SR-{TYPE}-{YYYYMMDD}-{GUID}
- **AND** assigns priority level (Low, Medium, High)
- **AND** validates service type is valid
- **AND** returns request details with request number

### Requirement: Request Status Workflow
The system SHALL track requests through statuses: Submitted, Assigned, InProgress, Completed, Cancelled.

#### Scenario: Progress request to completion
- **WHEN** service request is created (status: Submitted)
- **THEN** admin assigns it to staff (status: "Assigned", sets AssignedToUserId and AssignedAt)
- **AND** admin starts work (status: "InProgress")
- **AND** admin completes work (status: "Completed", sets CompletedAt and CompletionNotes)

### Requirement: Service Types
The system SHALL support service types: WasteCollection, StreetRepair, StreetLighting, TreeMaintenance, AnimalControl, Other.

#### Scenario: Submit waste collection request
- **WHEN** citizen creates service request with type "WasteCollection"
- **THEN** the system generates request number starting with "SR-WAS-"
- **AND** saves service type as "WasteCollection"

### Requirement: Priority Levels
The system SHALL support priority levels: Low, Medium, High.

#### Scenario: Set high priority
- **WHEN** citizen submits urgent service request with priority "High"
- **THEN** the system saves priority as "High"
- **AND** admin can filter high priority requests

### Requirement: Citizen Request Management
The system SHALL allow citizens to view their own service requests with filtering.

#### Scenario: List my requests
- **WHEN** citizen requests their service requests
- **THEN** the system returns only requests belonging to that citizen
- **AND** allows filtering by status (Submitted, Assigned, InProgress, Completed, Cancelled)
- **AND** allows filtering by service type
- **AND** orders by CreatedAt descending

#### Scenario: View request details
- **WHEN** citizen requests specific service request by ID
- **THEN** the system verifies ownership
- **AND** returns full request details including status, location, dates, completion notes

### Requirement: Admin Request Management
The system SHALL allow admin/staff to view all requests and update status.

#### Scenario: View all requests (admin)
- **WHEN** staff/admin requests service requests list
- **THEN** the system returns paginated list (20 per page)
- **AND** allows filtering by status, service type, priority, assigned user
- **AND** includes citizen name for each request
- **AND** orders by CreatedAt descending

#### Scenario: Update request status (admin)
- **WHEN** staff/admin updates service request status
- **THEN** the system validates new status is valid
- **AND** updates status and UpdatedAt timestamp
- **AND** if assigning to user, sets AssignedToUserId and AssignedAt
- **AND** if completing, sets CompletedAt and allows CompletionNotes

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying service requests from Tenant A
- **THEN** the system returns only requests from Tenant A
- **AND** applies global query filter on TenantId

## MODIFIED Data Model

### ServiceRequest Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy, indexed)
- **CitizenId**: Guid (Foreign Key to Citizen, indexed)
- **Citizen**: Navigation property to Citizen
- **RequestNumber**: string (Required, unique, auto-generated, indexed)
- **ServiceType**: string (Required, max 100 chars)
- **Title**: string (Required, max 200 chars)
- **Description**: string (Required, max 2000 chars)
- **Priority**: string (Required, max 50 chars)
- **Status**: string (Required, max 50 chars, default "Submitted")
- **Location**: string? (Optional, max 500 chars)
- **PreferredServiceDate**: DateTime? (UTC, nullable)
- **AssignedToUserId**: Guid? (nullable)
- **AssignedAt**: DateTime? (UTC, nullable)
- **CompletedAt**: DateTime? (UTC, nullable)
- **CompletionNotes**: string? (Optional, max 2000 chars)
- **CreatedAt**: DateTime (UTC)
- **UpdatedAt**: DateTime? (UTC, nullable)

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/ServiceRequests/`):
- **CreateServiceRequest**: POST /api/service-requests
  - Files: CreateServiceRequestCommand.cs, CreateServiceRequestCommandValidator.cs, CreateServiceRequestCommandHandler.cs, CreateServiceRequestResponse.cs, CreateServiceRequest.cs
- **GetUserServiceRequests**: GET /api/service-requests/my
  - Files: GetUserServiceRequestsQuery.cs, GetUserServiceRequestsQueryHandler.cs, GetUserServiceRequestsResponse.cs, GetUserServiceRequests.cs
- **GetServiceRequestById**: GET /api/service-requests/{id}
  - Files: GetServiceRequestByIdQuery.cs, GetServiceRequestByIdQueryHandler.cs, GetServiceRequestByIdResponse.cs, GetServiceRequestById.cs

**Admin Features** (located in `src/CitizensPortal.Api/Features/Admin/ServiceRequests/`):
- **GetAllServiceRequests**: GET /api/admin/service-requests (requires StaffOrAdmin role)
  - Files: GetAllServiceRequestsQuery.cs, GetAllServiceRequestsQueryHandler.cs, GetAllServiceRequestsResponse.cs, GetAllServiceRequests.cs
- **UpdateServiceRequestStatus**: PUT /api/admin/service-requests/{id}/status (requires StaffOrAdmin role)
  - Files: UpdateServiceRequestStatusCommand.cs, UpdateServiceRequestStatusCommandValidator.cs, UpdateServiceRequestStatusCommandHandler.cs, UpdateServiceRequestStatusResponse.cs, UpdateServiceRequestStatus.cs

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/ServiceRequest.cs`
- EF Configuration: `Infrastructure/Database/Configurations/ServiceRequestConfiguration.cs`

**Patterns Used**:
- CQRS via MediatR
- ErrorOr for functional error handling
- FluentValidation for request validation
- Carter for Minimal API endpoint organization
- Role-based authorization with custom policies (StaffOrAdmin)
- Ownership verification for citizen endpoints
