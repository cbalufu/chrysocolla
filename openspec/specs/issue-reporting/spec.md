# Issue Reporting

## Overview
The Issue Reporting capability allows citizens to report municipal problems such as potholes, broken street lights, water leaks, and other infrastructure issues. Issues are tracked with location data, priority levels, and status through to resolution.

## Requirements

### Requirement: Issue Submission
The system SHALL allow citizens to submit issue reports with title, description, category, and location information.

#### Scenario: Submit new issue with location
- **WHEN** a citizen submits an issue with title, description, category, location address, and GPS coordinates
- **THEN** the system creates an issue record with status "New" and priority "Medium"
- **AND** generates a unique reference number in format "ISS-YYYYMMDD-XXXXXXXX"
- **AND** associates the issue with the submitting citizen
- **AND** returns the created issue with reference number

#### Scenario: Submit issue without GPS coordinates
- **WHEN** a citizen submits an issue with only location address (no GPS)
- **THEN** the system creates the issue with null latitude/longitude
- **AND** generates a reference number
- **AND** accepts the submission successfully

#### Scenario: Submit issue without required fields
- **WHEN** a citizen attempts to submit an issue missing title, description, or category
- **THEN** the system rejects the submission
- **AND** returns validation errors for missing fields

### Requirement: Issue Categories
The system SHALL support predefined categories for classifying municipal issues: Road Maintenance, Street Lighting, Water and Sanitation, Waste Management, Public Safety, Parks, Noise, Illegal Dumping, Traffic Signals, and Other.

#### Scenario: Categorize issue as Road Maintenance
- **WHEN** a citizen reports a pothole and selects "Road Maintenance" category
- **THEN** the system categorizes the issue correctly
- **AND** enables filtering and reporting by this category

#### Scenario: Use Other category
- **WHEN** an issue doesn't fit predefined categories
- **THEN** the citizen can select "Other" category
- **AND** provide additional context in the description

### Requirement: Issue Priority Levels
The system SHALL support priority levels (Low, Medium, High, Critical) with Medium as the default for new issues.

#### Scenario: Default priority assignment
- **WHEN** a citizen submits a new issue without specifying priority
- **THEN** the system assigns priority "Medium" by default

#### Scenario: Staff adjusts priority to Critical
- **WHEN** staff reviews an issue and determines it requires urgent attention
- **THEN** staff can update the priority to "Critical"
- **AND** the system saves the priority change with audit trail

### Requirement: Issue Status Workflow
The system SHALL track issues through status workflow: New → Under Review → Assigned → In Progress → On Hold → Resolved → Closed, with Rejected as alternate terminal state.

#### Scenario: Progress issue from New to Resolved
- **WHEN** an issue is created (status: New)
- **THEN** staff can transition to "Under Review"
- **AND** then assign to staff member (status: "Assigned")
- **AND** staff begins work (status: "In Progress")
- **AND** staff completes work (status: "Resolved") with resolution date recorded
- **AND** finally close the issue (status: "Closed")

#### Scenario: Put issue On Hold
- **WHEN** an issue in progress encounters a blocker
- **THEN** staff can update status to "On Hold"
- **AND** provide notes explaining the hold reason

#### Scenario: Reject invalid issue
- **WHEN** staff determines an issue is invalid or outside jurisdiction
- **THEN** staff can update status to "Rejected"
- **AND** provide explanation in resolution notes

### Requirement: Issue Assignment
The system SHALL allow assignment of issues to staff members for resolution.

#### Scenario: Assign issue to staff member
- **WHEN** a manager assigns an issue to a staff member
- **THEN** the system records the staff user ID in `AssignedToUserId`
- **AND** updates status to "Assigned"
- **AND** notifies the assigned staff member

#### Scenario: Reassign issue to different staff member
- **WHEN** an issue needs to be reassigned
- **THEN** the system allows updating `AssignedToUserId`
- **AND** maintains audit trail of the reassignment

### Requirement: Issue Comments
The system SHALL support threaded comments on issues for updates, questions, and communication between citizens and staff.

#### Scenario: Citizen adds comment for clarification
- **WHEN** a citizen adds a comment to their submitted issue
- **THEN** the system creates an `IssueComment` record linked to the issue
- **AND** timestamps the comment
- **AND** associates it with the citizen user

#### Scenario: Staff provides update comment
- **WHEN** staff adds a progress update comment
- **THEN** the system creates the comment record
- **AND** notifies the issue submitter
- **AND** displays the comment in chronological order

### Requirement: Issue Attachments
The system SHALL allow photos and documents to be attached to issues as evidence.

