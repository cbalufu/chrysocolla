# Billing Management

## Overview
The Billing Management capability handles generation and management of bills for municipal services including property rates, water, electricity, and waste collection.

## Requirements

### Requirement: Bill Generation
The system SHALL generate bills for citizens with type-specific billing periods and due dates.

#### Scenario: Generate property rates bill
- **WHEN** generating a quarterly property rates bill for a citizen
- **THEN** the system creates bill with type "Property Rates"
- **AND** generates bill number "RATE-YYYYMMDD-XXXXXXXX"
- **AND** sets amount, due date, and billing period
- **AND** sets status to "Unpaid" with balance equal to amount

#### Scenario: Generate utility bill
- **WHEN** generating water or electricity bill
- **THEN** the system creates bill with appropriate type
- **AND** generates bill number with type-specific prefix (WAT-, ELEC-)
- **AND** links to property reference if applicable

### Requirement: Bill Types
The system SHALL support bill types: Property Rates, Water, Electricity, Waste, and Other.

#### Scenario: Create waste collection bill
- **WHEN** creating bill for waste collection services
- **THEN** the system sets type to "Waste"
- **AND** generates bill number with "WST-" prefix

### Requirement: Payment Recording
The system SHALL record payments against bills and automatically update status based on payment amount.

#### Scenario: Full payment
- **WHEN** a citizen pays the full bill amount
- **THEN** the system calls `RecordPayment(amount)`
- **AND** updates `AmountPaid` to equal `Amount`
- **AND** sets `Balance` to zero
- **AND** changes status from "Unpaid" to "Paid"
- **AND** records `PaidDate` as current UTC timestamp

#### Scenario: Partial payment
- **WHEN** a citizen pays less than the full amount
- **THEN** the system updates `AmountPaid` with payment amount
- **AND** calculates `Balance` as `Amount - AmountPaid`
- **AND** changes status to "PartiallyPaid"

#### Scenario: Multiple payments
- **WHEN** a citizen makes multiple partial payments
- **THEN** the system accumulates `AmountPaid`
- **AND** recalculates balance after each payment
- **AND** changes to "Paid" when balance reaches zero

### Requirement: Bill Status Management
The system SHALL track bill status through: Unpaid → PartiallyPaid → Paid, with Overdue for unpaid bills past due date.

#### Scenario: Mark overdue bills
- **WHEN** system checks bills past due date with unpaid balance
- **THEN** the system updates status to "Overdue"
- **AND** triggers notification to citizen

### Requirement: Bill Search and Filtering
The system SHALL provide search and filtering by citizen, type, status, and billing period.

#### Scenario: View citizen's bills
- **WHEN** a citizen views their bill history
- **THEN** the system returns all bills for that citizen
- **AND** orders by issue date descending
- **AND** shows current balance for each

#### Scenario: Filter by status and type
- **WHEN** filtering for unpaid water bills
- **THEN** the system returns bills matching both criteria
- **AND** respects tenant isolation

### Requirement: Property-Linked Billing
The system SHALL link bills to property references for rates and utility billing.

#### Scenario: Generate bill for property
- **WHEN** generating property-related bill
- **THEN** the system links bill to property via `PropertyReference`
- **AND** associates with property owner citizen

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all bill records.

#### Scenario: Tenant isolation
- **WHEN** querying bills from Tenant A
- **THEN** the system returns only bills from Tenant A

## Data Model

### Bill Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **CitizenId**: Guid (Foreign Key, Required)
- **BillNumber**: string (Generated, Unique)
- **Type**: BillType enum (Required)
- **Status**: BillStatus enum (Default: Unpaid)
- **Amount**: decimal (Required)
- **AmountPaid**: decimal (Default: 0)
- **Balance**: decimal (Calculated)
- **IssueDate**: DateTime (Required)
- **DueDate**: DateTime (Required)
- **PaidDate**: DateTime? (Nullable)
- **Description**: string (Optional)
- **Period**: string (e.g., "Q1 2024")
- **PropertyReference**: string (Optional)
- Audit fields

### BillType Enum
- PropertyRates, Water, Electricity, Waste, Other

### BillStatus Enum
- Unpaid, PartiallyPaid, Paid, Overdue

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/BillAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Bill.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/BillEnums.cs`
- HTTP API: No controller yet (needs to be created)
- Payment recording logic in `Bill.RecordPayment()` method
