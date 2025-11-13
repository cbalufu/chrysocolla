# Implementation Tasks

## Phase 1: Foundation (Database and Core Entities)

### 1.1 Domain Model Changes
- [ ] 1.1.1 Create `CitizenFederatedProfile` aggregate root entity
  - Add `NationalIdType` enum (NationalId, TIN, Passport)
  - Add `NationalIdEncrypted` property
  - Add `NationalIdHash` property
  - Add `VerificationStatus` enum (Pending, Verified, Rejected)
  - Add `VerificationMethod` enum (SelfService, InPerson, BulkImport)
  - Add `VerifiedAt`, `VerifiedByTenantId` properties
  - Add `ConsentStatus` enum and `ConsentGrantedAt` properties
- [ ] 1.1.2 Create `LinkedCitizenProfile` entity for tenant-citizen associations
  - Add `TenantId`, `CitizenId`, `LinkedAt` properties
- [ ] 1.1.3 Extend `Citizen` entity with national ID fields
  - Add `NationalIdType`, `HasFederatedProfile` properties
  - Add navigation property to `CitizenFederatedProfile`
- [ ] 1.1.4 Create repository interfaces
  - `ICitizenFederatedProfileRepository`
  - Extend `ICitizenRepository` with federation methods

### 1.2 Encryption and Security Utilities
- [ ] 1.2.1 Implement encryption service for national IDs
  - AES-256 encryption/decryption methods
  - Key management (Azure Key Vault integration placeholder)
  - Environment-specific key configuration
- [ ] 1.2.2 Implement hashing utility for national ID lookups
  - SHA-256 hashing with salt
  - Collision-resistant hash generation
- [ ] 1.2.3 Add security configurations
  - Configure encryption keys in appsettings
  - Add key rotation documentation

### 1.3 Database Schema
- [ ] 1.3.1 Create EF Core entity configurations
  - `CitizenFederatedProfileConfiguration`
  - `LinkedCitizenProfileConfiguration`
  - Update `CitizenConfiguration`
- [ ] 1.3.2 Create database migration
  - Add `CitizenFederatedProfiles` table
  - Add `LinkedCitizenProfiles` table
  - Add indexes on `NationalIdHash` for performance
  - Add foreign keys and constraints
  - Update `Citizens` table with new fields
- [ ] 1.3.3 Seed initial data if needed
  - Default verification statuses
  - Test data for development environment

### 1.4 Permissions
- [ ] 1.4.1 Define new permissions
  - `CitizenManagement.FederatedProfiles.View`
  - `CitizenManagement.FederatedProfiles.Manage`
  - `IdentityVerification.View`
  - `IdentityVerification.Approve`
  - `IdentityVerification.Reject`
  - `TenantManagement.SelfRegister`
  - `TenantManagement.Branding.Manage`
- [ ] 1.4.2 Assign permissions to roles
  - System Admin: All permissions
  - Council Admin: Verification, branding
  - Citizen: View own federated profile

## Phase 2: Identity Verification

### 2.1 Document Management Domain
- [ ] 2.1.1 Create `IdentityVerificationRequest` aggregate root
  - Add `CitizenId`, `TenantId`, `ReferenceNumber` properties
  - Add `Status` enum (PendingReview, Approved, Rejected, Expired)
  - Add `SubmittedAt`, `ReviewedAt`, `ReviewedByUserId` properties
  - Add `RejectionReason` property
  - Add document blob references
- [ ] 2.1.2 Create `VerificationDocument` entity
  - Add `DocumentType` enum (IdFront, IdBack, ProofOfAddress)
  - Add `BlobName`, `FileSize`, `UploadedAt` properties
  - Add `ExpiresAt` property
- [ ] 2.1.3 Create repository interface
  - `IIdentityVerificationRequestRepository`

### 2.2 Verification Application Services
- [ ] 2.2.1 Create `IdentityVerificationAppService`
  - `SubmitVerificationRequestAsync` method
  - `UploadDocumentAsync` method
  - `GetVerificationStatusAsync` method
  - `GetVerificationHistoryAsync` method
