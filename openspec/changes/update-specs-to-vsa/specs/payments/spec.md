## ADDED Requirements

### Requirement: VSA Architecture Implementation
The system SHALL implement payments using Vertical Slice Architecture with Minimal APIs.

#### Scenario: VSA implementation
- **WHEN** citizen interacts with payment endpoints
- **THEN** the system uses Carter modules for routing
- **AND** uses MediatR for CQRS command handling
- **AND** uses FluentValidation for request validation
- **AND** uses ErrorOr for functional error handling

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/Payments/`):
- **CreatePayment**: POST /api/payments
  - Creates payment record for bill
  - Statuses: Pending, Completed, Failed

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/Payment.cs`
- EF Configuration: `Infrastructure/Database/Configurations/PaymentConfiguration.cs`

**Patterns**: CQRS via MediatR, ErrorOr, FluentValidation, Carter
