## ADDED Requirements

### Requirement: Identity Document Upload
The system SHALL allow citizens to upload identity documents (National ID, TIN certificate, or Passport) for verification.

#### Scenario: Citizen uploads identity document photos
- **WHEN** a citizen uploads photos of their identity document (front and back)
- **THEN** the system validates the file format (JPEG, PNG, PDF)
- **AND** validates the file size (max 5MB per file)
- **AND** stores the documents securely in blob storage with expiration policy
- **AND** creates a verification request record with status "Pending Review"
- **AND** generates a reference number for tracking

#### Scenario: Invalid document format uploaded
- **WHEN** a citizen attempts to upload a document in an unsupported format
- **THEN** the system rejects the upload
- **AND** returns an error message specifying supported formats
- **AND** allows the citizen to retry with correct format

#### Scenario: Document file size exceeds limit
- **WHEN** a citizen uploads a document larger than 5MB
- **THEN** the system rejects the upload
- **AND** returns an error message with size limit
- **AND** suggests image compression or PDF optimization

### Requirement: Self-Service Verification Workflow
The system SHALL provide a self-service identity verification workflow where citizens submit documents online for council admin review.

#### Scenario: Citizen submits verification request
- **WHEN** a citizen completes document upload and submits verification request
- **THEN** the system creates a verification request assigned to the council's verification queue
- **AND** sends confirmation email to the citizen with reference number
- **AND** sets estimated review time (24-48 hours)
- **AND** notifies council admins of the pending request

#### Scenario: Council admin reviews and approves verification
- **WHEN** a council admin reviews documents and approves the verification
- **THEN** the system updates the citizen's federated profile verification status to "Verified"
- **AND** sets verification method to "SelfService"
- **AND** records the verifying council and admin
- **AND** sends approval notification to the citizen
- **AND** enables cross-council features for the citizen
- **AND** creates audit log entry with admin ID and timestamp

#### Scenario: Council admin reviews and rejects verification
- **WHEN** a council admin rejects the verification with a reason
- **THEN** the system updates the verification request status to "Rejected"
- **AND** sends rejection notification to the citizen with reason and retry instructions
- **AND** allows the citizen to resubmit after 24-hour cooling-off period
- **AND** flags the profile if rejection count exceeds threshold (3 rejections)

#### Scenario: Verification request timeout
- **WHEN** a verification request remains unreviewed for 7 days
- **THEN** the system sends reminder notification to council admins
- **AND** escalates to council manager after 14 days
- **AND** auto-expires the request after 30 days
- **AND** allows the citizen to resubmit

### Requirement: In-Person Verification Workflow
The system SHALL support in-person identity verification at council offices where staff can immediately verify and approve citizen identities.

#### Scenario: Council staff verifies citizen in person
- **WHEN** a council staff member verifies a citizen's identity with physical documents
- **THEN** the staff member can approve verification directly in the system
- **AND** the system updates the citizen's federated profile verification status to "Verified"
- **AND** sets verification method to "InPerson"
- **AND** records the verifying council, staff member, and timestamp
- **AND** immediately enables cross-council features for the citizen
- **AND** no document upload or waiting period required

#### Scenario: In-person verification at non-primary council
- **WHEN** a citizen visits a council office that is not their primary registration council
- **THEN** the staff can still verify the identity
- **AND** the verification applies to the citizen's federated profile (all councils)
- **AND** the verifying council is recorded
- **AND** the citizen can link properties in any council after verification

#### Scenario: Staff requires additional documentation
- **WHEN** council staff determines submitted documents are insufficient
- **THEN** the staff can request specific additional documents
- **AND** the system creates a notification to the citizen listing required documents
- **AND** the verification remains in "Pending" status until documents provided
- **AND** the citizen can upload additional documents or return in person

### Requirement: Document Security and Retention
The system SHALL secure identity documents with encryption and implement retention policies compliant with data protection regulations.