- [ ] 2.2.2 Create `IdentityVerificationAdminAppService`
  - `GetVerificationQueueAsync` method (with pagination)
  - `AssignVerificationToSelfAsync` method
  - `ApproveVerificationAsync` method
  - `RejectVerificationAsync` method
  - `GetVerificationDetailsAsync` method

### 2.3 Document Storage
- [ ] 2.3.1 Configure blob container for verification documents
  - Create dedicated container with encryption
  - Set access policies (private)
- [ ] 2.3.2 Implement document upload validation
  - File type validation (JPEG, PNG, PDF)
  - File size validation (max 5MB)
  - Image dimension validation
- [ ] 2.3.3 Implement document expiration job
  - Background job to delete expired documents (90 days)
  - Retention of metadata for audit

### 2.4 Verification DTOs
- [ ] 2.4.1 Create DTOs
  - `SubmitVerificationRequestDto`
  - `UploadDocumentDto`
  - `VerificationRequestDto`
  - `VerificationQueueItemDto`
  - `VerificationDetailsDto`
  - `ApproveVerificationDto`
  - `RejectVerificationDto`

### 2.5 Verification HTTP API
- [ ] 2.5.1 Create `IdentityVerificationController`
  - POST `/api/app/identity-verification/submit`
  - POST `/api/app/identity-verification/upload-document`
  - GET `/api/app/identity-verification/status`
  - GET `/api/app/identity-verification/history`
- [ ] 2.5.2 Create `IdentityVerificationAdminController`
  - GET `/api/app/identity-verification-admin/queue`
  - POST `/api/app/identity-verification-admin/assign/{id}`
  - POST `/api/app/identity-verification-admin/approve/{id}`
  - POST `/api/app/identity-verification-admin/reject/{id}`
  - GET `/api/app/identity-verification-admin/details/{id}`

### 2.6 Verification Business Logic
- [ ] 2.6.1 Implement fraud detection
  - Document hash checking (detect duplicate documents)
  - Rate limiting by IP address
  - Rejection count tracking
- [ ] 2.6.2 Implement notification triggers
  - Email notification on submission
  - Email notification on approval/rejection
  - Admin notifications for new requests

### 2.7 Verification Analytics
- [ ] 2.7.1 Create `VerificationAnalyticsAppService`
  - `GetVerificationMetricsAsync` method (approval rate, avg review time)
  - `GetVerificationTrendsAsync` method

## Phase 3: Council Self-Registration and Management

### 3.1 Council Registration Domain
- [ ] 3.1.1 Create `CouncilRegistrationRequest` entity
  - Add `CouncilName`, `RegistrationNumber`, `Region` properties
  - Add `ContactName`, `ContactEmail`, `ContactPhone` properties
  - Add `AdminName`, `AdminEmail` properties
  - Add `Status` enum (PendingApproval, Approved, Rejected, Expired)
  - Add `ReferenceNumber`, `SubmittedAt`, `ReviewedAt` properties
  - Add document blob references
- [ ] 3.1.2 Create repository interface
  - `ICouncilRegistrationRequestRepository`

### 3.2 Council Registration Application Services
- [ ] 3.2.1 Create `CouncilRegistrationAppService`
  - `SubmitRegistrationAsync` method
  - `GetRegistrationStatusAsync` method
- [ ] 3.2.2 Create `CouncilRegistrationAdminAppService`
  - `GetPendingRegistrationsAsync` method
  - `ApproveRegistrationAsync` method (creates tenant + admin user)
  - `RejectRegistrationAsync` method

### 3.3 Council Registration DTOs
- [ ] 3.3.1 Create DTOs
  - `SubmitCouncilRegistrationDto`
  - `CouncilRegistrationDto`
  - `CouncilRegistrationDetailsDto`
  - `ApproveCouncilRegistrationDto`
  - `RejectCouncilRegistrationDto`

### 3.4 Council Registration HTTP API
- [ ] 3.4.1 Create `CouncilRegistrationController`
  - POST `/api/app/council-registration/submit`
  - GET `/api/app/council-registration/status/{referenceNumber}`
