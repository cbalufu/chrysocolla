# Applications & Permits

## Overview
The Applications & Permits capability manages citizen applications for various municipal services including housing applications, business licenses, building permits, zoning permits, event permits, health certificates, and more.

## Requirements

### Requirement: Application Submission
The system SHALL allow citizens to submit applications with type, title, description, and supporting documents.

#### Scenario: Submit housing application
- **WHEN** a citizen submits a housing application with title and description
- **THEN** the system creates an application record with status "Draft"
- **AND** generates application number "HSG-YYYYMMDD-XXXXXX"
- **AND** records submission date as current UTC timestamp
- **AND** returns the created application with application number

#### Scenario: Submit business license application
- **WHEN** a citizen submits a business license application
- **THEN** the system generates application number with "LIC-" prefix
- **AND** sets status to "Draft"
- **AND** allows attachment of required documents

#### Scenario: Submit without required fields
- **WHEN** a citizen attempts to submit application without title or type
- **THEN** the system rejects the submission
- **AND** returns validation errors

### Requirement: Application Types
The system SHALL support application types: Housing, Business License, Building Permit, Zoning Permit, Event Permit, Parking Permit, Health Certificate, Fire Safety Certificate, Liquor License, Trade License, and Other.

#### Scenario: Create building permit application
- **WHEN** creating application with type "Building Permit"
- **THEN** the system generates application number with "BLD-" prefix
- **AND** applies type-specific validation rules
- **AND** requires type-specific documents

#### Scenario: Create zoning permit application
- **WHEN** creating application with type "Zoning Permit"
- **THEN** the system generates application number with "ZON-" prefix
- **AND** enables zoning-specific workflows

### Requirement: Application Status Workflow
The system SHALL manage application lifecycle through statuses: Draft → Submitted → Under Review → Pending Documents → Pending Payment → Approved/Rejected/Withdrawn/Expired.

#### Scenario: Progress from Draft to Approved
- **WHEN** an application is created (status: "Draft")
- **THEN** citizen submits it (status: "Submitted")
- **AND** staff begins review (status: "Under Review")
- **AND** if documents needed, request them (status: "Pending Documents")
- **AND** if payment required, request payment (status: "Pending Payment")
- **AND** staff approves (status: "Approved") with approval date recorded
- **AND** the system records `ReviewedByUserId` and `ApprovalDate`

#### Scenario: Reject application
- **WHEN** staff determines application cannot be approved
- **THEN** staff updates status to "Rejected"
- **AND** provides review notes explaining rejection reason
- **AND** records rejection date and reviewer ID

#### Scenario: Citizen withdraws application
- **WHEN** a citizen withdraws their pending application
- **THEN** the system updates status to "Withdrawn"
- **AND** maintains the application record for audit

### Requirement: Application Number Generation
The system SHALL generate unique application numbers with type-specific prefixes: HSG (Housing), LIC (Business License), BLD (Building Permit), ZON (Zoning Permit), APP (Other).

#### Scenario: Generate housing application number
- **WHEN** creating a housing application
- **THEN** the system generates number "HSG-YYYYMMDD-XXXXXX"
- **AND** ensures global uniqueness

#### Scenario: Generate general application number
- **WHEN** creating application type "Other"
- **THEN** the system generates number with "APP-" prefix

### Requirement: Document Attachments
The system SHALL support multiple document attachments for applications with document type classification.

#### Scenario: Attach required documents
- **WHEN** a citizen attaches ID document, proof of residence, and application form
- **THEN** the system stores each in blob storage
- **AND** creates `ApplicationDocument` records linked to the application
- **AND** tracks document types and upload dates

#### Scenario: Review documents
- **WHEN** staff reviews application documents
- **THEN** staff can view all attached documents
- **AND** request additional documents if needed
- **AND** update status to "Pending Documents" when more are required

### Requirement: Application Review
The system SHALL allow staff to review applications, request additional information, and make approval decisions.

