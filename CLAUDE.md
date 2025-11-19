<!-- OPENSPEC:START -->
# OpenSpec Instructions

These instructions are for AI assistants working in this project.

Always open `@/openspec/AGENTS.md` when the request:
- Mentions planning or proposals (words like proposal, spec, change, plan)
- Introduces new capabilities, breaking changes, architecture shifts, or big performance/security work
- Sounds ambiguous and you need the authoritative spec before coding

Use `@/openspec/AGENTS.md` to learn:
- How to create and apply change proposals
- Spec format and conventions
- Project structure and guidelines

Keep this managed block so 'openspec update' can refresh the instructions.

<!-- OPENSPEC:END -->

# Citizens Portal - Multi-Council Ratepayer System

## Project Overview

Citizens Portal is a multi-tenant platform for local authorities (councils) to manage citizen services including bills, payments, issues, appointments, and more. The platform supports **multi-council identity linking** where citizens can use their National ID or TIN to link accounts across multiple councils and view aggregated data.

## Architecture

- **Backend**: .NET 8+ with ASP.NET Core
- **Architecture Pattern**: Vertical Slice Architecture (VSA)
- **CQRS**: MediatR for commands and queries
- **API**: Carter for minimal API endpoint registration
- **Validation**: FluentValidation
- **Error Handling**: ErrorOr pattern
- **Database**: Entity Framework Core with multi-tenancy support
- **Multi-Tenancy**: Tenant isolation with global query filters

## Current Implementation Status

**Status**: Full-Featured MVP with Council Self-Registration

### ✅ Phase 1: Foundation (Database and Core Entities)
- Multi-tenant database schema
- National ID encryption (AES-256) and hashing (SHA-256)
- Federated profile entities (hub-and-spoke model)
- Security utilities for POPIA compliance

### ✅ Phase 2: Identity Verification (Complete with Enhancements)
**Core Features:**
- Submit verification request (citizens)
- Upload identity documents (JPEG, PNG, PDF, max 5MB)
- Check verification status
- Admin verification queue (paginated)
- Admin review (approve/reject)

**Enhancements:**
- Email notifications (submission, approval, rejection)
- In-app notifications
- Fraud detection (rate limiting, duplicate document detection)
- Document expiration background job (runs every 6 hours)
- Suspicious activity detection

**Endpoints:**
- `POST /api/identity-verification/submit`
- `POST /api/identity-verification/{requestId}/upload`
- `GET /api/identity-verification/status`
- `GET /api/identity-verification/queue` (Admin/Staff)
- `POST /api/identity-verification/{requestId}/review` (Admin/Staff)

### ✅ Phase 3: Council Self-Registration
**Features:**
- Public council registration (no auth required)
- Check registration status by reference number
- Admin queue for pending registrations
- Admin review with automatic tenant creation
- Secure password generation (16 characters)
- Welcome emails with credentials

**Endpoints:**
- `POST /api/council-registration/submit` (Public)
- `GET /api/council-registration/status/{referenceNumber}` (Public)
- `GET /api/council-registration/pending` (Admin/Staff)
- `POST /api/council-registration/{requestId}/review` (Admin/Staff)

**Tenant Initialization:**
- Creates tenant record with FederationEnabled=true
- Generates secure admin password
- Sends welcome email
- *Note: User entity creation is stubbed - needs auth system integration*

### ✅ Phase 4: Federated Profile & Cross-Council Aggregation
**Features:**
- Create/link federated profile using National ID
- View my federated profile
- Cross-council property aggregation
- Cross-council bill aggregation
- Cross-council issue aggregation
- Cross-council summary dashboard
- Consent management (POPIA compliance)
- Council unlinking

**Endpoints:**
- `POST /api/federated-profile/create-or-link`
- `GET /api/federated-profile/me`
- `GET /api/cross-council/properties`
- `GET /api/cross-council/bills`
- `GET /api/cross-council/issues`
- `GET /api/cross-council/summary`
- `POST /api/federated-profile/consent`
- `DELETE /api/federated-profile/unlink/{tenantId}`

**Council Branding:**
- Get council branding (public)
- Update council branding (Admin/Staff)
- Branding stored as JSON in Tenant entity

**Endpoints:**
- `GET /api/council-branding?tenantId={id}` (Public)
- `PUT /api/council-branding` (Admin/Staff)

### 🔄 Remaining Phases (Can be added incrementally)
- **Phase 5**: Data Synchronization (council API, webhooks, batch imports)
- **Phase 6**: POPIA Compliance (DSAR workflows, audit logging)
- **Phase 7**: Testing (unit, integration, performance, security tests)
- **Phase 8**: Documentation (API docs, user guides, deployment guides)

## Key Technical Patterns

### Vertical Slice Architecture
Features are organized by capability, not by layer:
```
Features/
  IdentityVerification/
    SubmitRequest/
      SubmitVerificationRequest.cs (endpoint)
      SubmitVerificationRequestCommand.cs
      SubmitVerificationRequestCommandHandler.cs
      SubmitVerificationRequestValidator.cs
      SubmitVerificationRequestResponse.cs
```

### Multi-Tenant Queries
Use `IgnoreQueryFilters()` for cross-tenant queries:
```csharp
var properties = await _context.Properties
    .IgnoreQueryFilters() // Bypass tenant filter
    .Where(p => p.TenantId == tenantId && p.OwnerId == citizenId)
    .ToListAsync();
```

