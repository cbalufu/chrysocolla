# Implementation Tasks

## Implementation Status

**Current Status**: Enhanced MVP with Identity Verification

An enhanced MVP has been implemented with identity verification workflows. Implementation includes:
- ✅ Complete Phase 1 (Foundation - Database & Security)
- ✅ Complete Phase 2 (Identity Verification - Self-Service & Admin Review)
- ✅ Complete Phase 3 Core (Council Branding & Customization)
- ✅ Complete Phase 4 (Federated Profile & Cross-Council Aggregation)
- 🔄 Remaining phases (3 remaining, 5, 6, 7, 8) can be added incrementally

## Phase 1: Foundation (Database and Core Entities) ✅ COMPLETE

### 1.1 Domain Model Changes ✅
- [x] 1.1.1 Create `CitizenFederatedProfile` aggregate root entity
  - Add `NationalIdType` enum (NationalId, TIN, Passport)
  - Add `NationalIdEncrypted` property
  - Add `NationalIdHash` property
  - Add `VerificationStatus` enum (Pending, Verified, Rejected)
  - Add `VerificationMethod` enum (SelfService, InPerson, BulkImport)
  - Add `VerifiedAt`, `VerifiedByTenantId` properties
  - Add `ConsentStatus` enum and `ConsentGrantedAt` properties
- [x] 1.1.2 Create `LinkedCitizenProfile` entity for tenant-citizen associations
  - Add `TenantId`, `CitizenId`, `LinkedAt` properties
- [x] 1.1.3 Extend `Citizen` entity with national ID fields
  - Add `NationalIdType`, `HasFederatedProfile` properties
  - Add navigation property to `CitizenFederatedProfile`
- [x] 1.1.4 Create repository interfaces (SKIPPED - VSA uses DbContext directly)
  - Not needed in VSA architecture

### 1.2 Encryption and Security Utilities ✅
- [x] 1.2.1 Implement encryption service for national IDs
  - AES-256 encryption/decryption methods
  - Key management (Azure Key Vault integration placeholder)
  - Environment-specific key configuration
- [x] 1.2.2 Implement hashing utility for national ID lookups
  - SHA-256 hashing with salt
  - Collision-resistant hash generation
- [x] 1.2.3 Add security configurations
  - Configure encryption keys in appsettings
  - Add key rotation documentation

### 1.3 Database Schema ✅
- [x] 1.3.1 Create EF Core entity configurations
  - `CitizenFederatedProfileConfiguration`
  - `LinkedCitizenProfileConfiguration`
  - Update `CitizenConfiguration`
- [ ] 1.3.2 Create database migration (REQUIRES dotnet CLI)
  - Ready to generate - all code is in place
  - See MIGRATION_INSTRUCTIONS.md for commands
- [ ] 1.3.3 Seed initial data if needed
  - Optional - can be added later

### 1.4 Permissions ⏸️
- [ ] 1.4.1 Define new permissions (DEFERRED - Current auth uses simple role-based)
  - Current implementation uses existing JWT auth
  - Can be enhanced later with policy-based auth
- [ ] 1.4.2 Assign permissions to roles (DEFERRED)
  - Works with existing Citizen/Admin/Staff roles

## Phase 4: Federated Profile Management (Core) ✅ COMPLETE

### 4.1 Core Federated Profile Operations ✅
- [x] 4.1.1 Create `CreateOrLinkFederatedProfile` feature
  - POST `/api/federated-profile/create-or-link`
  - Creates new federated profile or links to existing
  - Handles encryption and hashing
  - Manages consent
  - Prevents duplicate linkages
  - Supports profile reactivation
- [x] 4.1.2 Create `GetMyFederatedProfile` feature
  - GET `/api/federated-profile/me`
  - Returns profile details with linked councils
  - Shows verification and consent status
- [x] 4.1.3 Implement command handlers and validators
  - CQRS pattern with MediatR
  - FluentValidation for input validation
  - ErrorOr for error handling
- [x] 4.1.4 Add Carter endpoints with authentication
  - JWT authentication required
  - Swagger/OpenAPI documented

