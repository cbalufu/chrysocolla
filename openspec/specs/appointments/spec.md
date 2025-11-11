# Appointments

## Overview
The Appointments capability enables citizens to book appointments with municipal departments for application submission, document collection, consultations, and inspections.

## Requirements

### Requirement: Appointment Booking
The system SHALL allow citizens to book appointments with department, date, time, and purpose.

#### Scenario: Book appointment
- **WHEN** citizen books appointment for document collection
- **THEN** the system creates appointment with status "Scheduled"
- **AND** links to citizen and department
- **AND** reserves the time slot
- **AND** sends confirmation notification

### Requirement: Appointment Types
The system SHALL support appointment types: Application Submission, Document Collection, Consultation, Inspection.

#### Scenario: Book inspection appointment
- **WHEN** citizen books "Inspection" appointment
- **THEN** the system creates appointment with inspection type
- **AND** applies inspection-specific workflows

### Requirement: Appointment Status
The system SHALL track appointments through statuses: Scheduled, Confirmed, Completed, Cancelled, NoShow.

#### Scenario: Confirm appointment
- **WHEN** department confirms scheduled appointment
- **THEN** the system updates status to "Confirmed"
- **AND** sends confirmation to citizen

#### Scenario: Mark no-show
- **WHEN** citizen doesn't attend appointment
- **THEN** staff marks status as "NoShow"
- **AND** releases the time slot

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all appointment records.

#### Scenario: Tenant isolation
- **WHEN** querying appointments from Tenant A
- **THEN** the system returns only appointments from Tenant A

## Data Model

### Appointment Entity
- **Id**: Guid
- **TenantId**: Guid?
- **CitizenId**: Guid (FK)
- **DepartmentId**: Guid (FK)
- **AppointmentType**: AppointmentType enum
- **Status**: AppointmentStatus enum
- **AppointmentDate**: DateTime
- **Purpose**: string
- **Notes**: string
- Audit fields

### AppointmentType Enum
- ApplicationSubmission, DocumentCollection, Consultation, Inspection

### AppointmentStatus Enum
- Scheduled, Confirmed, Completed, Cancelled, NoShow

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/AppointmentAppService.cs`
- Entity in `src/CitizensPortal.Domain/Entities/Appointment.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/AppointmentEnums.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/AppointmentController.cs`
- Endpoints: `GET/POST/PUT/DELETE /api/appointments`
