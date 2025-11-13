## ADDED Requirements

### Requirement: VSA Architecture Implementation
The system SHALL implement billing using Vertical Slice Architecture with Minimal APIs.

#### Scenario: VSA implementation
- **WHEN** citizen interacts with billing endpoints
- **THEN** the system uses Carter modules for routing
- **AND** uses MediatR for CQRS query handling
- **AND** uses ErrorOr for functional error handling

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/Bills/`):
- **GetUserBills**: GET /api/bills/my
  - Filter by isPaid status
  - Returns bill number, amount, due date, payment status

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/Bill.cs`
- EF Configuration: `Infrastructure/Database/Configurations/BillConfiguration.cs`

**Patterns**: CQRS via MediatR, ErrorOr, Carter
