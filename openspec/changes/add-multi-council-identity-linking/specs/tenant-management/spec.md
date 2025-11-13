## ADDED Requirements

### Requirement: Council Self-Registration
The system SHALL allow councils to register themselves as tenants through a self-service workflow requiring system administrator approval.

#### Scenario: Council submits registration request
- **WHEN** a council representative submits a registration form with:
  - Council name and official registration number
  - Geographic region and service area
  - Primary contact information (name, email, phone)
  - Administrator details (name, email)
  - Supporting documents (council charter, authorization letter)
- **THEN** the system creates a tenant in "PendingApproval" status
- **AND** stores the registration details and documents
- **AND** sends confirmation email to the representative with reference number
- **AND** notifies system administrators of the pending request
- **AND** generates a registration reference number (e.g., REG-20241113-ABCD)

#### Scenario: Duplicate council registration attempt
- **WHEN** a council attempts to register with a name or registration number already in the system
- **THEN** the system prevents duplicate registration
- **AND** returns an error indicating the council already exists
- **AND** provides contact information for system administrators

#### Scenario: Invalid registration data submitted
- **WHEN** a council submits incomplete or invalid registration data
- **THEN** the system validates all required fields
- **AND** returns specific error messages for each invalid field
- **AND** allows the representative to correct and resubmit

### Requirement: Council Registration Approval Workflow
The system SHALL provide system administrators with tools to review and approve or reject council registration requests.

#### Scenario: System admin reviews and approves registration
- **WHEN** a system admin reviews a council registration and approves it
- **THEN** the system activates the tenant (status changes to "Active")
- **AND** creates the admin user account for the council with initial password
- **AND** sends welcome email to the council admin with login credentials
- **AND** initializes the council's database schema and default configuration
- **AND** assigns default permissions and roles
- **AND** creates audit log entry for the approval

#### Scenario: System admin rejects registration
- **WHEN** a system admin rejects a council registration with a reason
- **THEN** the system updates the registration status to "Rejected"
- **AND** sends rejection notification to the representative with reason
- **AND** allows the council to submit a new registration after addressing issues
- **AND** retains the rejected registration record for audit

#### Scenario: Registration request timeout
- **WHEN** a registration request remains unreviewed for 30 days
- **THEN** the system sends reminder notifications to system admins
- **AND** auto-expires the request after 60 days
- **AND** notifies the council representative of expiration
- **AND** allows resubmission if still interested

### Requirement: Council Branding and Customization
The system SHALL allow councils to customize their branding including logo, colors, fonts, and styling to reflect their identity.

#### Scenario: Council uploads logo
- **WHEN** a council admin uploads their council logo
- **THEN** the system validates the image format (PNG, JPG, SVG)
- **AND** validates the image dimensions (min 200x200px, max 2000x2000px)
- **AND** validates the file size (max 2MB)
- **AND** stores the logo in blob storage
- **AND** updates the tenant's branding configuration with logo URL
- **AND** generates thumbnail versions for different display sizes

#### Scenario: Council customizes color scheme
- **WHEN** a council admin sets primary and secondary colors
- **THEN** the system validates the color format (hex codes)
- **AND** ensures adequate contrast for accessibility (WCAG AA)
- **AND** updates the tenant's branding configuration
- **AND** provides a preview of the color scheme
- **AND** applies colors to the council's portal pages

#### Scenario: Council selects fonts
- **WHEN** a council admin selects heading and body fonts
- **THEN** the system provides a list of approved web-safe fonts
- **AND** validates the font selections
- **AND** updates the tenant's branding configuration
- **AND** applies fonts to the council's portal pages
- **AND** ensures fonts load correctly for performance

#### Scenario: Council adds custom CSS
- **WHEN** a council admin provides custom CSS for advanced styling
- **THEN** the system sanitizes the CSS to prevent security issues (XSS)
- **AND** validates the CSS syntax
- **AND** restricts to approved CSS properties (no JavaScript, no external URLs)
- **AND** previews the custom CSS in a sandbox
- **AND** applies the CSS only to the council's tenant pages
- **AND** logs all custom CSS changes for audit

#### Scenario: Council updates contact information and social media
- **WHEN** a council admin updates their public contact information
- **THEN** the system validates email addresses, phone numbers, and URLs
- **AND** allows adding social media links (Facebook, Twitter, website)
- **AND** displays the contact information on the council's public pages
- **AND** allows citizens to access contact information easily

### Requirement: Council Profile Management
The system SHALL allow councils to manage their profile information including service areas, office hours, and descriptions.

#### Scenario: Council updates service area description
- **WHEN** a council admin updates their service area description
- **THEN** the system validates the text length (max 1000 characters)
- **AND** supports rich text formatting (bold, italic, lists, links)
- **AND** displays the description on the council's public portal page
- **AND** indexes the description for search functionality

#### Scenario: Council sets office hours
- **WHEN** a council admin configures their office hours
- **THEN** the system allows setting different hours per day of week
- **AND** supports special hours for holidays
- **AND** validates time formats (HH:MM)
- **AND** displays office hours on the council's portal
- **AND** uses office hours to calculate expected response times for citizens

#### Scenario: Council uploads additional documents
- **WHEN** a council admin uploads public documents (policies, bylaws, service guides)
- **THEN** the system stores documents in blob storage
- **AND** validates file types (PDF, DOCX, XLSX)
- **AND** validates file size (max 10MB per file)
- **AND** makes documents publicly accessible on the council's portal
- **AND** tracks document version history