#### Scenario: Document storage and encryption
- **WHEN** an identity document is uploaded
- **THEN** the system stores it in encrypted blob storage
- **AND** generates a secure, time-limited access URL
- **AND** restricts access to authorized council admins only
- **AND** logs all document access attempts with user ID and timestamp

#### Scenario: Document expiration after verification
- **WHEN** a verification request is approved or rejected
- **THEN** the system retains documents for 90 days for audit purposes
- **AND** automatically deletes documents after 90 days
- **AND** updates the verification record to indicate documents deleted
- **AND** retains verification metadata (who, when, outcome) for 7 years

#### Scenario: Citizen requests document deletion before expiration
- **WHEN** a verified citizen requests early deletion of their uploaded documents
- **THEN** the system deletes the documents immediately
- **AND** retains verification metadata only
- **AND** logs the deletion request with citizen consent

### Requirement: Verification Queue Management
The system SHALL provide council admins with tools to manage and prioritize identity verification requests.

#### Scenario: Admin views verification queue
- **WHEN** a council admin accesses the verification queue
- **THEN** the system displays all pending requests for their council
- **AND** shows request age, citizen name, document count, and priority
- **AND** supports sorting by submission date, priority, and status
- **AND** supports filtering by verification status and citizen search

#### Scenario: Admin assigns verification to themselves
- **WHEN** an admin selects a verification request
- **THEN** the system assigns the request to that admin
- **AND** prevents other admins from reviewing the same request (optimistic locking)
- **AND** starts a review timer for SLA tracking
- **AND** allows admin to view uploaded documents and citizen details

#### Scenario: Bulk verification approval for migrated citizens
- **WHEN** a council is migrating existing citizens with pre-verified identities
- **THEN** an admin can upload a CSV file with verified national IDs
- **AND** the system batch-creates federated profiles with "Verified" status
- **AND** sets verification method to "BulkImport"
- **AND** generates a reconciliation report showing successful and failed imports

### Requirement: Verification Fraud Detection
The system SHALL implement fraud detection mechanisms to identify suspicious verification attempts.

#### Scenario: Multiple verification attempts with same document
- **WHEN** the system detects the same document hash submitted for different national IDs
- **THEN** the system flags all related verification requests as suspicious
- **AND** notifies council admins of potential fraud
- **AND** requires manual review and additional verification steps
- **AND** logs the incident for security audit

#### Scenario: Rapid verification submissions from same IP
- **WHEN** the system detects multiple verification requests from the same IP within a short time
- **THEN** the system applies rate limiting (max 3 requests per day per IP)
- **AND** flags the requests for review
- **AND** may require additional verification such as phone call or in-person visit

#### Scenario: High rejection rate for citizen
- **WHEN** a citizen has been rejected 3+ times
- **THEN** the system requires in-person verification for future attempts
- **AND** disables self-service verification for that citizen
- **AND** notifies council security team
- **AND** logs the pattern for investigation

### Requirement: Verification Analytics and Reporting
The system SHALL provide analytics and reports on identity verification activities for councils and system administrators.

#### Scenario: Council admin views verification metrics
- **WHEN** a council admin requests verification metrics
- **THEN** the system displays:
  - Total verification requests (pending, approved, rejected)
  - Average review time
  - Approval/rejection rates
  - Top rejection reasons
  - Trend analysis over time
- **AND** supports filtering by date range and verification method

#### Scenario: System admin views cross-council verification report
- **WHEN** a system admin requests platform-wide verification metrics
- **THEN** the system aggregates data across all councils
- **AND** displays comparative metrics per council
- **AND** identifies outliers (unusually high rejection rates, review times)
- **AND** supports export to CSV/PDF for executive reporting

#### Scenario: Audit trail for verified citizen
- **WHEN** an authorized user requests the verification history for a citizen
- **THEN** the system displays complete verification timeline including:
  - All verification attempts with outcomes
  - Documents submitted (metadata only, not documents)
  - Reviewing admins and councils
  - Status changes with timestamps
  - Consent decisions
- **AND** respects authorization rules (citizen can view own, admins can view their council's)