#### Scenario: Attach photo to new issue
- **WHEN** a citizen uploads a photo while creating an issue
- **THEN** the system stores the file in blob storage
- **AND** creates an `IssueAttachment` record with file metadata
- **AND** links it to the issue

#### Scenario: Add attachment to existing issue
- **WHEN** a user adds an attachment to an existing issue
- **THEN** the system accepts the file upload
- **AND** creates an attachment record
- **AND** associates it with the issue

### Requirement: Issue Resolution
The system SHALL track issue resolution with completion date and resolution notes.

#### Scenario: Mark issue as resolved
- **WHEN** staff completes work on an issue
- **THEN** staff updates status to "Resolved"
- **AND** provides resolution notes describing the fix
- **AND** the system records `ResolvedDate` as current UTC timestamp

#### Scenario: Attempt to resolve without notes
- **WHEN** staff attempts to mark issue resolved without providing resolution notes
- **THEN** the system requires resolution notes
- **AND** prevents status change until notes are provided

### Requirement: Issue Search and Filtering
The system SHALL provide search and filtering capabilities by category, status, priority, location, and date range.

#### Scenario: Filter by status "In Progress"
- **WHEN** a user filters issues by status "In Progress"
- **THEN** the system returns only issues with that status
- **AND** respects tenant isolation

#### Scenario: Filter by category and priority
- **WHEN** a user filters for "Road Maintenance" issues with "High" priority
- **THEN** the system returns only issues matching both criteria
- **AND** orders results by creation date descending

#### Scenario: Search by reference number
- **WHEN** a user searches by issue reference number (e.g., "ISS-20241111-ABC123")
- **THEN** the system returns the exact matching issue if found

### Requirement: Location-Based Issue Reporting
The system SHALL capture and store GPS coordinates (latitude/longitude) for issues to enable mapping and geographic analysis.

#### Scenario: Submit issue with GPS coordinates
- **WHEN** a citizen submits an issue with latitude and longitude
- **THEN** the system validates the coordinates are within valid ranges
- **AND** stores both the address and GPS coordinates
- **AND** enables geographic searching and mapping

#### Scenario: Find issues near location
- **WHEN** searching for issues within a radius of GPS coordinates
- **THEN** the system returns issues within the specified distance
- **AND** orders by proximity

### Requirement: Reference Number Generation
The system SHALL generate unique, human-readable reference numbers for all issues in the format "ISS-YYYYMMDD-XXXXXXXX".

#### Scenario: Generate reference on creation
- **WHEN** a new issue is created
- **THEN** the system generates a reference starting with "ISS-"
- **AND** includes the current date in YYYYMMDD format
- **AND** appends an 8-character unique identifier
- **AND** ensures the reference is globally unique

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all issue records.

#### Scenario: Tenant isolation for issue queries
- **WHEN** a user from Tenant A queries issues
- **THEN** the system returns only issues from Tenant A
- **AND** does not expose issues from other tenants

## Data Model

### IssueReport Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **CitizenId**: Guid (Foreign Key, Required)
- **Title**: string (Required)
- **Description**: string (Required)
- **Category**: IssueCategory enum (Required)
- **Priority**: IssuePriority enum (Default: Medium)
- **Status**: IssueStatus enum (Default: New)
- **LocationAddress**: string (Optional)
- **Latitude**: double? (Optional)
- **Longitude**: double? (Optional)
- **ReferenceNumber**: string (Generated, Unique)
- **ResolvedDate**: DateTime? (Nullable)
- **AssignedToUserId**: Guid? (Nullable)
- **ResolutionNotes**: string (Optional)
- **Attachments**: ICollection<IssueAttachment>
- **Comments**: ICollection<IssueComment>
- Audit fields (CreatorId, CreationTime, LastModifierId, LastModificationTime, DeleterId, DeletionTime, IsDeleted)

### IssueCategory Enum
- RoadMaintenance
- StreetLighting
- WaterAndSanitation
- WasteManagement
- PublicSafety
- Parks
- Noise
- IllegalDumping
- TrafficSignals
- Other

### IssuePriority Enum
- Low
- Medium
- High
- Critical

### IssueStatus Enum
- New
- UnderReview
- Assigned
- InProgress
- OnHold
- Resolved
- Closed
- Rejected

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/IssueReportAppService.cs`
- Implements `CrudAppService<IssueReport, IssueReportDto, Guid, IssueReportDto, CreateUpdateIssueReportDto>`
- Entity located in `src/CitizensPortal.Domain/Entities/IssueReport.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/IssueEnums.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/IssueReportController.cs`
- Endpoints: `GET/POST/PUT/DELETE /api/issue-reports`
