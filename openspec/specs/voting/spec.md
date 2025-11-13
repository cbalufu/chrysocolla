# Voting

## Overview
The Voting capability enables municipalities to conduct community voting on initiatives, proposals, and decisions with one-vote-per-citizen enforcement and result tallying.

## Requirements

### Requirement: Vote Creation
The system SHALL allow creation of community votes with title, description, options, and voting period.

#### Scenario: Create initiative vote
- **WHEN** creating vote for community initiative
- **THEN** the system creates vote with status "Upcoming"
- **AND** defines voting start and end dates
- **AND** sets voting options (e.g., Yes/No or multiple choices)
- **AND** initializes vote counts to zero

### Requirement: Vote Status
The system SHALL track votes through statuses: Upcoming, Active, Closed, Published.

#### Scenario: Activate vote
- **WHEN** voting start date arrives
- **THEN** the system updates status to "Active"
- **AND** enables citizen voting

#### Scenario: Close vote and publish results
- **WHEN** voting end date passes
- **THEN** the system updates status to "Closed"
- **AND** prevents additional votes
- **AND** allows result publication (status: "Published")

### Requirement: Vote Submission
The system SHALL allow citizens to cast votes with one-vote-per-citizen enforcement.

#### Scenario: Cast vote
- **WHEN** citizen casts vote on active voting
- **THEN** the system records vote choice
- **AND** links to citizen
- **AND** timestamps the vote
- **AND** increments vote count for selected option

#### Scenario: Prevent duplicate voting
- **WHEN** citizen attempts to vote again on same issue
- **THEN** the system detects existing vote
- **AND** prevents duplicate submission
- **AND** shows existing vote

#### Scenario: Change vote before close
- **WHEN** citizen wants to change their vote while voting is active
- **THEN** the system allows updating vote choice
- **AND** decrements old option count
- **AND** increments new option count

### Requirement: Vote Options
The system SHALL support multiple voting options with flexible configuration (Yes/No, multiple choices, ranked choice).

#### Scenario: Yes/No vote
- **WHEN** creating simple Yes/No vote
- **THEN** the system configures two options
- **AND** tallies results for each

#### Scenario: Multiple choice vote
- **WHEN** creating vote with multiple options
- **THEN** the system allows defining custom options
- **AND** tallies votes for each option

### Requirement: Result Tallying
The system SHALL calculate and display vote results with counts and percentages.

#### Scenario: View results
- **WHEN** viewing closed vote results
- **THEN** the system shows total votes cast
- **AND** displays vote count for each option
- **AND** calculates percentage for each option

### Requirement: Vote Anonymity
The system SHALL protect voter privacy while preventing duplicate votes.

#### Scenario: Anonymous result display
- **WHEN** viewing vote results
- **THEN** the system shows aggregated counts
- **AND** does not reveal individual citizen votes publicly
- **AND** only records vote existence for duplicate prevention

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying votes from Tenant A
- **THEN** the system returns only votes from Tenant A

## Data Model

### Vote Entity
- **Id**: Guid
- **TenantId**: Guid?
- **Title**: string
- **Description**: string
- **Status**: VoteStatus enum
- **StartDate**: DateTime
- **EndDate**: DateTime
- **Options**: string (JSON array of vote options)
- **Results**: string (JSON object with vote counts)
- **TotalVotes**: int
- Audit fields

### VoteCast Entity
- **Id**: Guid
- **VoteId**: Guid (FK)
- **CitizenId**: Guid (FK)
- **SelectedOption**: string
- **CastDate**: DateTime

### VoteStatus Enum
- Upcoming, Active, Closed, Published

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/VoteAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Vote.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/CommunityEnums.cs`
- HTTP API: No controller yet (needs to be created)
- JSON storage for flexible vote options
- One-vote-per-citizen enforced via unique constraint on (VoteId, CitizenId)
