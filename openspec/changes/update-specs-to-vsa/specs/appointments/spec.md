## ADDED Requirements

### Requirement: VSA Architecture Implementation
The system SHALL implement appointments using Vertical Slice Architecture with Minimal APIs.

#### Scenario: VSA implementation
- **WHEN** citizen interacts with appointment endpoints
- **THEN** the system uses Carter modules for routing
- **AND** uses MediatR for CQRS command/query handling
- **AND** uses FluentValidation for request validation
- **AND** uses ErrorOr for functional error handling

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/Appointments/`):
- **BookAppointment**: POST /api/appointments
  - Appointment types: GeneralInquiry, BillPayment, DocumentSubmission, Other
  - Statuses: Scheduled, Completed, Cancelled
- **GetUserAppointments**: GET /api/appointments/my
  - Filter by status
  - Returns appointment details with scheduled time

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/Appointment.cs`
- EF Configuration: `Infrastructure/Database/Configurations/AppointmentConfiguration.cs`

**Patterns**: CQRS via MediatR, ErrorOr, FluentValidation, Carter
