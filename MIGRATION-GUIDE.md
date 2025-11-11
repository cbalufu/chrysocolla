# Citizens Portal - ABP Angular Migration Guide

This guide will help you migrate your existing Citizens Portal backend code to a fresh ABP Framework project with Angular frontend.

## Prerequisites

Before starting, ensure you have:
- ✅ .NET 8.0 SDK installed
- ✅ Node.js 18+ and npm installed
- ✅ ABP CLI 8.3.0: `dotnet tool install -g Volo.Abp.Cli --version 8.3.0`
- ✅ Angular CLI: `npm install -g @angular/cli`
- ✅ SQL Server (or connection to SQL Server)

## Step 0: Generate Fresh ABP Project

**Run this command on your local machine:**

```bash
# Navigate to parent directory of your current project
cd /path/to/projects

# Generate new ABP project with Angular
abp new CitizensPortal -t app -u angular -d ef --separate-auth-server -v 8.3.0 --connection-string "Server=localhost;Database=CitizensPortalV2;Trusted_Connection=True;TrustServerCertificate=True"

# This will create:
# - CitizensPortal/ (solution folder)
#   ├── angular/ (Angular frontend)
#   ├── aspnet-core/ (Backend solution)
#   │   ├── src/
#   │   │   ├── CitizensPortal.Domain.Shared
#   │   │   ├── CitizensPortal.Domain
#   │   │   ├── CitizensPortal.Application.Contracts
#   │   │   ├── CitizensPortal.Application
#   │   │   ├── CitizensPortal.EntityFrameworkCore
#   │   │   ├── CitizensPortal.HttpApi
#   │   │   ├── CitizensPortal.HttpApi.Host
#   │   │   ├── CitizensPortal.AuthServer
#   │   │   └── CitizensPortal.DbMigrator
#   │   └── test/
#   │       ├── CitizensPortal.Application.Tests
#   │       ├── CitizensPortal.Domain.Tests
#   │       └── CitizensPortal.TestBase
```

## Expected ABP Project Structure

```
CitizensPortal/
├── angular/                                    # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── proxy/                         # Auto-generated API proxies
│   │   │   ├── home/                          # Home page
│   │   │   ├── account/                       # Login/Register (from ABP)
│   │   │   ├── identity/                      # User/Role management (from ABP)
│   │   │   ├── tenant-management/             # Tenant management (from ABP)
│   │   │   └── app.component.ts
│   │   ├── assets/
│   │   ├── environments/
│   │   │   ├── environment.ts
│   │   │   └── environment.prod.ts
│   │   └── index.html
│   ├── package.json
│   └── angular.json
│
└── aspnet-core/                               # Backend .NET Solution
    ├── src/
    │   ├── CitizensPortal.Domain.Shared/      # Shared constants, enums
    │   │   ├── CitizensPortalDomainSharedModule.cs
    │   │   ├── Localization/
    │   │   │   └── CitizensPortal/
    │   │   │       └── en.json
    │   │   └── Enums/                         # YOUR ENUMS GO HERE
    │   │
    │   ├── CitizensPortal.Domain/             # Domain layer
    │   │   ├── CitizensPortalDomainModule.cs
    │   │   ├── Data/
    │   │   │   ├── ICitizensPortalDbContext.cs
    │   │   │   └── CitizensPortalDbMigrationService.cs
    │   │   ├── Entities/                      # YOUR ENTITIES GO HERE
    │   │   └── OpenIddict/                    # OAuth configuration
    │   │
    │   ├── CitizensPortal.Application.Contracts/  # DTOs and Interfaces
    │   │   ├── CitizensPortalApplicationContractsModule.cs
    │   │   ├── Permissions/
    │   │   │   ├── CitizensPortalPermissions.cs
    │   │   │   └── CitizensPortalPermissionDefinitionProvider.cs
    │   │   ├── DTOs/                          # YOUR DTOS GO HERE
    │   │   └── Services/                      # YOUR SERVICE INTERFACES GO HERE
    │   │
    │   ├── CitizensPortal.Application/        # Application services
    │   │   ├── CitizensPortalApplicationModule.cs
    │   │   ├── CitizensPortalApplicationAutoMapperProfile.cs
    │   │   └── Services/                      # YOUR APP SERVICES GO HERE
    │   │
    │   ├── CitizensPortal.EntityFrameworkCore/    # EF Core layer
    │   │   ├── CitizensPortalEntityFrameworkCoreModule.cs
    │   │   ├── EntityFrameworkCore/
    │   │   │   ├── CitizensPortalDbContext.cs
    │   │   │   ├── CitizensPortalDbContextFactory.cs
    │   │   │   └── EntityConfigurations/      # YOUR EF CONFIGS GO HERE
    │   │   └── Migrations/                    # Will be generated
    │   │
    │   ├── CitizensPortal.HttpApi/            # API Controllers
    │   │   ├── CitizensPortalHttpApiModule.cs
    │   │   └── Controllers/                   # YOUR CONTROLLERS GO HERE
    │   │
    │   ├── CitizensPortal.HttpApi.Host/       # Web API Host
    │   │   ├── CitizensPortalHttpApiHostModule.cs
    │   │   ├── Program.cs
    │   │   ├── appsettings.json
    │   │   └── wwwroot/
    │   │
    │   ├── CitizensPortal.AuthServer/         # Authentication Server
    │   │   ├── CitizensPortalAuthServerModule.cs
    │   │   ├── Program.cs
    │   │   └── appsettings.json
    │   │
    │   └── CitizensPortal.DbMigrator/         # Database migrator
    │       ├── CitizensPortalDbMigratorModule.cs
    │       └── Program.cs
    │
    └── test/
        ├── CitizensPortal.Application.Tests/
        ├── CitizensPortal.Domain.Tests/
        └── CitizensPortal.TestBase/
```

