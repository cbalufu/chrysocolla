## ADDED Requirements

### Requirement: VSA Architecture Implementation
The system SHALL implement document management using Vertical Slice Architecture integrated with Applications feature.

#### Scenario: VSA implementation
- **WHEN** citizen uploads documents
- **THEN** the system uses Carter modules for routing
- **AND** uses MediatR for CQRS command handling
- **AND** uses FluentValidation for request validation
- **AND** uses ErrorOr for functional error handling

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Implementation**: Document management is integrated into the Applications feature slice

**Citizen Features** (located in `src/CitizensPortal.Api/Features/Applications/`):
- **UploadDocument**: POST /api/applications/{id}/documents
  - Validates file size (max 50MB)
  - Stores metadata: DocumentType, FileName, FileUrl, FileSize, ContentType
  - Links to application via ApplicationId foreign key

**Infrastructure**:
- Entity: `Infrastructure/Database/Entities/ApplicationDocument.cs`
- EF Configuration: `Infrastructure/Database/Configurations/ApplicationDocumentConfiguration.cs`
- Relationship: Many ApplicationDocuments to one Application

**Patterns**: CQRS via MediatR, ErrorOr, FluentValidation, Carter
