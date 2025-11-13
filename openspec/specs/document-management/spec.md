# Document Management

## Overview
The Document Management capability provides secure storage, retrieval, and management of documents using blob storage with features like categorization, expiry tracking, digital signatures, and permission levels.

## Requirements

### Requirement: Document Upload
The system SHALL allow upload of documents to blob storage with metadata including file name, category, and expiry date.

#### Scenario: Upload document
- **WHEN** a user uploads a document file
- **THEN** the system stores the file in blob storage
- **AND** generates MD5 hash for integrity verification
- **AND** creates document record with file metadata
- **AND** assigns unique GUID identifier
- **AND** returns document ID and metadata

#### Scenario: Upload with category
- **WHEN** uploading document with specific category
- **THEN** the system categorizes the document
- **AND** enables filtering by category

### Requirement: Document Categories
The system SHALL support document categories: Identity, Proof of Residence, Application Form, Supporting Document, Certificate, Receipt, and Other.

#### Scenario: Categorize as identity document
- **WHEN** uploading citizen ID document
- **THEN** the system sets category to "Identity"
- **AND** applies appropriate retention and security rules

### Requirement: Document Download
The system SHALL provide secure download of stored documents with permission checks.

#### Scenario: Download own document
- **WHEN** a citizen downloads their own document
- **THEN** the system verifies ownership or permission
- **AND** retrieves file from blob storage
- **AND** returns file with correct content type

#### Scenario: Unauthorized download attempt
- **WHEN** a user attempts to download document without permission
- **THEN** the system denies access
- **AND** returns authorization error

### Requirement: Document Expiry Tracking
The system SHALL track document expiry dates and identify expired documents.

#### Scenario: Set expiry date
- **WHEN** uploading document with expiry date (e.g., ID expires 2025-12-31)
- **THEN** the system stores the expiry date
- **AND** enables expiry date queries

#### Scenario: Query expired documents
- **WHEN** querying for expired documents
- **THEN** the system returns documents where expiry date is past current date

### Requirement: Digital Signatures
The system SHALL support digital signing of documents with signature metadata.

#### Scenario: Sign document
- **WHEN** an authorized user digitally signs a document
- **THEN** the system updates `IsSigned` to true
- **AND** records `SignedBy` user ID
- **AND** records `SignedDate` timestamp
- **AND** stores signature data

### Requirement: Document Permissions
The system SHALL enforce permission levels: Private (owner only), Shared (specific users), Public (all users).

#### Scenario: Private document access
- **WHEN** document permission is "Private"
- **THEN** only the document owner can access it
- **AND** other users receive authorization error

#### Scenario: Public document access
- **WHEN** document permission is "Public"
- **THEN** all users in the tenant can view the document

### Requirement: File Integrity Verification
The system SHALL generate and store MD5 hash for uploaded files to verify integrity.

#### Scenario: Verify file integrity
- **WHEN** downloading a document
- **THEN** the system can verify the MD5 hash matches stored hash
- **AND** detect any file corruption

### Requirement: Document Search and Filtering
The system SHALL provide search by file name, category, upload date, and expiry status.

#### Scenario: Search by category
- **WHEN** filtering documents by "Certificate" category
- **THEN** the system returns all certificates
- **AND** respects permission levels

#### Scenario: Find expiring documents
- **WHEN** searching for documents expiring within 30 days
- **THEN** the system returns documents with expiry date within that range

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants for all document records.

#### Scenario: Tenant isolation
- **WHEN** querying documents from Tenant A
- **THEN** the system returns only documents from Tenant A

## Data Model

### Document Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **FileName**: string (Required)
- **FileSize**: long (Bytes)
- **ContentType**: string (MIME type)
- **BlobName**: string (Storage identifier)
- **Category**: DocumentCategory enum
- **Description**: string (Optional)
- **ExpiryDate**: DateTime? (Nullable)
- **PermissionLevel**: DocumentPermission enum
- **IsSigned**: bool (Default: false)
- **SignedBy**: Guid? (Nullable)
- **SignedDate**: DateTime? (Nullable)
- **SignatureData**: string (Optional)
- **MD5Hash**: string (Integrity check)
- Audit fields

### DocumentCategory Enum
- Identity, ProofOfResidence, ApplicationForm, SupportingDocument, Certificate, Receipt, Other

### DocumentPermission Enum
- Private, Shared, Public

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/DocumentAppService.cs`
- Implements `CrudAppService<Document, DocumentDto, Guid, DocumentDto, CreateUpdateDocumentDto>`
- Entity in `src/CitizensPortal.Domain/Entities/Document.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/DocumentEnums.cs`
- HTTP API exposed via `src/CitizensPortal.HttpApi/Controllers/DocumentController.cs`
- Endpoints: `GET/POST/PUT/DELETE /api/documents`
- Uses ABP Blob Storage (FileSystem provider)
- MD5 hash calculation on upload for integrity verification
