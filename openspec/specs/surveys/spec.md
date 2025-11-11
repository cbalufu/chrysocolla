# Surveys

## Overview
The Surveys capability enables municipalities to create and conduct public surveys/polls with multiple question types, anonymous responses, and response analytics.

## Requirements

### Requirement: Survey Creation
The system SHALL allow creation of surveys with title, description, start/end dates, and multiple questions.

#### Scenario: Create public survey
- **WHEN** creating new survey about park improvements
- **THEN** the system creates survey with status "Draft"
- **AND** sets start and end dates
- **AND** allows adding multiple questions
- **AND** configures anonymous/non-anonymous setting

### Requirement: Question Types
The system SHALL support question types: MultipleChoice, SingleChoice, Text, Rating.

#### Scenario: Add multiple choice question
- **WHEN** adding question "Which park facilities would you like to see?"
- **THEN** the system creates question with type "MultipleChoice"
- **AND** stores options as JSON array
- **AND** allows multiple answer selections

#### Scenario: Add rating question
- **WHEN** adding question with type "Rating"
- **THEN** the system configures rating scale (e.g., 1-5)
- **AND** captures numeric ratings from respondents

### Requirement: Survey Status
The system SHALL track surveys through statuses: Draft, Active, Closed, Archived.

#### Scenario: Activate survey
- **WHEN** survey start date arrives
- **THEN** the system updates status to "Active"
- **AND** makes survey available to citizens

#### Scenario: Close survey
- **WHEN** survey end date passes
- **THEN** the system updates status to "Closed"
- **AND** prevents new responses
- **AND** enables results viewing

### Requirement: Anonymous Responses
The system SHALL support anonymous surveys where responses are not linked to citizens.

#### Scenario: Submit anonymous response
- **WHEN** survey is configured as anonymous
- **THEN** citizen can respond without identity tracking
- **AND** response is created with null CitizenId

### Requirement: Response Submission
The system SHALL allow citizens to submit responses with answers to all required questions.

#### Scenario: Submit complete response
- **WHEN** citizen completes all required questions
- **THEN** the system creates `SurveyResponse` record
- **AND** creates `SurveyAnswer` for each question
- **AND** increments total response count
- **AND** timestamps submission

#### Scenario: Multiple response control
- **WHEN** survey allows multiple responses
- **THEN** citizen can submit multiple times
- **AND** system accepts additional responses

#### Scenario: Single response enforcement
- **WHEN** survey disallows multiple responses
- **THEN** system prevents citizen from submitting again
- **AND** shows existing response for editing if allowed

### Requirement: Response Analytics
The system SHALL provide response statistics and analytics for completed surveys.

#### Scenario: View survey results
- **WHEN** viewing closed survey results
- **THEN** the system shows total responses
- **AND** aggregates answers by question
- **AND** calculates percentages for choice questions

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying surveys from Tenant A
- **THEN** the system returns only surveys from Tenant A

## Data Model

### Survey Entity
- **Id**: Guid
- **TenantId**: Guid?
- **Title**: string
- **Description**: string
- **Status**: SurveyStatus enum
- **StartDate**: DateTime
- **EndDate**: DateTime
- **IsAnonymous**: bool
- **AllowMultipleResponses**: bool
- **TotalResponses**: int
- **Questions**: ICollection<SurveyQuestion>
- **Responses**: ICollection<SurveyResponse>
- Audit fields

### SurveyQuestion Entity
- **Id**: Guid
- **SurveyId**: Guid (FK)
- **QuestionText**: string
- **Type**: QuestionType enum
- **OrderIndex**: int
- **IsRequired**: bool
- **Options**: string (JSON for choice questions)
- **Answers**: ICollection<SurveyAnswer>

### SurveyResponse Entity
- **Id**: Guid
- **SurveyId**: Guid (FK)
- **CitizenId**: Guid? (Nullable for anonymous)
- **SubmittedDate**: DateTime
- **Answers**: ICollection<SurveyAnswer>

### SurveyAnswer Entity
- **Id**: Guid
- **SurveyResponseId**: Guid (FK)
- **SurveyQuestionId**: Guid (FK)
- **AnswerText**: string

### SurveyStatus Enum
- Draft, Active, Closed, Archived

### QuestionType Enum
- MultipleChoice, SingleChoice, Text, Rating

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/SurveyAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Survey.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/CommunityEnums.cs`
- HTTP API: No controller yet (needs to be created)
- JSON storage for question options provides flexibility