---

## Migration Process Overview

**Total Time Estimate: 3-4 hours**

| Phase | What to Migrate | Time Estimate |
|-------|----------------|---------------|
| Phase 1 | Domain.Shared + Domain | 45 minutes |
| Phase 2 | Application Layer | 60 minutes |
| Phase 3 | API & Infrastructure | 45 minutes |
| Phase 4 | Angular Setup | 30 minutes |
| Phase 5 | Testing & Fixes | 30 minutes |

---

## Phase 1: Migrate Domain Layer (45 minutes)

### 1.1 Migrate Enums (5 minutes)

**Source:** `old/src/CitizensPortal.Domain.Shared/Enums/`
**Destination:** `new/aspnet-core/src/CitizensPortal.Domain.Shared/Enums/`

**Copy these files:**
```bash
# Copy all enum files
cp -r old/src/CitizensPortal.Domain.Shared/Enums/* \
      new/aspnet-core/src/CitizensPortal.Domain.Shared/Enums/
```

**Files to copy:**
- ✅ `ApplicationEnums.cs`
- ✅ `AppointmentEnums.cs`
- ✅ `BillEnums.cs`
- ✅ `CertificateEnums.cs`
- ✅ `CommunityEnums.cs`
- ✅ `DocumentEnums.cs`
- ✅ `EmergencyEnums.cs`
- ✅ `InfrastructureEnums.cs`
- ✅ `IssueEnums.cs`
- ✅ `NotificationEnums.cs`
- ✅ `PaymentEnums.cs`
- ✅ `PropertyEnums.cs`
- ✅ `ServiceRequestEnums.cs`
- ✅ `TicketEnums.cs`

---

### 1.2 Migrate Domain Entities (25 minutes)

**Source:** `old/src/CitizensPortal.Domain/Entities/`
**Destination:** `new/aspnet-core/src/CitizensPortal.Domain/Entities/`

**Copy these files:**
```bash
# Copy all entity files
cp -r old/src/CitizensPortal.Domain/Entities/* \
      new/aspnet-core/src/CitizensPortal.Domain/Entities/
```

