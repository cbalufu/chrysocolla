# Emergency Alerts

## Overview
The Emergency Alerts capability enables municipalities to broadcast emergency notifications to citizens based on alert type, severity, and geographic location for natural disasters, fires, health emergencies, security threats, and utility outages.

## Requirements

### Requirement: Emergency Alert Broadcasting
The system SHALL allow authorized users to create and broadcast emergency alerts with type, severity, message, and affected area.

#### Scenario: Broadcast flood warning
- **WHEN** authorized user creates emergency alert for flood warning
- **THEN** the system creates alert with type "NaturalDisaster" and severity "Warning"
- **AND** defines affected geographic area
- **AND** broadcasts to all citizens in affected radius
- **AND** sends via email and in-app notifications

### Requirement: Alert Types
The system SHALL support alert types: NaturalDisaster, Fire, HealthEmergency, Security, UtilityOutage, PublicSafety.

#### Scenario: Utility outage alert
- **WHEN** creating alert for power outage
- **THEN** the system sets type to "UtilityOutage"
- **AND** includes estimated restoration time

### Requirement: Severity Levels
The system SHALL support severity levels: Info, Warning, Critical.

#### Scenario: Critical alert
- **WHEN** creating critical severity alert
- **THEN** the system marks as highest priority
- **AND** uses urgent notification styling
- **AND** requires immediate attention

### Requirement: Geographic Targeting
The system SHALL support location-based alert targeting using GPS coordinates and radius.

#### Scenario: Alert specific area
- **WHEN** creating alert with GPS center point and 5km radius
- **THEN** the system identifies all citizens within radius
- **AND** sends alerts only to affected citizens

#### Scenario: Municipality-wide alert
- **WHEN** creating alert with no specific location
- **THEN** the system broadcasts to all citizens in the tenant

### Requirement: Alert Acknowledgment
The system SHALL track citizen acknowledgment of alerts.

#### Scenario: Citizen acknowledges alert
- **WHEN** citizen views and acknowledges alert
- **THEN** the system records acknowledgment timestamp
- **AND** marks alert as read for that citizen

### Requirement: Alert History
The system SHALL maintain complete history of all emergency alerts.

#### Scenario: View past alerts
- **WHEN** querying alert history
- **THEN** the system returns alerts ordered by creation date
- **AND** includes alert status and reach statistics

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** broadcasting alert in Tenant A
- **THEN** the system notifies only citizens from Tenant A

## Data Model

### EmergencyAlert Entity
- **Id**: Guid
- **TenantId**: Guid?
- **AlertType**: EmergencyAlertType enum
- **Severity**: AlertSeverity enum
- **Title**: string
- **Message**: string
- **Latitude**: double? (Center point)
- **Longitude**: double? (Center point)
- **RadiusKm**: double? (Affected radius)
- **IsActive**: bool
- **ExpiryDate**: DateTime?
- **AcknowledgmentCount**: int
- Audit fields

### EmergencyAlertType Enum
- NaturalDisaster, Fire, HealthEmergency, Security, UtilityOutage, PublicSafety

### AlertSeverity Enum
- Info, Warning, Critical

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/EmergencyAlertAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/EmergencyAlert.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/EmergencyEnums.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/EmergencyAlertController.cs`
- Endpoints: `GET/POST/PUT/DELETE /api/emergency-alerts`
- GPS-based radius calculations for targeted alerts