#### Scenario: Assign reviewer
- **WHEN** a manager assigns application to staff for review
- **THEN** the system records `ReviewedByUserId`
- **AND** notifies the assigned reviewer

#### Scenario: Add review notes
- **WHEN** a reviewer adds notes during review
- **THEN** the system stores notes in `ReviewNotes` field
- **AND** maintains audit trail

#### Scenario: Request additional documents
- **WHEN** reviewer determines more documents are needed
- **THEN** reviewer updates status to "Pending Documents"
- **AND** provides notes on what documents are required
- **AND** notifies the applicant

### Requirement: Application Status History
The system SHALL track all status changes with timestamps and user IDs for complete audit trail.

#### Scenario: Record status change
- **WHEN** application status changes from "Submitted" to "Under Review"
- **THEN** the system creates `ApplicationStatusHistory` record
- **AND** captures previous status, new status, change timestamp, and user ID
- **AND** maintains chronological history

#### Scenario: View status history
- **WHEN** viewing application status history
- **THEN** the system returns all status changes in chronological order
- **AND** includes who made each change and when

### Requirement: Application Data Storage
The system SHALL support flexible application-specific data storage using JSON format to accommodate varying requirements across application types.

#### Scenario: Store custom application data
- **WHEN** an application includes type-specific fields (e.g., business registration number, property dimensions)
- **THEN** the system stores the data in `ApplicationData` as JSON
- **AND** allows retrieval and validation of custom fields

### Requirement: Application Search and Filtering
The system SHALL provide search and filtering by type, status, citizen, and date range.

#### Scenario: Filter by application type and status
- **WHEN** searching for all "Building Permit" applications with status "Under Review"
- **THEN** the system returns matching applications
- **AND** respects tenant isolation

#### Scenario: Search by application number
- **WHEN** searching by application number (e.g., "BLD-20241111-ABC123")
- **THEN** the system returns the exact matching application

#### Scenario: View citizen's applications
- **WHEN** a citizen views their application history
- **THEN** the system returns all applications submitted by that citizen
- **AND** orders by submission date descending

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all application records.

#### Scenario: Tenant isolation
- **WHEN** querying applications from Tenant A
- **THEN** the system returns only applications from Tenant A
- **AND** does not expose applications from other tenants

## Data Model

### Application Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **CitizenId**: Guid (Foreign Key, Required)
- **ApplicationNumber**: string (Generated, Unique)
- **Type**: ApplicationType enum (Required)
- **Status**: ApplicationStatus enum (Default: Draft)
- **Title**: string (Required)
- **Description**: string (Required)
- **ApplicationData**: string (JSON, Optional)
- **SubmissionDate**: DateTime (Required)
- **ApprovalDate**: DateTime? (Nullable)
- **RejectionDate**: DateTime? (Nullable)
- **ReviewedByUserId**: Guid? (Nullable)
- **ReviewNotes**: string (Optional)
- **Documents**: ICollection<ApplicationDocument>
- **StatusHistory**: ICollection<ApplicationStatusHistory>
- Audit fields (CreatorId, CreationTime, LastModifierId, LastModificationTime, DeleterId, DeletionTime, IsDeleted)

### ApplicationType Enum
- Housing
- BusinessLicense
- BuildingPermit
- ZoningPermit
- EventPermit
- ParkingPermit
- HealthCertificate
- FireSafetyCertificate
- LiquorLicense
- TradeLicense
- Other

### ApplicationStatus Enum
- Draft
- Submitted
- UnderReview
- PendingDocuments
- PendingPayment
- Approved
- Rejected
- Withdrawn
- Expired

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/ApplicationAppService.cs`
- Implements `CrudAppService<Application, ApplicationDto, Guid, ApplicationDto, CreateUpdateApplicationDto>`
- Entity located in `src/CitizensPortal.Domain/Entities/Application.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/ApplicationEnums.cs`
- HTTP API: No controller yet (needs to be created)
- Planned endpoints: `GET/POST/PUT/DELETE /api/applications`
