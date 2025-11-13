# Payment Processing

## Overview
The Payment Processing capability handles recording and tracking of payments made by citizens for bills, applications, and other municipal services.

## Requirements

### Requirement: Payment Recording
The system SHALL record payment transactions with amount, method, reference, and link to related entity (bill, application, etc.).

#### Scenario: Record bill payment
- **WHEN** a citizen makes a payment for a bill
- **THEN** the system creates a payment record with amount, payment method, and transaction reference
- **AND** generates a unique payment reference number
- **AND** links payment to the bill
- **AND** calls bill's `RecordPayment()` method to update bill status
- **AND** creates a payment receipt

#### Scenario: Record application fee payment
- **WHEN** a citizen pays application fee
- **THEN** the system records payment linked to the application
- **AND** updates application status from "Pending Payment" to "Under Review"

### Requirement: Payment Methods
The system SHALL support multiple payment methods including Cash, Card, EFT, Mobile Money, and Other.

#### Scenario: Cash payment
- **WHEN** processing cash payment at municipal office
- **THEN** the system records payment method as "Cash"
- **AND** generates receipt for citizen

#### Scenario: Electronic payment
- **WHEN** citizen pays online via card or EFT
- **THEN** the system records appropriate payment method
- **AND** stores transaction reference from payment gateway

### Requirement: Payment Receipts
The system SHALL generate payment receipts with unique receipt numbers for all successful payments.

#### Scenario: Generate receipt
- **WHEN** a payment is successfully recorded
- **THEN** the system creates `PaymentReceipt` record
- **AND** generates unique receipt number
- **AND** includes payment details, amount, date, and payer information
- **AND** makes receipt available for download/print

### Requirement: Payment Status Tracking
The system SHALL track payment status through: Pending → Processing → Completed, with Failed as alternate state.

#### Scenario: Complete payment
- **WHEN** payment processing succeeds
- **THEN** the system updates status to "Completed"
- **AND** records completion timestamp
- **AND** generates receipt

#### Scenario: Failed payment
- **WHEN** payment processing fails
- **THEN** the system updates status to "Failed"
- **AND** records failure reason
- **AND** notifies citizen

### Requirement: Payment Search and Filtering
The system SHALL provide search and filtering by citizen, payment method, date range, and status.

#### Scenario: View citizen's payment history
- **WHEN** a citizen views their payment history
- **THEN** the system returns all payments made by that citizen
- **AND** orders by payment date descending
- **AND** shows associated bill/application details

#### Scenario: Search by transaction reference
- **WHEN** searching by transaction reference number
- **THEN** the system returns the matching payment record

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all payment records.

#### Scenario: Tenant isolation
- **WHEN** querying payments from Tenant A
- **THEN** the system returns only payments from Tenant A

## Data Model

### Payment Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **CitizenId**: Guid (Foreign Key, Required)
- **Amount**: decimal (Required)
- **PaymentMethod**: PaymentMethod enum (Required)
- **PaymentDate**: DateTime (Required)
- **TransactionReference**: string (Optional)
- **Status**: PaymentStatus enum
- **BillId**: Guid? (Nullable, Foreign Key)
- **ApplicationId**: Guid? (Nullable, Foreign Key)
- **Notes**: string (Optional)
- Audit fields

### PaymentMethod Enum
- Cash, Card, EFT, MobileMoney, Other

### PaymentStatus Enum
- Pending, Processing, Completed, Failed

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/PaymentAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Payment.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/PaymentEnums.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/PaymentController.cs`
- Endpoints: `GET/POST /api/payments`
