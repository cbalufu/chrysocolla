# Citizen Management

## Overview
The Citizen Management capability provides registration, profile management, and verification for citizens/ratepayers who use municipal services.

## Requirements

### Requirement: Citizen Registration
The system SHALL allow new citizens to register with their personal information including first name, last name, email, phone number, and national ID.

#### Scenario: Successful registration
- **WHEN** a new citizen provides valid personal information (first name, last name, email, phone number, national ID)
- **THEN** the system creates a citizen record with `IsVerified` set to false
- **AND** assigns a unique GUID identifier
- **AND** returns the created citizen record

#### Scenario: Duplicate email registration
- **WHEN** a citizen attempts to register with an email that already exists in the system
- **THEN** the system rejects the registration
- **AND** returns a validation error indicating the email is already in use

#### Scenario: Invalid national ID
- **WHEN** a citizen provides an invalid or missing national ID during registration
- **THEN** the system rejects the registration
- **AND** returns a validation error

### Requirement: Citizen Profile Management
The system SHALL allow citizens to view and update their profile information including address, city, postal code, and property reference.

#### Scenario: Update address information
- **WHEN** a verified citizen updates their address, city, or postal code
- **THEN** the system saves the updated information
- **AND** maintains audit trail of the modification
- **AND** returns the updated profile

#### Scenario: Update property reference
- **WHEN** a citizen updates their property reference
- **THEN** the system validates the property reference exists
- **AND** links the citizen to the property
- **AND** saves the updated information

### Requirement: Citizen Verification
The system SHALL support verification of citizen identities to grant access to services requiring verified status.

#### Scenario: Verify citizen identity
- **WHEN** an administrator verifies a citizen's identity
- **THEN** the system sets `IsVerified` to true
- **AND** records the `VerificationDate` as current UTC timestamp
- **AND** enables access to services requiring verified status

#### Scenario: Unverified citizen access restriction
- **WHEN** an unverified citizen attempts to access services requiring verification
- **THEN** the system denies access
- **AND** returns an error indicating verification is required

### Requirement: Citizen Lookup
The system SHALL provide search and retrieval capabilities for citizen records based on various criteria.

#### Scenario: Search by email
- **WHEN** searching for a citizen by email address
- **THEN** the system returns the matching citizen record if found
- **AND** respects tenant isolation (only returns citizens from the same tenant)

#### Scenario: Search by national ID
- **WHEN** searching for a citizen by national ID
- **THEN** the system returns the matching citizen record if found
- **AND** respects tenant isolation

#### Scenario: List all citizens with pagination
- **WHEN** requesting a list of all citizens with pagination parameters
- **THEN** the system returns a paginated list of citizens
- **AND** includes total count for pagination
- **AND** respects tenant isolation

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all citizen records.

#### Scenario: Tenant A cannot access Tenant B citizens
- **WHEN** a user from Tenant A queries for citizens
- **THEN** the system returns only citizens belonging to Tenant A
- **AND** does not expose any citizen data from other tenants

### Requirement: Audit Trail
The system SHALL maintain a complete audit trail for all citizen record operations.

#### Scenario: Track citizen creation
- **WHEN** a new citizen record is created
- **THEN** the system records the creator user ID
- **AND** records the creation timestamp in UTC
- **AND** records the tenant ID

#### Scenario: Track citizen modifications
- **WHEN** a citizen record is modified
- **THEN** the system records the modifier user ID
- **AND** records the modification timestamp in UTC
- **AND** maintains the original creation information

#### Scenario: Track citizen deletion
- **WHEN** a citizen record is soft-deleted
- **THEN** the system records the deleter user ID
- **AND** records the deletion timestamp in UTC
- **AND** maintains the record for audit purposes (soft delete)

### Requirement: CRUD Operations
The system SHALL provide complete CRUD (Create, Read, Update, Delete) operations for citizen records via the application service.

#### Scenario: Create new citizen
- **WHEN** creating a citizen with valid data
- **THEN** the system generates a new GUID
- **AND** validates all required fields
- **AND** saves the record to the database
- **AND** returns the created citizen DTO

#### Scenario: Get citizen by ID
- **WHEN** retrieving a citizen by GUID
- **THEN** the system returns the citizen DTO if found and belongs to current tenant
- **AND** returns not found error if ID doesn't exist or belongs to different tenant

#### Scenario: Update existing citizen
- **WHEN** updating a citizen with valid data
- **THEN** the system validates the update permissions
- **AND** applies the changes
- **AND** maintains audit information
- **AND** returns the updated citizen DTO

#### Scenario: Delete citizen
- **WHEN** deleting a citizen by ID
- **THEN** the system performs a soft delete (sets IsDeleted flag)
- **AND** maintains the record for audit purposes
- **AND** excludes the citizen from normal queries

## Data Model

### Citizen Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **FirstName**: string (Required)
- **LastName**: string (Required)
- **Email**: string (Required, Unique)
- **PhoneNumber**: string (Required)
- **NationalId**: string (Required)
- **Address**: string (Optional)
- **City**: string (Optional)
- **PostalCode**: string (Optional)
- **PropertyReference**: string (Optional)
- **IsVerified**: bool (Default: false)
- **VerificationDate**: DateTime? (Nullable)
- **CreatorId**: Guid? (Audit)
- **CreationTime**: DateTime (Audit)
- **LastModifierId**: Guid? (Audit)
- **LastModificationTime**: DateTime? (Audit)
- **DeleterId**: Guid? (Audit)
- **DeletionTime**: DateTime? (Audit)
- **IsDeleted**: bool (Soft delete)

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/CitizenAppService.cs`
- Implements `CrudAppService<Citizen, CitizenDto, Guid, CitizenDto, CreateUpdateCitizenDto>`
- DTOs defined in `src/CitizensPortal.Application.Contracts/DTOs/`
- Entity located in `src/CitizensPortal.Domain/Entities/Citizen.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/CitizenController.cs`
- Endpoints: `GET/POST/PUT/DELETE /api/citizens`
