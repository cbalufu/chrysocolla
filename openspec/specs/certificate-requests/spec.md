# Certificate Requests

## Overview
The Certificate Requests capability enables citizens to request official certificates including birth, death, marriage, residence, good conduct, and tax clearance certificates.

## Requirements

### Requirement: Certificate Request Submission
The system SHALL allow citizens to request certificates with type, purpose, and delivery preference.

#### Scenario: Request birth certificate
- **WHEN** citizen requests birth certificate
- **THEN** the system creates request with type "Birth"
- **AND** generates unique reference number
- **AND** sets status to "Submitted"
- **AND** calculates application fee

### Requirement: Certificate Types
The system SHALL support certificate types: Birth, Death, Marriage, Residence, GoodConduct, TaxClearance.

#### Scenario: Request tax clearance certificate
- **WHEN** citizen requests "TaxClearance" certificate
- **THEN** the system validates citizen has no outstanding bills
- **AND** processes request if validation passes

### Requirement: Certificate Status Workflow
The system SHALL track certificate requests through statuses: Submitted, UnderReview, Approved, Ready, Collected, Rejected.

#### Scenario: Progress to collection
- **WHEN** certificate request submitted (status: Submitted)
- **THEN** staff reviews (status: "UnderReview")
- **AND** approves (status: "Approved")
- **AND** prepares certificate (status: "Ready")
- **AND** citizen collects (status: "Collected")

### Requirement: Delivery Options
The system SHALL support delivery methods: Collection, Email, Postal.

#### Scenario: Request email delivery
- **WHEN** citizen selects "Email" delivery
- **THEN** the system notes delivery preference
- **AND** emails PDF certificate when ready

#### Scenario: In-person collection
- **WHEN** citizen selects "Collection" delivery
- **THEN** the system notifies when ready for pickup
- **AND** records collection date when collected

### Requirement: Application Fees
The system SHALL calculate and track application fees for certificate requests.

#### Scenario: Pay certificate fee
- **WHEN** citizen pays certificate application fee
- **THEN** the system links payment to certificate request
- **AND** proceeds with processing

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying certificate requests from Tenant A
- **THEN** the system returns only requests from Tenant A

## Data Model

### CertificateRequest Entity
- **Id**: Guid
- **TenantId**: Guid?
- **CitizenId**: Guid (FK)
- **CertificateType**: CertificateType enum
- **Status**: CertificateStatus enum
- **ReferenceNumber**: string
- **Purpose**: string
- **DeliveryMethod**: DeliveryMethod enum
- **ApplicationFee**: decimal
- **IsPaid**: bool
- **ApprovalDate**: DateTime?
- **CollectionDate**: DateTime?
- Audit fields

### CertificateType Enum
- Birth, Death, Marriage, Residence, GoodConduct, TaxClearance

### CertificateStatus Enum
- Submitted, UnderReview, Approved, Ready, Collected, Rejected

### DeliveryMethod Enum
- Collection, Email, Postal

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/CertificateRequestAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/CertificateRequest.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/CertificateEnums.cs`
- HTTP API: No controller yet (needs to be created)