- [ ] 3.4.2 Create `CouncilRegistrationAdminController`
  - GET `/api/app/council-registration-admin/pending`
  - POST `/api/app/council-registration-admin/approve/{id}`
  - POST `/api/app/council-registration-admin/reject/{id}`

### 3.5 Tenant Initialization Logic
- [ ] 3.5.1 Implement tenant creation workflow
  - Create tenant record
  - Initialize database schema for new tenant
  - Create admin user with initial password
  - Assign default roles and permissions
  - Send welcome email with credentials

### 3.6 Council Branding Domain
- [ ] 3.6.1 Create `TenantBranding` entity (or use TenantProperties)
  - Store as JSON in `ExtraProperties` or new table
  - Schema for logo URL, colors, fonts, custom CSS
- [ ] 3.6.2 Create `TenantBrandingManager` domain service
  - Validation logic for branding data
  - CSS sanitization logic

### 3.7 Council Branding Application Service
- [ ] 3.7.1 Create `CouncilBrandingAppService`
  - `UploadLogoAsync` method
  - `UpdateColorsAsync` method
  - `UpdateFontsAsync` method
  - `UpdateCustomCssAsync` method
  - `GetBrandingAsync` method
  - `UpdateContactInfoAsync` method

### 3.8 Council Branding DTOs
- [ ] 3.8.1 Create DTOs
  - `TenantBrandingDto`
  - `UpdateBrandingColorsDto`
  - `UpdateBrandingFontsDto`
  - `UpdateCustomCssDto`
  - `UpdateContactInfoDto`

### 3.9 Council Branding HTTP API
- [ ] 3.9.1 Create `CouncilBrandingController`
  - POST `/api/app/council-branding/logo`
  - PUT `/api/app/council-branding/colors`
  - PUT `/api/app/council-branding/fonts`
  - PUT `/api/app/council-branding/custom-css`
  - PUT `/api/app/council-branding/contact-info`
  - GET `/api/app/council-branding`

### 3.10 Council Profile Management
- [ ] 3.10.1 Extend `TenantAppService` or create `CouncilProfileAppService`
  - `UpdateServiceAreaAsync` method
  - `UpdateOfficeHoursAsync` method
  - `UploadDocumentAsync` method
  - `GetProfileAsync` method

## Phase 4: Federated Profile and Cross-Council Features

### 4.1 Federated Profile Application Service
- [ ] 4.1.1 Create `CitizenFederatedProfileAppService`
  - `CreateOrLinkProfileAsync` method (links national ID to citizen)
  - `GetFederatedProfileAsync` method
  - `LinkToCouncilAsync` method
  - `UnlinkFromCouncilAsync` method
  - `UpdateNationalIdAsync` method (triggers re-verification)
  - `GrantConsentAsync` method
  - `RevokeConsentAsync` method
- [ ] 4.1.2 Implement federated profile lookup
  - Query by national ID hash
  - Resolve linked tenant-citizen records
  - Authorization checks

### 4.2 Federated Profile DTOs
- [ ] 4.2.1 Create DTOs
  - `CreateFederatedProfileDto`
  - `CitizenFederatedProfileDto`
  - `LinkedCouncilDto`
  - `UpdateNationalIdDto`
  - `ConsentDto`

### 4.3 Federated Profile HTTP API
- [ ] 4.3.1 Create `CitizenFederatedProfileController`
  - POST `/api/app/citizen-federated-profile/create-or-link`
  - GET `/api/app/citizen-federated-profile/me`
  - POST `/api/app/citizen-federated-profile/link-council`
  - POST `/api/app/citizen-federated-profile/unlink-council/{tenantId}`
  - PUT `/api/app/citizen-federated-profile/national-id`
  - POST `/api/app/citizen-federated-profile/consent`

### 4.4 Cross-Council Aggregation Services
- [ ] 4.4.1 Create `CrossCouncilPropertyAppService`
  - `GetAggregatedPropertiesAsync` method
  - Implement multi-tenant query logic
  - Add caching layer
