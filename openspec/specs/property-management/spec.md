# Property Management

## Overview
The Property Management capability maintains a registry of properties within the municipality including residential, commercial, industrial, agricultural, and vacant land with ownership, valuation, and zoning information.

## Requirements

### Requirement: Property Registration
The system SHALL allow registration of properties with type, address, GPS coordinates, and owner information.

#### Scenario: Register residential property
- **WHEN** registering a new residential property
- **THEN** the system creates property record with unique reference number
- **AND** links to owner citizen
- **AND** records property type as "Residential"
- **AND** captures address and GPS coordinates

### Requirement: Property Types
The system SHALL support property types: Residential, Commercial, Industrial, Agricultural, VacantLand.

#### Scenario: Register commercial property
- **WHEN** creating property with type "Commercial"
- **THEN** the system applies commercial property rules
- **AND** enables commercial-specific billing

### Requirement: Property Valuation
The system SHALL track market value and assessment value for properties.

#### Scenario: Record property valuation
- **WHEN** updating property market value
- **THEN** the system stores the valuation amount
- **AND** maintains valuation history
- **AND** uses value for rates calculation

### Requirement: Property Status
The system SHALL track property status: Active, Inactive, UnderReview.

#### Scenario: Set property inactive
- **WHEN** property is no longer in use
- **THEN** the system updates status to "Inactive"
- **AND** suspends active billing

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all property records.

#### Scenario: Tenant isolation
- **WHEN** querying properties from Tenant A
- **THEN** the system returns only properties from Tenant A

## Data Model

### Property Entity
- **Id**: Guid
- **TenantId**: Guid?
- **PropertyReference**: string (Unique)
- **PropertyType**: PropertyType enum
- **Status**: PropertyStatus enum
- **Address**: string
- **City**: string
- **PostalCode**: string
- **Latitude**: double?
- **Longitude**: double?
- **ZoningInfo**: string
- **MarketValue**: decimal?
- **AssessmentValue**: decimal?
- **OwnerId**: Guid (Citizen FK)
- Audit fields

### PropertyType Enum
- Residential, Commercial, Industrial, Agricultural, VacantLand

### PropertyStatus Enum
- Active, Inactive, UnderReview

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/PropertyAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Property.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/PropertyEnums.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/PropertyController.cs`
- Endpoints: `GET/POST/PUT/DELETE /api/properties`