### Requirement: Council Federation Settings
The system SHALL allow councils to opt-in or opt-out of the cross-council federation system.

#### Scenario: Council enables federation
- **WHEN** a council admin enables cross-council federation
- **THEN** the system updates the tenant's federation status to "Enabled"
- **AND** allows citizens with verified national IDs to link to this council
- **AND** participates in cross-council aggregated views for citizens
- **AND** notifies existing citizens of the new cross-council features
- **AND** creates audit log entry for the federation enablement

#### Scenario: Council disables federation
- **WHEN** a council admin disables cross-council federation
- **THEN** the system updates the tenant's federation status to "Disabled"
- **AND** prevents new citizens from linking via national ID
- **AND** existing linked citizens retain access but no new aggregation queries include this council
- **AND** notifies affected citizens of the change
- **AND** provides a reason field for transparency

#### Scenario: Council with disabled federation re-enables
- **WHEN** a council that previously disabled federation re-enables it
- **THEN** the system restores federation capabilities
- **AND** re-includes the council in aggregated views for linked citizens
- **AND** notifies previously linked citizens of restoration

### Requirement: Council Data Synchronization Configuration
The system SHALL allow councils to configure data synchronization from their external systems via APIs, webhooks, or batch imports.

#### Scenario: Council generates API credentials
- **WHEN** a council admin requests API credentials for data sync
- **THEN** the system generates a unique API key and secret
- **AND** scopes the credentials to the council's tenant only
- **AND** displays the credentials once with security warning
- **AND** allows the admin to revoke and regenerate credentials
- **AND** logs all API usage with the credentials

#### Scenario: Council configures webhook endpoints
- **WHEN** a council admin configures webhook URLs for event subscriptions
- **THEN** the system validates the webhook URL format (HTTPS required)
- **AND** performs a test webhook call to verify connectivity
- **AND** allows subscribing to specific event types (bill created, payment received, etc.)
- **AND** implements retry logic for failed webhook deliveries
- **AND** provides delivery logs for debugging

#### Scenario: Council uploads batch data file
- **WHEN** a council admin uploads a CSV or Excel file for batch data import
- **THEN** the system validates the file format and structure
- **AND** previews the first 10 rows for confirmation
- **AND** processes the import via background job
- **AND** generates a reconciliation report showing successes and errors
- **AND** sends completion notification with report to the admin
- **AND** supports rollback if critical errors detected

#### Scenario: Council sets synchronization schedule
- **WHEN** a council admin configures automatic sync schedule
- **THEN** the system allows setting sync frequency (hourly, daily, weekly)
- **AND** allows specifying sync time windows (e.g., nightly)
- **AND** validates the schedule doesn't conflict with system maintenance
- **AND** executes syncs according to schedule
- **AND** notifies admin of sync completion or failures

### Requirement: Council Usage Analytics
The system SHALL provide councils with analytics on their platform usage and citizen engagement.

#### Scenario: Council views citizen registration metrics
- **WHEN** a council admin requests citizen analytics
- **THEN** the system displays:
  - Total registered citizens for the council
  - New registrations over time (daily, weekly, monthly)
  - Verification status breakdown
  - Citizens linked via cross-council federation
- **AND** supports filtering by date range
- **AND** provides trend visualizations

#### Scenario: Council views service usage metrics
- **WHEN** a council admin requests service usage analytics
- **THEN** the system displays:
  - Issue reports submitted (by category)
  - Applications submitted (by type)
  - Bills paid (total amount, count)
  - Service requests (by type)
  - Average response/resolution times
- **AND** compares current period to previous period
- **AND** supports export to CSV/PDF

#### Scenario: Council views citizen engagement metrics
- **WHEN** a council admin requests engagement analytics
- **THEN** the system displays:
  - Active users (daily, weekly, monthly)
  - Feature usage (most used services)
  - Citizen satisfaction scores (if surveys enabled)
  - Average session duration
- **AND** provides recommendations for improving engagement

### Requirement: Council Multi-Admin Management
The system SHALL allow councils to manage multiple administrator accounts with different permission levels.

#### Scenario: Primary admin adds new admin user
- **WHEN** a council's primary admin adds a new administrator
- **THEN** the system creates a user account for the new admin
- **AND** allows assigning specific admin roles (full admin, verification admin, billing admin, etc.)
- **AND** sends invitation email with initial password
- **AND** requires the new admin to change password on first login
- **AND** logs the admin creation in audit trail

#### Scenario: Admin permissions are role-based
- **WHEN** a council assigns an admin role to a user
- **THEN** the system grants only the permissions associated with that role
- **AND** enforces permission checks on all admin actions
- **AND** supports these role types:
  - Full Admin: All council management capabilities
  - Verification Admin: Identity verification only
  - Billing Admin: Billing and payment management
  - Service Admin: Service requests and issue management
  - Read-Only Admin: View-only access to reports

#### Scenario: Admin account is deactivated
- **WHEN** a council admin deactivates another admin account
- **THEN** the system disables the account immediately
- **AND** revokes all active sessions for that admin
- **AND** retains the admin's historical audit logs
- **AND** prevents the admin from logging in
- **AND** allows reactivation later if needed
