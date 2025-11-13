# Consultations

## Overview
The Consultations capability enables municipalities to conduct public consultations on policies, budgets, infrastructure projects, and development plans with citizen comment and feedback collection.

## Requirements

### Requirement: Consultation Creation
The system SHALL allow creation of public consultations with title, description, type, and consultation period.

#### Scenario: Create budget consultation
- **WHEN** creating consultation for annual budget review
- **THEN** the system creates consultation with status "Draft"
- **AND** sets type to "Budget"
- **AND** defines consultation start and end dates
- **AND** publishes consultation document

### Requirement: Consultation Types
The system SHALL support consultation types: Policy, Budget, InfrastructureProject, DevelopmentPlan.

#### Scenario: Infrastructure project consultation
- **WHEN** creating consultation for new road project
- **THEN** the system sets type to "InfrastructureProject"
- **AND** can link to related infrastructure project
- **AND** gathers citizen feedback

### Requirement: Consultation Status
The system SHALL track consultations through statuses: Draft, Open, Closed, Published.

#### Scenario: Open consultation
- **WHEN** consultation start date arrives
- **THEN** the system updates status to "Open"
- **AND** enables citizen comment submission

#### Scenario: Close and publish results
- **WHEN** consultation end date passes
- **THEN** the system updates status to "Closed"
- **AND** staff can publish summary (status: "Published")
- **AND** makes results publicly available

### Requirement: Public Comments
The system SHALL allow citizens to submit comments on consultations with moderation.

#### Scenario: Submit comment
- **WHEN** citizen submits comment on open consultation
- **THEN** the system creates `ConsultationComment` record
- **AND** sets status to "Pending" (awaiting moderation)
- **AND** links to consultation and citizen

#### Scenario: Approve comment
- **WHEN** moderator reviews and approves comment
- **THEN** the system updates comment status to "Approved"
- **AND** displays comment publicly

#### Scenario: Reject inappropriate comment
- **WHEN** moderator rejects comment
- **THEN** the system updates status to "Rejected"
- **AND** hides from public view

### Requirement: Comment Moderation
The system SHALL provide moderation workflow for citizen comments with approval/rejection.

#### Scenario: Moderate pending comments
- **WHEN** moderator reviews pending comments
- **THEN** the system shows all comments with status "Pending"
- **AND** allows bulk approval/rejection
- **AND** maintains audit trail

### Requirement: Consultation Documents
The system SHALL support attachment of consultation documents for citizen review.

#### Scenario: Attach policy document
- **WHEN** attaching consultation document
- **THEN** the system stores document in blob storage
- **AND** makes available for download
- **AND** links to consultation

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying consultations from Tenant A
- **THEN** the system returns only consultations from Tenant A

## Data Model

### Consultation Entity
- **Id**: Guid
- **TenantId**: Guid?
- **Title**: string
- **Description**: string
- **Type**: ConsultationType enum
- **Status**: ConsultationStatus enum
- **StartDate**: DateTime
- **EndDate**: DateTime
- **DocumentUrl**: string (Optional)
- **Comments**: ICollection<ConsultationComment>
- Audit fields

### ConsultationComment Entity
- **Id**: Guid
- **ConsultationId**: Guid (FK)
- **CitizenId**: Guid (FK)
- **CommentText**: string
- **Status**: CommentStatus enum
- **SubmittedDate**: DateTime
- **ModeratedBy**: Guid?
- **ModerationDate**: DateTime?

### ConsultationType Enum
- Policy, Budget, InfrastructureProject, DevelopmentPlan

### ConsultationStatus Enum
- Draft, Open, Closed, Published

### CommentStatus Enum
- Pending, Approved, Rejected

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/ConsultationAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Consultation.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/CommunityEnums.cs`
- HTTP API: No controller yet (needs to be created)
- Moderation workflow requires staff permissions
