## ADDED Requirements

### Requirement: VSA Architecture Implementation
The system SHALL implement emergency alerts using Vertical Slice Architecture with Minimal APIs.

#### Scenario: VSA implementation
- **WHEN** citizen or public user interacts with alert endpoints
- **THEN** the system uses Carter modules for routing
- **AND** uses MediatR for CQRS query handling
- **AND** uses ErrorOr for functional error handling
- **AND** allows public access without authentication

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/EmergencyAlerts/`):
- **GetActiveAlerts**: GET /api/emergency-alerts/active
  - Returns alerts where current time is between StartTime and EndTime
  - Public endpoint (no authentication required)
- **GetAlertById**: GET /api/emergency-alerts/{id}
  - Public endpoint
- **GetAllAlerts**: GET /api/emergency-alerts
  - Returns all alerts (active and expired)
  - Public endpoint

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/EmergencyAlert.cs`
- EF Configuration: `Infrastructure/Database/Configurations/EmergencyAlertConfiguration.cs`
- Fields: AlertType, Severity, Title, Message, StartTime, EndTime

**Patterns**: CQRS via MediatR, ErrorOr, Carter

**Note**: Admin endpoints for creating/managing alerts not yet implemented
