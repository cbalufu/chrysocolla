# Infrastructure Projects

## Overview
The Infrastructure Projects capability provides public transparency into municipal infrastructure projects with GIS mapping, budget tracking, progress updates, contractor information, and photo documentation.

## Requirements

### Requirement: Project Creation
The system SHALL allow creation of infrastructure projects with name, category, budget, location, and timeline.

#### Scenario: Create road construction project
- **WHEN** creating new road construction project
- **THEN** the system creates project with status "Planned"
- **AND** generates project number "PROJ-YYYY-XXXXXX"
- **AND** sets category to "Roads"
- **AND** initializes percentage complete to 0%

### Requirement: Project Categories
The system SHALL support project categories: Roads, Water, Electricity, PublicBuildings, Parks, Drainage.

#### Scenario: Water infrastructure project
- **WHEN** creating project with category "Water"
- **THEN** the system categorizes for water infrastructure
- **AND** enables category-specific reporting

### Requirement: Project Status Workflow
The system SHALL track projects through statuses: Planned, InProgress, OnHold, Completed, Cancelled.

#### Scenario: Progress project
- **WHEN** project created (status: Planned)
- **THEN** construction starts (status: "InProgress")
- **AND** work continues until complete (status: "Completed")
- **AND** records actual completion date

### Requirement: GeoJSON Boundary Mapping
The system SHALL support GeoJSON polygon data for defining project boundaries and areas.

#### Scenario: Define project area
- **WHEN** creating area-based project (e.g., road resurfacing)
- **THEN** the system stores GeoJSON polygon in `BoundaryGeoJson`
- **AND** enables map visualization
- **AND** supports spatial queries

#### Scenario: Point-based project
- **WHEN** creating point location project
- **THEN** the system stores latitude/longitude
- **AND** displays on map as point marker

### Requirement: Budget Tracking
The system SHALL track project budget and spending with variance reporting.

#### Scenario: Track project spending
- **WHEN** project costs are incurred
- **THEN** the system updates `SpentAmount`
- **AND** calculates budget variance
- **AND** alerts if over budget

### Requirement: Progress Tracking
The system SHALL track project completion percentage with milestone updates.

#### Scenario: Update progress
- **WHEN** project manager updates progress to 45%
- **THEN** the system updates `PercentageComplete`
- **AND** creates `ProjectUpdate` record
- **AND** includes update description and date

### Requirement: Project Updates
The system SHALL maintain chronological progress updates with descriptions.

#### Scenario: Add progress update
- **WHEN** adding project update "Foundation completed"
- **THEN** the system creates `ProjectUpdate` linked to project
- **AND** records update title, description, and percentage
- **AND** timestamps the update

### Requirement: Project Images
The system SHALL support photo documentation of project progress.

#### Scenario: Upload progress photo
- **WHEN** uploading construction progress photo
- **THEN** the system stores image in blob storage
- **AND** creates `ProjectImage` record with caption
- **AND** links to project
- **AND** displays in project gallery

### Requirement: Contractor Management
The system SHALL track contractor information for projects.

#### Scenario: Assign contractor
- **WHEN** awarding project to contractor
- **THEN** the system records contractor name and contact
- **AND** displays contractor information publicly

### Requirement: Timeline Management
The system SHALL track planned and actual project timelines.

#### Scenario: Set project timeline
- **WHEN** planning project
- **THEN** the system records start date and planned completion date
- **AND** tracks actual completion date when finished
- **AND** calculates timeline variance

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying projects from Tenant A
- **THEN** the system returns only projects from Tenant A

## Data Model

### InfrastructureProject Entity
- **Id**: Guid
- **TenantId**: Guid?
- **ProjectNumber**: string
- **Name**: string
- **Description**: string
- **Category**: ProjectCategory enum
- **Status**: ProjectStatus enum
- **Location**: string
- **Latitude**: double?
- **Longitude**: double?
- **BoundaryGeoJson**: string (GeoJSON polygon)
- **Budget**: decimal
- **SpentAmount**: decimal
- **StartDate**: DateTime?
- **PlannedCompletionDate**: DateTime?
- **ActualCompletionDate**: DateTime?
- **PercentageComplete**: int
- **ContractorName**: string
- **ContractorContact**: string
- **ProjectManagerId**: Guid?
- **Updates**: ICollection<ProjectUpdate>
- **Images**: ICollection<ProjectImage>
- Audit fields

### ProjectCategory Enum
- Roads, Water, Electricity, PublicBuildings, Parks, Drainage

### ProjectStatus Enum
- Planned, InProgress, OnHold, Completed, Cancelled

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/InfrastructureProjectAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/InfrastructureProject.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/InfrastructureEnums.cs`
- HTTP API: No controller yet (needs to be created)
- GeoJSON support for mapping and spatial queries
- Service areas tracked separately in `ServiceArea` entity