- [ ] 4.4.2 Create `CrossCouncilBillingAppService`
  - `GetAggregatedBillsAsync` method
  - `GetTotalOutstandingAsync` method
- [ ] 4.4.3 Create `CrossCouncilIssueAppService`
  - `GetAggregatedIssuesAsync` method

### 4.5 Cross-Council Aggregation DTOs
- [ ] 4.5.1 Create DTOs
  - `AggregatedPropertyDto` (extends PropertyDto with TenantId/CouncilName)
  - `AggregatedBillDto`
  - `AggregatedIssueDto`
  - `CrossCouncilSummaryDto`

### 4.6 Cross-Council HTTP API
- [ ] 4.6.1 Create `CrossCouncilController`
  - GET `/api/app/cross-council/properties`
  - GET `/api/app/cross-council/bills`
  - GET `/api/app/cross-council/issues`
  - GET `/api/app/cross-council/summary`

### 4.7 Caching Infrastructure
- [ ] 4.7.1 Configure distributed cache for federated profiles
  - Redis or in-memory cache configuration
  - Cache key strategy
  - TTL settings (5-15 minutes)
- [ ] 4.7.2 Implement cache invalidation
  - Invalidate on profile updates
  - Invalidate on verification status changes

### 4.8 Performance Optimization
- [ ] 4.8.1 Add database indexes
  - Index on `NationalIdHash` in `CitizenFederatedProfiles`
  - Composite indexes on tenant-citizen lookups
- [ ] 4.8.2 Implement query pagination
  - All aggregated views must support pagination
  - Default page size: 20
- [ ] 4.8.3 Implement parallel tenant queries
  - Use `Task.WhenAll` for multi-tenant queries
  - Add circuit breaker for failing tenants

## Phase 5: Data Synchronization

### 5.1 Council API Credentials
- [ ] 5.1.1 Create `CouncilApiCredential` entity
  - Add `TenantId`, `ApiKey`, `ApiSecret` (encrypted) properties
  - Add `CreatedAt`, `LastUsedAt`, `IsActive` properties
- [ ] 5.1.2 Create `CouncilApiCredentialManager` domain service
  - Generate secure API keys
  - Encryption/decryption of secrets

### 5.2 Data Sync Application Services
- [ ] 5.2.1 Create `CouncilDataSyncAppService`
  - `GenerateApiCredentialsAsync` method
  - `RevokeApiCredentialsAsync` method
  - `ConfigureWebhooksAsync` method
  - `UploadBatchDataAsync` method
  - `GetSyncHistoryAsync` method

### 5.3 Data Sync API Endpoints
- [ ] 5.3.1 Create authenticated sync endpoints
  - POST `/api/sync/bills` (with API key auth)
  - POST `/api/sync/payments`
  - POST `/api/sync/properties`
  - POST `/api/sync/citizens`
- [ ] 5.3.2 Implement API key authentication middleware
  - Validate API key from header
  - Resolve tenant from API key
  - Rate limiting per API key

### 5.4 Batch Import Processing
- [ ] 5.4.1 Create background job for batch processing
  - Parse CSV/Excel files
  - Validate data format
  - Import records with error handling
  - Generate reconciliation report
- [ ] 5.4.2 Create `BatchImportResult` entity
  - Track import status, success count, error count
  - Store error details

### 5.5 Webhook Infrastructure
- [ ] 5.5.1 Create `WebhookSubscription` entity
  - Add `TenantId`, `EventType`, `TargetUrl` properties
  - Add `IsActive`, `RetryCount` properties
- [ ] 5.5.2 Implement webhook delivery service
  - Send HTTP POST to webhook URLs
  - Retry logic with exponential backoff
  - Log delivery status

### 5.6 Data Sync DTOs
- [ ] 5.6.1 Create DTOs
  - `GenerateApiCredentialsDto`
  - `ApiCredentialDto`
  - `ConfigureWebhookDto`
  - `BatchImportDto`
  - `SyncHistoryDto`

## Phase 6: POPIA Compliance and Data Privacy

