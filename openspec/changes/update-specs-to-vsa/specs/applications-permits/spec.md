## ADDED Requirements

### Requirement: VSA Architecture Implementation
The system SHALL implement applications and permits using Vertical Slice Architecture with Minimal APIs.

#### Scenario: VSA implementation
- **WHEN** citizen or admin interacts with application endpoints
- **THEN** the system uses Carter modules for routing
- **AND** uses MediatR for CQRS command/query handling
- **AND** uses FluentValidation for request validation
- **AND** uses ErrorOr for functional error handling

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/Applications/`):
- **SubmitApplication**: POST /api/applications
  - Auto-generates application number format: APP-{TYPE}-{YYYYMMDD}-{GUID}
  - Application types: BuildingPermit, BusinessLicense, ResidencyPermit, ParkingPermit, EventPermit, Other
  - Stores form data as JSON in FormDataJson field
- **GetUserApplications**: GET /api/applications/my
- **GetApplicationById**: GET /api/applications/{id}
- **UploadDocument**: POST /api/applications/{id}/documents
  - Max file size: 50MB
  - Stores file metadata (URL, size, content type)

**Admin Features** (located in `src/CitizensPortal.Api/Features/Admin/Applications/`):
- **GetAllApplications**: GET /api/admin/applications (requires StaffOrAdmin role)
  - Paginated (20 per page)
- **ReviewApplication**: PUT /api/admin/applications/{id}/review (requires StaffOrAdmin role)
  - Review statuses: Approved, Rejected, UnderReview
  - Auto-timestamps ReviewedAt and ApprovedAt

**Infrastructure**:
- Entities: `Infrastructure/Database/Entities/Application.cs`, `Infrastructure/Database/Entities/ApplicationDocument.cs`
- EF Configuration: `Infrastructure/Database/Configurations/ApplicationConfiguration.cs`, `Infrastructure/Database/Configurations/ApplicationDocumentConfiguration.cs`

**Patterns**: CQRS via MediatR, ErrorOr, FluentValidation, Carter, Role-based authorization