**Entities to copy (26 files):**
- ✅ `Citizen.cs`
- ✅ `IssueReport.cs`, `IssueComment.cs`, `IssueAttachment.cs`
- ✅ `Application.cs`, `ApplicationDocument.cs`, `ApplicationStatusHistory.cs`
- ✅ `Bill.cs`
- ✅ `Payment.cs`, `PaymentReceipt.cs`
- ✅ `Document.cs`
- ✅ `Notification.cs`
- ✅ `Property.cs`
- ✅ `Appointment.cs`
- ✅ `ServiceRequest.cs`
- ✅ `Ticket.cs`, `TicketMessage.cs`, `TicketAttachment.cs`
- ✅ `CertificateRequest.cs`
- ✅ `EmergencyAlert.cs`
- ✅ `InfrastructureProject.cs`
- ✅ `Survey.cs`
- ✅ `Consultation.cs`
- ✅ `Vote.cs`
- ✅ `Forum.cs`
- ✅ `Department.cs`
- ✅ `CitizenDashboard.cs`

---

### 1.3 Update CitizensPortalDbContext (10 minutes)

**File:** `new/aspnet-core/src/CitizensPortal.EntityFrameworkCore/EntityFrameworkCore/CitizensPortalDbContext.cs`

**Add DbSets for all your entities:**

```csharp
public class CitizensPortalDbContext :
    AbpDbContext<CitizensPortalDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    // YOUR ENTITIES - Add these DbSets
    public DbSet<Citizen> Citizens { get; set; }
    public DbSet<IssueReport> IssueReports { get; set; }
    public DbSet<IssueComment> IssueComments { get; set; }
    public DbSet<IssueAttachment> IssueAttachments { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<ApplicationDocument> ApplicationDocuments { get; set; }
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentReceipt> PaymentReceipts { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketMessage> TicketMessages { get; set; }
    public DbSet<TicketAttachment> TicketAttachments { get; set; }
    public DbSet<CertificateRequest> CertificateRequests { get; set; }
    public DbSet<EmergencyAlert> EmergencyAlerts { get; set; }
    public DbSet<InfrastructureProject> InfrastructureProjects { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
    public DbSet<SurveyResponse> SurveyResponses { get; set; }
    public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<ForumTopic> ForumTopics { get; set; }
    public DbSet<ForumPost> ForumPosts { get; set; }
    public DbSet<ForumReply> ForumReplies { get; set; }
    public DbSet<Department> Departments { get; set; }

    // ... rest of ABP's default DbSets ...

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure ABP modules
        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();

        // YOUR ENTITY CONFIGURATIONS
        // You can add configurations here or in separate files
    }
}
```

---

### 1.4 Add Blob Storage Configuration (5 minutes)

**File:** `new/aspnet-core/src/CitizensPortal.Domain/CitizensPortalDomainModule.cs`