### Error Handling
Use ErrorOr pattern:
```csharp
public async Task<ErrorOr<Response>> Handle(Command request, CancellationToken ct)
{
    if (invalid)
        return Error.Validation("Code", "Message");

    // ... success logic
    return new Response(...);
}
```

### Endpoint Registration
Carter modules for minimal APIs:
```csharp
public sealed class FeatureEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/feature", async (...) => { ... })
            .WithName("FeatureName")
            .WithTags("Category")
            .Produces<Response>(StatusCodes.Status200OK);
    }
}
```

## Security

### National ID Protection
- **Encryption**: AES-256 for storage
- **Hashing**: SHA-256 with salt for lookups
- **POPIA Compliance**: Explicit consent required for cross-council data sharing

### Authentication & Authorization
- JWT-based authentication
- Role-based authorization (Admin, Staff, Citizen)
- `[Authorize]` attribute on protected endpoints
- Claims-based identity (CitizenId, UserId, Role)

### Fraud Detection
- IP-based rate limiting (3 attempts per hour)
- Document hash checking for duplicates
- Suspicious activity detection
- Rejection count tracking

## Database Migrations

**To generate migrations:**
```bash
cd src/CitizensPortal.Api
dotnet ef migrations add MigrationName
dotnet ef database update
```

**Current migrations needed:**
- Phase 1: Federated profiles and linked profiles
- Phase 2: Identity verification requests and documents
- Phase 3: Council registration requests

## Service Registration

**Required DI registrations:**
```csharp
// Phase 1
services.AddScoped<INationalIdEncryptionService, NationalIdEncryptionService>();

// Phase 2
services.AddScoped<IVerificationNotificationService, VerificationNotificationService>();
services.AddScoped<IFraudDetectionService, FraudDetectionService>();
services.AddHostedService<DocumentExpirationService>();

// Phase 3
services.AddScoped<ITenantInitializationService, TenantInitializationService>();

// Email
services.AddScoped<IEmailService, EmailService>();
```

## Configuration

**appsettings.json additions:**
```json
{
  "Security": {
    "NationalIdEncryptionKey": "32-byte-key-change-in-production!",
    "NationalIdHashSalt": "salt-change-in-production"
  }
}
```

## API Documentation

All endpoints are documented with:
- OpenAPI/Swagger documentation
- Summary and description
- Request/response models
- Status code examples

Access Swagger UI at: `/swagger`

## Common Workflows

### Citizen Verification Workflow
1. Citizen submits verification request → Receives reference number
2. Citizen uploads identity documents (ID front, back, proof of address)
3. Citizen checks status using reference number
4. Admin reviews queue and approves/rejects
5. On approval: CitizenFederatedProfile created, citizen notified via email
6. Citizen can now link accounts across councils

### Council Registration Workflow
1. Council submits registration (public endpoint) → Receives reference number
2. Council checks status using reference number
3. Admin reviews pending registrations
4. Admin approves → Tenant + admin user created, credentials emailed
5. Council admin logs in, changes password, configures branding
6. Council can now manage citizens and services

### Cross-Council Linking Workflow
1. Citizen creates/links federated profile using National ID
2. System finds existing federated profile or creates new one
3. Links current tenant-citizen to federated profile
4. Citizen grants consent for cross-council data sharing
5. Citizen can view aggregated properties, bills, issues from all councils
6. Citizen can unlink from specific councils at any time

## Notes for Developers

### Important Files
- `ApplicationDbContext.cs` - Database context with multi-tenancy
- `ITenantAccessor.cs` - Gets current tenant ID
- `IMultiTenant.cs` - Interface for tenant-scoped entities
- `NationalIdEncryptionService.cs` - Encryption/hashing for National IDs

### NOT Multi-Tenant
These entities exist at the host level (not tenant-scoped):
- `CitizenFederatedProfile` - Hub for cross-council identity
- `LinkedCitizenProfile` - Links federated profile to tenant-citizens
- `CouncilRegistrationRequest` - Exists before tenant creation

### Production TODOs
1. **Authentication System Integration**
   - Complete User entity creation in TenantInitializationService
   - Implement password hashing
   - Configure roles and permissions

2. **Blob Storage**
   - Integrate Azure Blob Storage or AWS S3
   - Implement actual file uploads in UploadVerificationDocument
   - Implement blob deletion in DocumentExpirationService

3. **Email Configuration**
   - Configure SMTP settings for production
   - Update EmailService to send real emails (currently logs only)

4. **Distributed Cache**
   - Consider Redis for rate limiting cache in multi-instance deployments
   - Consider Redis for federated profile caching

5. **Background Jobs**
   - DocumentExpirationService is implemented as BackgroundService
   - Consider Hangfire for more robust job scheduling

6. **Security Keys**
   - Rotate encryption keys in production
   - Use Azure Key Vault or similar for key management
   - Update hash salt for production

## Reference Documents

- OpenSpec Proposal: `openspec/changes/add-multi-council-identity-linking/`
- Tasks Tracking: `openspec/changes/add-multi-council-identity-linking/tasks.md`
- Design Decisions: `openspec/changes/add-multi-council-identity-linking/design.md`
- Migration Instructions: `MIGRATION_INSTRUCTIONS.md`