### 4.4 Cross-Council Aggregation Services ✅
- [x] 4.4.1 Create `GetCrossCouncilProperties` feature
  - GET `/api/cross-council/properties`
  - Retrieves properties from all linked councils
  - Uses `IgnoreQueryFilters()` for multi-tenant queries
  - Includes tenant identification
  - Supports pagination
- [x] 4.4.2 Create `GetCrossCouncilBills` feature
  - GET `/api/cross-council/bills`
  - Retrieves bills from all linked councils
  - Calculates total outstanding amount
  - Supports filtering by status
  - Supports pagination
- [x] 4.4.3 Create `GetCrossCouncilIssues` feature
  - GET `/api/cross-council/issues`
  - Retrieves issues from all linked councils
  - Supports filtering by status and category
  - Supports pagination
- [x] 4.4.4 Create `GetCrossCouncilSummary` feature
  - GET `/api/cross-council/summary`
  - Dashboard with per-council and total statistics
  - Property, bill, and issue counts
  - Total outstanding calculations

### 4.5 Cross-Council Aggregation DTOs ✅
- [x] 4.5.1 Created DTOs
  - `AggregatedPropertyDto` with TenantId/CouncilName
  - `AggregatedBillDto` with council identification
  - `AggregatedIssueDto` with council identification
  - `CrossCouncilSummaryDto` with totals and per-council breakdowns

### 4.6 Consent Management ✅
- [x] 4.6.1 Create `UpdateConsent` feature
  - POST `/api/federated-profile/consent`
  - Grant or revoke cross-council data sharing consent
  - POPIA compliance - citizen control over data
  - Consent enforced in all aggregation endpoints
  - Updates consent status and timestamp

### 4.7 Council Linking Management ✅
- [x] 4.7.1 Create `UnlinkCouncil` feature
  - DELETE `/api/federated-profile/unlink/{tenantId}`
  - Soft delete with audit trail (UnlinkedAt timestamp)
  - Prevents unlinking from current council
  - Returns remaining linked councils count
  - Unlinked data no longer in aggregated views

## Phase 3: Council Management (Core Features) ✅ COMPLETE

### 3.6 Council Branding Domain ✅
- [x] 3.6.1 Extended `Tenant` entity with branding fields
  - `BrandingConfig` (JSON) - stores configuration
  - `FederationEnabled` (bool) - opt-in/out flag
- [x] 3.6.2 Created `TenantBrandingConfig` model
  - Logo URL storage
  - Colors: Primary, secondary, accent (hex)
  - Fonts: Heading and body selections
  - Custom CSS with sanitization
  - Contact information structure
  - Social media links

### 3.7 Council Branding Application Service ✅
- [x] 3.7.1 Create `GetCouncilBranding` feature
  - GET `/api/council-branding?tenantId={id}`
  - Public endpoint (no authentication)
  - Returns full branding configuration
  - Falls back to defaults if not configured
  - Query any council's branding
- [x] 3.7.2 Create `UpdateCouncilBranding` feature
  - PUT `/api/council-branding`
  - Requires Admin or Staff role
  - Partial updates supported
  - Hex color validation
  - CSS sanitization (removes dangerous patterns)
  - Stores as JSON in Tenant table

### 3.8 Council Branding DTOs ✅
- [x] 3.8.1 Created DTOs
  - `BrandingDto` - complete branding configuration
  - `ColorsDto` - color scheme
  - `FontsDto` - typography
  - `ContactDto` - contact information
  - `SocialMediaDto` - social links
  - Update request DTOs for partial updates

## Phase 2: Identity Verification ✅ COMPLETE

### 2.1 Document Management Domain ✅
- [x] 2.1.1 Create `IdentityVerificationRequest` aggregate root
  - `VerificationRequestStatus` enum (PendingReview, UnderReview, Approved, Rejected, Expired)
  - Full entity with all required fields including assignment tracking
  - Multi-tenant support with `IMultiTenant`
- [x] 2.1.2 Create `VerificationDocument` entity
  - `DocumentType` enum (IdFront, IdBack, ProofOfAddress, Passport, TinCertificate)
  - Soft delete support with `IsDeleted`, `DeletedAt`
  - Document expiration tracking