**Add to ConfigureServices:**

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    base.ConfigureServices(context);

    // Add Blob Storage
    Configure<AbpBlobStoringOptions>(options =>
    {
        options.Containers.ConfigureDefault(container =>
        {
            container.UseFileSystem(fileSystem =>
            {
                fileSystem.BasePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "blob-storage");
            });
        });
    });
}
```

**Add NuGet package:**
```bash
cd new/aspnet-core/src/CitizensPortal.Domain
dotnet add package Volo.Abp.BlobStoring.FileSystem --version 8.3.0
```

---

## Phase 2: Migrate Application Layer (60 minutes)

### 2.1 Migrate DTOs (15 minutes)

**Source:** `old/src/CitizensPortal.Application.Contracts/DTOs/`
**Destination:** `new/aspnet-core/src/CitizensPortal.Application.Contracts/DTOs/`

**Copy all DTO folders:**
```bash
cp -r old/src/CitizensPortal.Application.Contracts/DTOs/* \
      new/aspnet-core/src/CitizensPortal.Application.Contracts/DTOs/
```

**DTO directories to copy:**
- ✅ `Citizens/`
- ✅ `Issues/`
- ✅ `Applications/`
- ✅ `Bills/`
- ✅ `Payments/`
- ✅ `Documents/`
- ✅ `Notifications/`
- ✅ `Properties/`
- ✅ `Appointments/`
- ✅ `ServiceRequests/`
- ✅ `Tickets/`
- ✅ `Certificates/`
- ✅ `EmergencyAlerts/`
- ✅ `Infrastructure/`
- ✅ `Community/` (Surveys, Consultations, Votes, Forums)

---

### 2.2 Migrate Service Interfaces (10 minutes)

**Source:** `old/src/CitizensPortal.Application.Contracts/Services/`
**Destination:** `new/aspnet-core/src/CitizensPortal.Application.Contracts/Services/`

**Copy all service interface files:**
```bash
cp -r old/src/CitizensPortal.Application.Contracts/Services/* \
      new/aspnet-core/src/CitizensPortal.Application.Contracts/Services/
```

**Fix this known issue in `IInfrastructureProjectAppService.cs`:**

```csharp
// BEFORE (wrong):
Task<List<InfrastructureProjectDto>> GetProjectsByCategoryAsync(Domain.Enums.ProjectCategory category);

// AFTER (correct):
Task<List<InfrastructureProjectDto>> GetProjectsByCategoryAsync(Domain.Shared.Enums.ProjectCategory category);
// OR simply:
Task<List<InfrastructureProjectDto>> GetProjectsByCategoryAsync(ProjectCategory category);
```

---

### 2.3 Migrate Permissions (10 minutes)

**Source:** `old/src/CitizensPortal.Application.Contracts/Permissions/`
**Destination:** `new/aspnet-core/src/CitizensPortal.Application.Contracts/Permissions/`

**Files to copy:**
```bash
cp old/src/CitizensPortal.Application.Contracts/Permissions/CitizensPortalPermissions.cs \
   new/aspnet-core/src/CitizensPortal.Application.Contracts/Permissions/

cp old/src/CitizensPortal.Application.Contracts/Permissions/CitizensPortalPermissionDefinitionProvider.cs \
   new/aspnet-core/src/CitizensPortal.Application.Contracts/Permissions/
```

---

### 2.4 Migrate Application Services (20 minutes)

**Source:** `old/src/CitizensPortal.Application/Services/`
**Destination:** `new/aspnet-core/src/CitizensPortal.Application/Services/`

**Copy all application service files:**
```bash
cp -r old/src/CitizensPortal.Application/Services/* \
      new/aspnet-core/src/CitizensPortal.Application/Services/
```

**Services to copy (18 files):**
- ✅ `CitizenAppService.cs`
- ✅ `IssueReportAppService.cs`
- ✅ `ApplicationAppService.cs`
- ✅ `BillAppService.cs`
- ✅ `PaymentAppService.cs`
- ✅ `DocumentAppService.cs`
- ✅ `NotificationAppService.cs`
- ✅ `PropertyAppService.cs`
- ✅ `AppointmentAppService.cs`
- ✅ `ServiceRequestAppService.cs`
- ✅ `TicketAppService.cs`
- ✅ `CertificateRequestAppService.cs`
- ✅ `EmergencyAlertAppService.cs`
- ✅ `InfrastructureProjectAppService.cs`
- ✅ `SurveyAppService.cs`
- ✅ `ConsultationAppService.cs`
- ✅ `VoteAppService.cs`
- ✅ `ForumAppService.cs`

---

### 2.5 Update AutoMapper Profile (5 minutes)

**File:** `new/aspnet-core/src/CitizensPortal.Application/CitizensPortalApplicationAutoMapperProfile.cs`

**Copy your AutoMapper configuration:**

```bash
# If you have custom AutoMapper profile, copy it:
cp old/src/CitizensPortal.Application/CitizensPortalApplicationAutoMapperProfile.cs \
   new/aspnet-core/src/CitizensPortal.Application/
```

---

## Phase 3: Migrate API & Infrastructure (45 minutes)

### 3.1 Migrate HTTP Controllers (15 minutes)

**Source:** `old/src/CitizensPortal.HttpApi/Controllers/`
**Destination:** `new/aspnet-core/src/CitizensPortal.HttpApi/Controllers/`

**Copy existing controllers:**
```bash
cp old/src/CitizensPortal.HttpApi/Controllers/*.cs \
   new/aspnet-core/src/CitizensPortal.HttpApi/Controllers/
```

**Controllers to copy (8 files):**
- ✅ `CitizenController.cs`
- ✅ `IssueReportController.cs`
- ✅ `DocumentController.cs`
- ✅ `PaymentController.cs`
- ✅ `PropertyController.cs`
- ✅ `AppointmentController.cs`
- ✅ `ServiceRequestController.cs`
- ✅ `EmergencyAlertController.cs`

**Create missing controllers for services without HTTP APIs (10 services):**
You'll need to create controllers for:
- BillController
- NotificationController
- CertificateRequestController
- TicketController
- InfrastructureProjectController
- SurveyController
- ConsultationController
- VoteController
- ForumController
- ApplicationController (if missing)

---

### 3.2 Migrate Localization Resources (5 minutes)

**Source:** `old/src/CitizensPortal.Domain.Shared/Localization/CitizensPortal/`
**Destination:** `new/aspnet-core/src/CitizensPortal.Domain.Shared/Localization/CitizensPortal/`

**Copy localization files:**
```bash
cp old/src/CitizensPortal.Domain.Shared/Localization/CitizensPortal/en.json \
   new/aspnet-core/src/CitizensPortal.Domain.Shared/Localization/CitizensPortal/
```

---

### 3.3 Update HttpApi.Host Configuration (10 minutes)

**File:** `new/aspnet-core/src/CitizensPortal.HttpApi.Host/appsettings.json`

**Update connection string:**
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=CitizensPortalV2;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "AuthServer": {
    "Authority": "https://localhost:44334",
    "RequireHttpsMetadata": "false"
  },
  "App": {
    "SelfUrl": "https://localhost:44300",
    "ClientUrl": "http://localhost:4200",
    "CorsOrigins": "http://localhost:4200,https://localhost:44307",
    "RedirectAllowedUrls": "http://localhost:4200,https://localhost:44307"
  }
}
```

**File:** `new/aspnet-core/src/CitizensPortal.HttpApi.Host/CitizensPortalHttpApiHostModule.cs`

**Add Blob Storage configuration:**

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();
    var hostingEnvironment = context.Services.GetHostingEnvironment();

    // Add Blob Storage
    Configure<AbpBlobStoringOptions>(options =>
    {
        options.Containers.ConfigureDefault(container =>
        {
            container.UseFileSystem(fileSystem =>
            {
                var basePath = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot", "blob-storage");
                fileSystem.BasePath = basePath;
            });
        });
    });

    // ... rest of configuration
}
```

---

### 3.4 Create Database Migration (10 minutes)

**Run these commands:**

```bash
cd new/aspnet-core/src/CitizensPortal.EntityFrameworkCore

# Create initial migration
dotnet ef migrations add InitialCreate

# This will generate a migration with all your entities
```

---

### 3.5 Run Database Migrator (5 minutes)

**File:** `new/aspnet-core/src/CitizensPortal.DbMigrator/appsettings.json`

**Update connection string:**
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=CitizensPortalV2;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Run migrator:**
```bash
cd new/aspnet-core/src/CitizensPortal.DbMigrator
dotnet run
```

This will:
- Create the database
- Apply migrations
- Seed admin user (admin / 1q2w3E*)
- Seed initial data

---

## Phase 4: Angular Frontend Setup (30 minutes)

### 4.1 Install Dependencies (5 minutes)

```bash
cd new/angular
npm install
```

---

### 4.2 Update Environment Configuration (5 minutes)

**File:** `new/angular/src/environments/environment.ts`

```typescript
export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200',
    name: 'CitizensPortal',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44334/',
    redirectUri: window.location.origin,
    clientId: 'CitizensPortal_App',
    responseType: 'code',
    scope: 'offline_access CitizensPortal',
    requireHttps: false,
  },
  apis: {
    default: {
      url: 'https://localhost:44300',
      rootNamespace: 'CitizensPortal',
    },
  },
};
```

---

### 4.3 Generate TypeScript API Proxies (10 minutes)

**This auto-generates TypeScript services for your APIs:**

```bash
cd new/angular

# Make sure backend is running first!
cd ../aspnet-core/src/CitizensPortal.HttpApi.Host
dotnet run &

# Wait for backend to start, then generate proxies
cd ../../angular
abp generate-proxy -t ng
```

This creates:
- `src/app/proxy/citizens/` - Citizen services
- `src/app/proxy/issues/` - Issue report services
- `src/app/proxy/documents/` - Document services
- `src/app/proxy/payments/` - Payment services
- ... (all your services)

---

### 4.4 Create Angular Feature Modules (10 minutes)

**Generate modules for your features:**

```bash
cd new/angular

# Generate modules for each feature
ng generate module citizens --routing
ng generate module issues --routing
ng generate module applications --routing
ng generate module billing --routing
ng generate module documents --routing
ng generate module properties --routing
ng generate module appointments --routing
ng generate module certificates --routing
ng generate module infrastructure --routing
ng generate module community --routing
ng generate module emergency-alerts --routing

# Generate components (example for citizens)
ng generate component citizens/citizen-list
ng generate component citizens/citizen-detail
ng generate component issues/issue-list
ng generate component issues/issue-create
# ... repeat for each feature
```

---

## Phase 5: Testing & Verification (30 minutes)

### 5.1 Build Backend

```bash
cd new/aspnet-core
dotnet build
```

**Expected result:** ✅ Build succeeded with 0 errors

---

### 5.2 Start Backend Services

**Terminal 1 - Auth Server:**
```bash
cd new/aspnet-core/src/CitizensPortal.AuthServer
dotnet run
# Should start on https://localhost:44334
```

**Terminal 2 - API Host:**
```bash
cd new/aspnet-core/src/CitizensPortal.HttpApi.Host
dotnet run
# Should start on https://localhost:44300
```

---

### 5.3 Start Angular App

**Terminal 3:**
```bash
cd new/angular
npm start
# Should start on http://localhost:4200
```

---

### 5.4 Test Login

1. Navigate to: http://localhost:4200
2. Click "Login"
3. Use default credentials:
   - Username: `admin`
   - Password: `1q2w3E*`
4. You should see the dashboard

---

### 5.5 Test API Endpoints

**Access Swagger:**
- Navigate to: https://localhost:44300/swagger
- Authorize with admin credentials
- Test your API endpoints

**Key endpoints to test:**
- `GET /api/citizens` - List citizens
- `GET /api/issue-reports` - List issues
- `GET /api/documents` - List documents
- `GET /api/payments` - List payments

---

## Post-Migration Checklist

### ✅ Backend Verification

- [ ] All 26 entities migrated
- [ ] All 18 application services working
- [ ] All 8 existing controllers functional
- [ ] Created missing 10 controllers
- [ ] Database migration successful
- [ ] Blob storage configured
- [ ] Email settings configured
- [ ] Multi-tenancy working
- [ ] Permissions applied correctly
- [ ] Swagger UI accessible

### ✅ Frontend Verification

- [ ] Angular app starts successfully
- [ ] Login/logout working
- [ ] API proxies generated
- [ ] Can view existing ABP modules (Users, Roles, Tenants)
- [ ] Custom feature modules created
- [ ] Routing configured
- [ ] CORS configured correctly

---

## Common Issues & Solutions

### Issue 1: Build Errors After Migration

**Problem:** Namespace errors or missing references

**Solution:**
```bash
# Clean and rebuild
cd new/aspnet-core
dotnet clean
dotnet restore
dotnet build
```

### Issue 2: Database Migration Fails

**Problem:** EF Core can't find DbContext

**Solution:**
```bash
# Ensure you're in the EntityFrameworkCore project
cd src/CitizensPortal.EntityFrameworkCore

# Specify startup project
dotnet ef migrations add InitialCreate \
  --startup-project ../CitizensPortal.DbMigrator
```

### Issue 3: Angular Can't Connect to API

**Problem:** CORS errors in browser console

**Solution:** Update `appsettings.json` in HttpApi.Host:
```json
{
  "App": {
    "CorsOrigins": "http://localhost:4200"
  }
}
```

### Issue 4: OpenIddict Configuration Issues

**Problem:** OAuth redirect errors

**Solution:** Ensure URLs match in:
1. `appsettings.json` (HttpApi.Host)
2. `appsettings.json` (AuthServer)
3. `environment.ts` (Angular)

### Issue 5: Blob Storage Path Not Found

**Problem:** Document upload fails

**Solution:** Create directory manually:
```bash
mkdir -p new/aspnet-core/src/CitizensPortal.HttpApi.Host/wwwroot/blob-storage
```

---

## Performance Optimization (Optional)

### Enable Response Compression

**File:** `CitizensPortalHttpApiHostModule.cs`

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // Add Response Compression
    context.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });
}
```

### Enable Response Caching

```csharp
// In controllers
[ResponseCache(Duration = 60)] // Cache for 60 seconds
public async Task<List<InfrastructureProjectDto>> GetPublicProjectsAsync()
{
    // ...
}
```

---

## Next Steps After Migration

### 1. Build Custom Angular Components

For each feature, you'll need to create:
- List pages (data tables)
- Create/Edit forms
- Detail views
- Search/filter components

**Example structure:**
```
citizens/
├── citizen-list/
│   ├── citizen-list.component.ts
│   ├── citizen-list.component.html
│   └── citizen-list.component.scss
├── citizen-create/
├── citizen-edit/
└── citizen-detail/
```

### 2. Add Missing HTTP Controllers

Create controllers for services without APIs:
- BillController
- NotificationController
- InfrastructureProjectController
- SurveyController
- ConsultationController
- VoteController
- ForumController
- TicketController
- CertificateRequestController

### 3. Customize Theme

ABP includes LeptonX Lite theme. Customize it:
- Update logo in `angular/src/assets/`
- Modify colors in `styles.scss`
- Add custom CSS

### 4. Configure Email

**File:** `appsettings.json`

```json
{
  "Settings": {
    "Abp.Mailing.Smtp.Host": "smtp.gmail.com",
    "Abp.Mailing.Smtp.Port": "587",
    "Abp.Mailing.Smtp.UserName": "your-email@gmail.com",
    "Abp.Mailing.Smtp.Password": "your-password",
    "Abp.Mailing.Smtp.EnableSsl": "true",
    "Abp.Mailing.DefaultFromAddress": "noreply@yourmuni.gov.za"
  }
}
```

### 5. Add GIS/Mapping

For infrastructure projects and issue reporting:

```bash
cd new/angular
npm install leaflet @types/leaflet
```

### 6. Setup CI/CD

Consider setting up:
- GitHub Actions / Azure DevOps
- Automated testing
- Docker containers
- Kubernetes deployment

---

## Estimated Timeline

| Task | Time |
|------|------|
| Generate ABP project | 5 min |
| Phase 1: Domain migration | 45 min |
| Phase 2: Application migration | 60 min |
| Phase 3: API & Infrastructure | 45 min |
| Phase 4: Angular setup | 30 min |
| Phase 5: Testing | 30 min |
| **Total** | **~3.5 hours** |

---

## Support & Documentation

- **ABP Documentation:** https://docs.abp.io
- **ABP Angular Documentation:** https://docs.abp.io/en/abp/latest/UI/Angular/Quick-Start
- **ABP Community:** https://community.abp.io
- **OpenSpec Documentation:** Your `openspec/` folder

---

## Summary

By following this guide, you will:

✅ Have a **professional ABP Framework + Angular** application
✅ Keep **100% of your existing backend logic**
✅ Get **pre-built admin modules** (Users, Roles, Tenants)
✅ Have **auto-generated TypeScript API clients**
✅ Get **responsive UI with LeptonX theme**
✅ Support **multi-tenancy out of the box**
✅ Have **proper authentication & authorization**

The migration is straightforward—mostly copy-paste with a few configuration updates. The hardest part is creating the Angular UI components, but ABP provides excellent scaffolding and examples to follow.

Good luck with your migration! 🚀