### 6.1 Data Subject Access Requests (DSAR)
- [ ] 6.1.1 Create `DataSubjectAccessRequest` entity
  - Add `CitizenId`, `RequestType` enum (Export, Delete), `Status`
  - Add `RequestedAt`, `CompletedAt`, `ReportBlobName`
- [ ] 6.1.2 Create `DsarAppService`
  - `SubmitExportRequestAsync` method
  - `SubmitDeleteRequestAsync` method
  - `GetRequestStatusAsync` method
  - `DownloadExportAsync` method
- [ ] 6.1.3 Implement background job for DSAR processing
  - Collect federated profile data
  - Query all linked tenants for citizen data
  - Generate JSON and PDF reports
  - Anonymize data for delete requests

### 6.2 Consent Management
- [ ] 6.2.1 Implement consent tracking in federated profile
  - Already added in Phase 4.1
- [ ] 6.2.2 Create consent UI flows (API endpoints)
  - Display consent form during registration
  - Allow revoking/granting consent

### 6.3 Audit Logging Enhancements
- [ ] 6.3.1 Ensure audit logs for all sensitive operations
  - Federated profile access
  - National ID lookups
  - Verification approvals/rejections
  - Cross-council data queries
  - Consent changes
- [ ] 6.3.2 Create `AuditLogAppService` for compliance reporting
  - `GetAuditTrailForCitizenAsync` method

## Phase 7: Testing

### 7.1 Unit Tests
- [ ] 7.1.1 Test encryption/hashing utilities
- [ ] 7.1.2 Test federated profile domain logic
- [ ] 7.1.3 Test verification workflows
- [ ] 7.1.4 Test council registration workflows
- [ ] 7.1.5 Test cross-council aggregation logic

### 7.2 Integration Tests
- [ ] 7.2.1 Test cross-tenant queries with test tenants
- [ ] 7.2.2 Test identity verification end-to-end
- [ ] 7.2.3 Test council self-registration end-to-end
- [ ] 7.2.4 Test data synchronization endpoints
- [ ] 7.2.5 Test DSAR workflows

### 7.3 Performance Tests
- [ ] 7.3.1 Load test cross-council aggregation with 100+ linked properties
- [ ] 7.3.2 Load test verification queue with 1000+ pending requests
- [ ] 7.3.3 Test caching effectiveness
- [ ] 7.3.4 Test query performance with indexes

### 7.4 Security Tests
- [ ] 7.4.1 Test authorization bypass attempts
- [ ] 7.4.2 Test national ID encryption at rest
- [ ] 7.4.3 Test API key authentication
- [ ] 7.4.4 Test CSS sanitization for XSS
- [ ] 7.4.5 Test rate limiting

## Phase 8: Documentation and Deployment

### 8.1 API Documentation
- [ ] 8.1.1 Update Swagger documentation for all new endpoints
- [ ] 8.1.2 Create API integration guide for councils (data sync)
- [ ] 8.1.3 Document webhook event types and payloads

### 8.2 User Documentation
- [ ] 8.2.1 Create citizen guide for national ID linking
- [ ] 8.2.2 Create council admin guide for identity verification
- [ ] 8.2.3 Create council guide for self-registration
- [ ] 8.2.4 Create council guide for branding customization

### 8.3 Database Migration Plan
- [ ] 8.3.1 Create migration scripts for production
- [ ] 8.3.2 Test migration on staging environment
- [ ] 8.3.3 Create rollback scripts

### 8.4 Configuration
- [ ] 8.4.1 Add feature flags for federation (enable/disable per environment)
- [ ] 8.4.2 Configure encryption keys in Azure Key Vault (production)
- [ ] 8.4.3 Configure blob storage for production
- [ ] 8.4.4 Configure distributed cache (Redis)

### 8.5 Deployment
- [ ] 8.5.1 Deploy to staging environment
- [ ] 8.5.2 Run smoke tests on staging
- [ ] 8.5.3 Deploy to production
- [ ] 8.5.4 Monitor performance and errors post-deployment
