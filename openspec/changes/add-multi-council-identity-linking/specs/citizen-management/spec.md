## ADDED Requirements

### Requirement: National Identifier Registration
The system SHALL allow citizens to register with a national identifier (National ID, TIN, or Passport number) to enable cross-council identity linking.

#### Scenario: Citizen registers with national ID
- **WHEN** a citizen completes registration with a valid national ID number
- **THEN** the system creates a federated profile linked to their national ID
- **AND** the national ID is encrypted and stored with a hashed lookup index
- **AND** the citizen's verification status is set to "Pending"

#### Scenario: Duplicate national ID registration attempt
- **WHEN** a citizen attempts to register with a national ID already in the system
- **THEN** the system prevents duplicate registration
- **AND** returns an error indicating the ID is already registered
- **AND** provides instructions for account recovery

#### Scenario: Registration without national ID
- **WHEN** a citizen registers without providing a national ID
- **THEN** the system allows registration with email/phone only
- **AND** the citizen can add their national ID later through profile settings
- **AND** the citizen cannot access cross-council features until ID is linked

### Requirement: Federated Profile Management
The system SHALL maintain a federated citizen profile at the host tenant level that links to tenant-specific citizen records across multiple councils.

#### Scenario: Retrieve federated profile by national ID
- **WHEN** the system looks up a citizen by national ID hash
- **THEN** it returns the federated profile containing all linked tenant-citizen associations
- **AND** the profile includes verification status and method
- **AND** the profile lists all councils the citizen is linked to

#### Scenario: Link new council to existing federated profile
- **WHEN** a verified citizen registers with a new council using their national ID
- **THEN** the system automatically links the new tenant-citizen record to their federated profile
- **AND** the citizen gains access to cross-council aggregated views
- **AND** an audit log entry is created for the linking event

#### Scenario: Unlink council from federated profile
- **WHEN** a citizen requests to unlink a specific council from their profile
- **THEN** the system removes the tenant-citizen association from the federated profile
- **AND** the tenant-specific data remains in the council's tenant (data retained for compliance)
- **AND** the citizen loses access to that council's data in aggregated views
- **AND** an audit log entry is created for the unlinking event

### Requirement: Cross-Council Data Aggregation
The system SHALL provide aggregated views of citizen data across all linked councils while maintaining tenant data isolation and authorization.

#### Scenario: View aggregated properties across councils
- **WHEN** a verified citizen with multiple linked councils requests their property list
- **THEN** the system queries each linked tenant for the citizen's properties
- **AND** returns a unified list with council identification for each property
- **AND** respects authorization rules for each tenant
- **AND** excludes data from councils where the citizen lacks permission

#### Scenario: View aggregated bills across councils
- **WHEN** a verified citizen requests their outstanding bills
- **THEN** the system retrieves bills from all linked council tenants
- **AND** aggregates them with council identification and totals per council
- **AND** supports sorting and filtering across the aggregated dataset

#### Scenario: View aggregated issues across councils
- **WHEN** a verified citizen views their issue reports
- **THEN** the system fetches issues from all linked councils
- **AND** displays them in a unified timeline with council identification
- **AND** allows filtering by council, status, and category

#### Scenario: Performance of aggregated views with many councils
- **WHEN** a citizen linked to 10+ councils requests aggregated data
- **THEN** the system completes the query within 500ms (p95)
- **AND** uses caching to improve performance for repeated queries
- **AND** implements pagination to limit data transfer

### Requirement: National ID Update and Re-verification
The system SHALL support updating a citizen's national identifier with mandatory re-verification to maintain data integrity.

#### Scenario: Citizen requests national ID update
- **WHEN** a verified citizen requests to update their national ID (e.g., due to marriage, legal name change)
- **THEN** the system sets their verification status to "Pending"
- **AND** requires the citizen to complete identity verification again
- **AND** maintains historical link to previous national ID for audit purposes
- **AND** notifies all linked councils of the pending verification

#### Scenario: Re-verification approved after ID update
- **WHEN** a council admin approves the re-verification
- **THEN** the system updates the federated profile with the new national ID
- **AND** restores the citizen's verified status
- **AND** re-enables cross-council features
- **AND** creates audit log entries for the ID change

#### Scenario: Re-verification rejected after ID update
- **WHEN** a council admin rejects the re-verification
- **THEN** the system retains the original national ID
- **AND** notifies the citizen of the rejection with reason
- **AND** allows the citizen to retry verification

### Requirement: Consent Management for Cross-Council Data Sharing
The system SHALL obtain and manage explicit consent from citizens for cross-council data sharing to comply with data privacy regulations.

#### Scenario: Citizen provides consent during registration
- **WHEN** a citizen registers with a national ID
- **THEN** the system presents a consent form explaining cross-council data sharing
- **AND** requires explicit acceptance before enabling federation features
- **AND** records the consent decision with timestamp in the federated profile

#### Scenario: Citizen revokes cross-council consent
- **WHEN** a verified citizen revokes their cross-council data sharing consent
- **THEN** the system disables aggregated views and cross-council features
- **AND** retains the federated profile with consent status "Revoked"
- **AND** the citizen can still access individual council data separately
- **AND** notifies all linked councils of the consent revocation

#### Scenario: Citizen re-grants consent after revocation
- **WHEN** a citizen who previously revoked consent grants it again
- **THEN** the system re-enables cross-council features
- **AND** updates the consent status with new timestamp
- **AND** creates audit log entry for consent change

### Requirement: Data Subject Access Request (DSAR) for Federated Profile
The system SHALL support data subject access requests allowing citizens to export all their federated profile data for POPIA compliance.

#### Scenario: Citizen requests federated profile data export
- **WHEN** a citizen submits a DSAR for their federated profile
- **THEN** the system generates a comprehensive report including:
  - Federated profile details (masked national ID)
  - List of all linked councils
  - Verification history and status
  - Consent history
  - Audit logs of profile access and modifications
- **AND** delivers the report in machine-readable format (JSON) and human-readable format (PDF)
- **AND** completes the export within 30 days (regulatory requirement)

#### Scenario: Citizen requests federated profile deletion
- **WHEN** a citizen requests deletion of their federated profile
- **THEN** the system unlinks all council associations
- **AND** anonymizes the national ID data (keeps hash for fraud prevention)
- **AND** notifies all linked councils of the deletion
- **AND** retains minimal audit data for compliance (7 years)
