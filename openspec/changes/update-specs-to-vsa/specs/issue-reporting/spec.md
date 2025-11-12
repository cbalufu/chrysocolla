## ADDED Requirements

### Requirement: VSA Architecture Implementation
The system SHALL implement issue reporting using Vertical Slice Architecture with Minimal APIs.

#### Scenario: VSA implementation
- **WHEN** citizen or admin interacts with issue reporting endpoints
- **THEN** the system uses Carter modules for routing
- **AND** uses MediatR for CQRS command/query handling
- **AND** uses FluentValidation for request validation
- **AND** uses ErrorOr for functional error handling

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Citizen Features** (located in `src/CitizensPortal.Api/Features/Issues/`):
- **CreateIssue**: POST /api/issues
- **GetUserIssues**: GET /api/issues/my
- **GetIssueById**: GET /api/issues/{id}
- **UpdateIssue**: PUT /api/issues/{id}
- **DeleteIssue**: DELETE /api/issues/{id}

**Admin Features** (located in `src/CitizensPortal.Api/Features/Admin/Issues/`):
- **GetAllIssues**: GET /api/admin/issues (requires StaffOrAdmin role)
- **AssignIssue**: PUT /api/admin/issues/{id}/assign (requires StaffOrAdmin role)
- **UpdateIssueStatus**: PUT /api/admin/issues/{id}/status (requires StaffOrAdmin role)
- **AddInternalComment**: POST /api/admin/issues/{id}/comments (requires StaffOrAdmin role)

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/Issue.cs`
- EF Configuration: `Infrastructure/Database/Configurations/IssueConfiguration.cs`

**Patterns**: CQRS via MediatR, ErrorOr, FluentValidation, Carter, Role-based authorization
