## MODIFIED Requirements

### Requirement: Certificate Request Submission
The system SHALL allow citizens to submit certificate requests with type, purpose, delivery preference, and auto-generated reference number.

#### Scenario: Request birth certificate
- **WHEN** citizen submits birth certificate request
- **THEN** the system creates request with type "Birth"
- **AND** generates unique reference number in format CERT-{TYPE}-{YYYYMMDD}-{GUID}
- **AND** sets status to "Submitted"
- **AND** calculates application fee based on certificate type
- **AND** sets IsPaid to false
- **AND** returns request details

### Requirement: Certificate Types
The system SHALL support certificate types: Birth, Death, Marriage, Residence, GoodConduct, TaxClearance with type-specific fees.

#### Scenario: Request tax clearance certificate
- **WHEN** citizen requests "TaxClearance" certificate
- **THEN** the system creates request with type "TaxClearance"
- **AND** sets appropriate application fee
- **AND** includes purpose field (required for tax clearance)

### Requirement: Certificate Status Workflow
The system SHALL track certificate requests through statuses: Submitted, UnderReview, Approved, Ready, Collected, Rejected.

#### Scenario: Progress to collection
- **WHEN** certificate request is submitted (status: Submitted)
- **THEN** admin reviews it (status: "UnderReview")
- **AND** admin approves it (status: "Approved", sets ApprovalDate)
- **AND** admin marks as ready (status: "Ready")
- **AND** admin records collection (status: "Collected", sets CollectionDate)

#### Scenario: Reject request
- **WHEN** admin rejects certificate request
- **THEN** the system sets status to "Rejected"
- **AND** records rejection reason
- **AND** notifies citizen

### Requirement: Delivery Options
The system SHALL support delivery methods: Collection, Email, Postal.

#### Scenario: Request email delivery
- **WHEN** citizen selects "Email" delivery method
- **THEN** the system records delivery preference as "Email"
- **AND** when status changes to "Ready"
- **THEN** the system sends email with certificate PDF attachment

#### Scenario: In-person collection
- **WHEN** citizen selects "Collection" delivery method
- **THEN** the system records delivery preference as "Collection"
- **AND** sends notification when ready for pickup
- **AND** records CollectionDate when admin marks as collected

## ADDED Requirements

### Requirement: Citizen Request Management
The system SHALL allow citizens to view their certificate requests with filtering.

#### Scenario: List my certificate requests
- **WHEN** citizen requests their certificate requests
- **THEN** the system returns only requests belonging to that citizen
- **AND** allows filtering by status
- **AND** allows filtering by certificate type
- **AND** orders by CreatedAt descending
- **AND** includes payment status (IsPaid)

#### Scenario: View request details
- **WHEN** citizen requests specific certificate request by ID
- **THEN** the system verifies ownership
- **AND** returns full request details including reference number, status, fees, delivery method

### Requirement: Admin Request Processing
The system SHALL allow admin/staff to view all requests and update status.

#### Scenario: View all certificate requests (admin)
- **WHEN** staff/admin requests certificate requests list
- **THEN** the system returns paginated list (20 per page)
- **AND** allows filtering by status, certificate type, payment status
- **AND** includes citizen name for each request
- **AND** orders by CreatedAt descending
- **AND** requires StaffOrAdmin role

#### Scenario: Update request status (admin)
- **WHEN** staff/admin updates certificate request status
- **THEN** the system validates new status is valid workflow state
- **AND** updates status and UpdatedAt timestamp
- **AND** if approving, sets ApprovalDate
- **AND** if collecting, sets CollectionDate
- **AND** requires StaffOrAdmin role

### Requirement: Reference Number Generation
The system SHALL generate unique reference numbers for certificate requests.

#### Scenario: Generate reference number
- **WHEN** certificate request is created
- **THEN** the system generates reference number in format CERT-{TYPE}-{YYYYMMDD}-{GUID}
- **AND** for Birth certificate: CERT-BIR-20251112-ABC12345
- **AND** ensures uniqueness within tenant

## MODIFIED Data Model

### CertificateRequest Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy, indexed)
- **CitizenId**: Guid (Foreign Key to Citizen, indexed)
- **Citizen**: Navigation property to Citizen
- **ReferenceNumber**: string (Required, unique, auto-generated, indexed)
- **CertificateType**: string (Required, max 50 chars: Birth, Death, Marriage, Residence, GoodConduct, TaxClearance)
- **Status**: string (Required, max 50 chars, default "Submitted")
- **Purpose**: string (Required, max 500 chars)
- **DeliveryMethod**: string (Required, max 50 chars: Collection, Email, Postal)
- **ApplicationFee**: decimal (Required, precision 18,2)
- **IsPaid**: bool (Default false, indexed)
- **ApprovalDate**: DateTime? (UTC, nullable)
- **CollectionDate**: DateTime? (UTC, nullable)
- **RejectionReason**: string? (Optional, max 1000 chars)
- **CreatedAt**: DateTime (UTC)
- **UpdatedAt**: DateTime? (UTC, nullable)

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/CertificateRequests/`):
- **SubmitCertificateRequest**: POST /api/certificate-requests
  - Files: SubmitCertificateRequestCommand.cs, SubmitCertificateRequestCommandValidator.cs, SubmitCertificateRequestCommandHandler.cs, SubmitCertificateRequestResponse.cs, SubmitCertificateRequest.cs
- **GetUserCertificateRequests**: GET /api/certificate-requests/my
  - Files: GetUserCertificateRequestsQuery.cs, GetUserCertificateRequestsQueryHandler.cs, GetUserCertificateRequestsResponse.cs, GetUserCertificateRequests.cs
- **GetCertificateRequestById**: GET /api/certificate-requests/{id}
  - Files: GetCertificateRequestByIdQuery.cs, GetCertificateRequestByIdQueryHandler.cs, GetCertificateRequestByIdResponse.cs, GetCertificateRequestById.cs

**Admin Features** (located in `src/CitizensPortal.Api/Features/Admin/CertificateRequests/`):
- **GetAllCertificateRequests**: GET /api/admin/certificate-requests (requires StaffOrAdmin role)
  - Files: GetAllCertificateRequestsQuery.cs, GetAllCertificateRequestsQueryHandler.cs, GetAllCertificateRequestsResponse.cs, GetAllCertificateRequests.cs
- **UpdateRequestStatus**: PUT /api/admin/certificate-requests/{id}/status (requires StaffOrAdmin role)
  - Files: UpdateRequestStatusCommand.cs, UpdateRequestStatusCommandValidator.cs, UpdateRequestStatusCommandHandler.cs, UpdateRequestStatusResponse.cs, UpdateRequestStatus.cs

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/CertificateRequest.cs`
- EF Configuration: `Infrastructure/Database/Configurations/CertificateRequestConfiguration.cs`

**Patterns Used**:
- CQRS via MediatR
- ErrorOr for functional error handling
- FluentValidation for request validation
- Carter for Minimal API endpoint organization
- Role-based authorization with custom policies (StaffOrAdmin)
- Ownership verification for citizen endpoints