- [x] 2.1.3 Create EF Core configurations (ADAPTED - VSA uses DbContext directly)
  - `IdentityVerificationRequestConfiguration` with indexes
  - `VerificationDocumentConfiguration`
  - Added to `ApplicationDbContext`

### 2.2 Verification Features (VSA) ✅
- [x] 2.2.1 Create `SubmitVerificationRequest` feature
  - POST `/api/identity-verification/submit`
  - Generates unique reference number (VER-YYYYMMDD-XXXXX format)
  - Prevents duplicate pending requests
  - 90-day expiration for documents
  - FluentValidation for input validation
- [x] 2.2.2 Create `UploadVerificationDocument` feature
  - POST `/api/identity-verification/{requestId}/upload`
  - Multipart form file upload
  - File type validation (JPEG, PNG, PDF)
  - File size validation (max 5MB)
  - Replaces existing documents of same type (soft delete)
  - Blob name generation with unique identifiers
- [x] 2.2.3 Create `GetVerificationStatus` feature
  - GET `/api/identity-verification/status`
  - Query by request ID, reference number, or latest request
  - Shows all uploaded documents (non-deleted)
  - Contextual status messages for citizens
- [x] 2.2.4 Create `GetVerificationQueue` feature (Admin/Staff)
  - GET `/api/identity-verification/queue`
  - Pagination support (default 20, max 100)
  - Filter by status (defaults to pending/under review)
  - Shows days waiting calculation
  - Document count per request
- [x] 2.2.5 Create `ReviewVerification` feature (Admin/Staff)
  - POST `/api/identity-verification/{requestId}/review`
  - Approve or reject with reason
  - On approval: Creates/updates `CitizenFederatedProfile` with verified status
  - Links citizen to federated profile
  - Uses `NationalIdEncryptionService` for secure storage
  - Records reviewer info and timestamp

### 2.3 Document Storage ✅
- [x] 2.3.1 Document upload validation implemented
  - File type validation (JPEG, PNG, PDF)
  - File size validation (max 5MB)
  - ContentType validation
- [x] 2.3.2 Blob storage structure designed
  - Format: `verification-docs/{referenceNumber}/{documentType}-{timestamp}-{guid}.ext`
  - TODO: Actual blob storage integration (Azure Blob Storage, AWS S3, etc.)
  - Currently stores metadata only (production-ready pattern in place)
- [ ] 2.3.3 Document expiration job (DEFERRED)
  - Background job to delete expired documents (90 days)
  - Can be implemented using Hangfire or similar
  - Retention of metadata for audit

### 2.4 Verification DTOs ✅
- [x] 2.4.1 Created all required DTOs
  - `SubmitVerificationRequestDto` and Response
  - `UploadVerificationDocumentDto` (via IFormFile) and Response
  - `GetVerificationStatusResponse` with `VerificationDocumentDto`
  - `VerificationQueueItemDto` and Response (paginated)
  - `ReviewVerificationDto` and Response

### 2.5 Verification HTTP API ✅
- [x] 2.5.1 Citizen endpoints (Carter modules)
  - POST `/api/identity-verification/submit` - Submit verification request
  - POST `/api/identity-verification/{requestId}/upload` - Upload document
  - GET `/api/identity-verification/status` - Check status
  - All require `[Authorize]` attribute
  - Full OpenAPI/Swagger documentation
- [x] 2.5.2 Admin/Staff endpoints (Carter modules)
  - GET `/api/identity-verification/queue` - View verification queue
  - POST `/api/identity-verification/{requestId}/review` - Approve/reject
  - Role-based authorization (Admin or Staff required)

### 2.6 Verification Business Logic ⏸️
- [ ] 2.6.1 Fraud detection (DEFERRED - Can be added incrementally)
  - Document hash checking (detect duplicate documents)
  - Rate limiting by IP address
  - Rejection count tracking
- [ ] 2.6.2 Notification triggers (DEFERRED - Can be added incrementally)
  - Email notification on submission
  - Email notification on approval/rejection
  - Admin notifications for new requests

### 2.7 Verification Analytics ⏸️
- [ ] 2.7.1 Analytics service (DEFERRED - Can be added later)
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
