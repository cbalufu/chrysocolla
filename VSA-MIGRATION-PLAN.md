# Citizens Portal - Vertical Slice Architecture Migration Plan

**Migration**: From ABP Framework to ASP.NET Core Minimal APIs with Vertical Slice Architecture
**Frontend**: React with TypeScript
**Architecture**: Vertical Slice Architecture (VSA) + CQRS
**Multi-Tenancy**: Shared Database with Row-Level Isolation
**Date**: 2025-11-11

---

## Table of Contents
1. [Executive Summary](#executive-summary)
2. [Architecture Overview](#architecture-overview)
3. [Technology Stack](#technology-stack)
4. [Project Structure](#project-structure)
5. [Multi-Tenancy Strategy](#multi-tenancy-strategy)
6. [Migration Approach](#migration-approach)
7. [Implementation Phases](#implementation-phases)
8. [Feature Slices](#feature-slices)

---

## Executive Summary

### Current State
- **Framework**: ABP Framework 8.3.0 (heavily layered, DDD-focused)
- **Issues**:
  - Network restrictions preventing ABP template downloads
  - 50+ build errors revealing structural mismatches
  - Over-engineered for current needs
  - No frontend UI
  - Hand-coded entities/services don't align with ABP conventions

### Target State
- **Architecture**: Vertical Slice Architecture with Minimal APIs
- **Benefits**:
  - **Simpler**: Each feature is self-contained, easier to understand
  - **Faster**: Minimal APIs are lightweight, ~30% performance improvement
  - **Maintainable**: Features can be modified independently
  - **Flexible**: Mix technologies per slice (EF Core, Dapper, etc.)
  - **Team-friendly**: Multiple developers can work on different slices without conflicts

---

## Architecture Overview

### Vertical Slice Architecture Principles

VSA organizes code by **feature/use case** instead of technical layers:

```
Traditional Layered (ABP)     →     Vertical Slice Architecture
─────────────────────────────        ───────────────────────────
Controllers                          Features/
  └─ CitizenController                ├─ Citizens/
Services                              │   ├─ Register/
  └─ CitizenAppService                │   │   ├─ Register.cs (Endpoint)
DTOs                                  │   │   ├─ RegisterCommand.cs
  └─ CitizenDto                       │   │   ├─ RegisterValidator.cs
Domain                                │   │   └─ RegisterResponse.cs
  └─ Citizen.cs                       │   ├─ GetProfile/
Repositories                          │   │   ├─ GetProfile.cs
  └─ CitizenRepository                │   │   ├─ GetProfileQuery.cs
                                      │   │   └─ ProfileResponse.cs
❌ Changes spread across layers       ✅ All code for one feature in one place
```

### Key Patterns

1. **REPR Pattern** (Request-Endpoint-Response)
   - **Request**: Input contract (Command/Query)
   - **Endpoint**: Minimal API endpoint + handler
   - **Response**: Output contract (DTO)

2. **CQRS** (Command Query Responsibility Segregation)
   - Commands: Mutate state (POST, PUT, DELETE)
   - Queries: Read state (GET)
   - MediatR pipelines for cross-cutting concerns

3. **Feature Folders**
   - Each feature contains everything it needs
   - Minimize coupling between features
   - Maximize coupling within a feature

---

## Technology Stack

### Backend (.NET 9)
| Technology | Purpose | Version |
|------------|---------|---------|
| ASP.NET Core | Web framework | 9.0 |
| Minimal APIs | Lightweight HTTP APIs | 9.0 |
| MediatR | In-process messaging (CQRS) | 12.x |
| FluentValidation | Request validation | 11.x |
| Entity Framework Core | ORM (primary data access) | 9.0 |
| Dapper | Micro-ORM (read-heavy queries) | 2.1+ |
| Serilog | Structured logging | 8.x |
| Carter | Minimal API organization | 8.x |
| FastEndpoints | Alternative to Carter (TBD) | 5.x |

### Frontend (React 18+)
| Technology | Purpose | Version |
|------------|---------|---------|
| React | UI framework | 18.3+ |
| TypeScript | Type safety | 5.x |
| Vite | Build tool | 5.x |
| TanStack Query | Data fetching/caching | 5.x |
| React Router | SPA routing | 6.x |
| Zustand | State management | 4.x |
| Tailwind CSS | Styling | 3.x |
| Shadcn UI | Component library | Latest |

### Database & Infrastructure
| Technology | Purpose |
|------------|---------|
| SQL Server | Primary database |
| Redis | Caching, session storage |
| Seq | Centralized logging |
| OpenTelemetry | Observability |

---

## Project Structure

### Solution Structure

```
CitizensPortal/
├── src/
│   ├── CitizensPortal.Api/              # Minimal API host
│   │   ├── Program.cs                   # Application entry point
│   │   ├── appsettings.json
│   │   ├── Features/                    # All feature slices
│   │   │   ├── Citizens/
│   │   │   ├── Issues/
│   │   │   ├── Applications/
│   │   │   └── ... (18 features)
│   │   ├── Infrastructure/              # Shared infrastructure
│   │   │   ├── Database/
│   │   │   │   ├── ApplicationDbContext.cs
│   │   │   │   ├── Migrations/
│   │   │   │   └── Configurations/
│   │   │   ├── MultiTenancy/
│   │   │   │   ├── TenantMiddleware.cs
│   │   │   │   ├── TenantAccessor.cs
│   │   │   │   └─ TenantQueryFilter.cs
│   │   │   ├── Auth/
│   │   │   ├── Behaviors/               # MediatR pipeline behaviors
│   │   │   ├── Filters/
│   │   │   └── Extensions/
│   │   └── Contracts/                   # Shared contracts
│   │       ├── Common/
│   │       │   ├─ Result.cs
│   │       │   ├─ PagedResult.cs
│   │       │   └─ Error.cs
│   │       └── Enums/
│   │
│   └── CitizensPortal.Web/              # React frontend
│       ├── public/
│       ├── src/
│       │   ├── features/                # Feature-based organization
│       │   │   ├── citizens/
│       │   │   ├── issues/
│       │   │   └── ...
│       │   ├── components/              # Shared components
│       │   ├── hooks/                   # Shared hooks
│       │   ├── services/                # API clients
│       │   ├── lib/                     # Utilities
│       │   └── App.tsx
│       ├── package.json
│       ├── tsconfig.json
│       └── vite.config.ts
│
└── tests/
    ├── CitizensPortal.Api.Tests/
    └── CitizensPortal.Integration.Tests/
```

### Feature Slice Structure (Example: Citizen Registration)

```
Features/Citizens/Register/
├── Register.cs                          # Minimal API endpoint
├── RegisterCommand.cs                   # Request contract
├── RegisterCommandHandler.cs            # Business logic
├── RegisterCommandValidator.cs          # Validation rules
├── RegisterResponse.cs                  # Response contract
└── RegisterMapper.cs                    # Mapping (if needed)
```

**Register.cs** (Minimal API Endpoint):
```csharp
public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/citizens/register", async (
            RegisterCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithName("RegisterCitizen")
        .WithTags("Citizens")
        .WithOpenApi();
    }
}
```

**RegisterCommand.cs**:
```csharp
public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string NationalId,
    string PhoneNumber
) : IRequest<Result<RegisterResponse>>;
```

**RegisterCommandHandler.cs**:
```csharp
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITenantAccessor _tenantAccessor;

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand command,
        CancellationToken ct)
    {
        // 1. Check if email exists
        var exists = await _context.Citizens
            .AnyAsync(c => c.Email == command.Email, ct);

        if (exists)
            return Result.Failure<RegisterResponse>(
                new Error("Citizen.EmailExists", "Email already registered"));

        // 2. Create citizen entity
        var citizen = new Citizen
        {
            Email = command.Email,
            PasswordHash = _passwordHasher.Hash(command.Password),
            FirstName = command.FirstName,
            LastName = command.LastName,
            NationalId = command.NationalId,
            PhoneNumber = command.PhoneNumber,
            TenantId = _tenantAccessor.TenantId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Citizens.Add(citizen);
        await _context.SaveChangesAsync(ct);

        // 3. Return response
        return Result.Success(new RegisterResponse(
            citizen.Id,
            citizen.Email,
            $"{citizen.FirstName} {citizen.LastName}"
        ));
    }
}
```

---

## Multi-Tenancy Strategy

### Approach: Shared Database with Row-Level Isolation

**Why This Approach?**
- ✅ Cost-effective: Single database, simpler infrastructure
- ✅ Easy to maintain: One schema, one backup strategy
- ✅ Scalable: Handle thousands of tenants
- ✅ Performance: Indexed tenant columns + EF Core global filters
- ✅ Data isolation: Automatic filtering via query filters

### Implementation

#### 1. Tenant Entity
```csharp
public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Subdomain { get; set; }
    public string ConnectionString { get; set; } // For future migration to multi-DB
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### 2. Multi-Tenant Entities
```csharp
public interface IMultiTenant
{
    Guid? TenantId { get; set; }
}

public class Citizen : IMultiTenant
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; } // Tenant isolation
    public string Email { get; set; }
    // ... other properties
}
```

#### 3. Tenant Resolution Middleware
```csharp
public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public async Task InvokeAsync(HttpContext context, ITenantAccessor tenantAccessor)
    {
        // Resolve tenant from subdomain, header, or JWT claim
        var tenantId = ResolveTenantId(context);
        tenantAccessor.SetTenant(tenantId);

        await _next(context);
    }

    private Guid? ResolveTenantId(HttpContext context)
    {
        // Priority 1: JWT claim
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = context.User.FindFirst("tenant_id");
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tid))
                return tid;
        }

        // Priority 2: Subdomain (tenant1.citizensportal.com)
        var host = context.Request.Host.Host;
        var parts = host.Split('.');
        if (parts.Length > 2)
        {
            var subdomain = parts[0];
            // Lookup tenant by subdomain from cache/db
            return GetTenantBySubdomain(subdomain);
        }

        // Priority 3: X-Tenant-Id header
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantHeader))
        {
            if (Guid.TryParse(tenantHeader, out var tid))
                return tid;
        }

        return null;
    }
}
```

#### 4. EF Core Global Query Filter
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Apply global query filter to all multi-tenant entities
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
        if (typeof(IMultiTenant).IsAssignableFrom(entityType.ClrType))
        {
            var method = SetGlobalQueryMethod.MakeGenericMethod(entityType.ClrType);
            method.Invoke(this, new object[] { modelBuilder });
        }
    }
}

private static readonly MethodInfo SetGlobalQueryMethod =
    typeof(ApplicationDbContext).GetMethod(
        nameof(SetGlobalQuery),
        BindingFlags.NonPublic | BindingFlags.Static);

private static void SetGlobalQuery<T>(ModelBuilder builder) where T : class, IMultiTenant
{
    builder.Entity<T>().HasQueryFilter(e => e.TenantId == _tenantAccessor.TenantId);
}
```

### Tenant Isolation Guarantees

✅ **Automatic Filtering**: All queries automatically filter by TenantId
✅ **Write Protection**: Save interceptor ensures TenantId is set on create
✅ **Migration Safety**: Can move to database-per-tenant later
✅ **Performance**: Indexed TenantId column on all tables

---

## Migration Approach

### Option A: Incremental Feature Migration (Recommended)

**Duration**: 2-3 weeks
**Risk**: Low
**Effort**: Medium

**Process**:
1. Create new ASP.NET Core 9 Minimal API project
2. Migrate shared infrastructure (database, entities, multi-tenancy)
3. Migrate features one-by-one as vertical slices
4. Keep old ABP project running until all features migrated
5. Decommission ABP project

**Benefits**:
- ✅ Lower risk - test each feature incrementally
- ✅ Learn VSA patterns as you go
- ✅ Maintain working system during migration

### Option B: Fresh Rewrite (Faster but Higher Risk)

**Duration**: 1 week
**Risk**: Medium
**Effort**: High (intense)

**Process**:
1. Create new solution from scratch
2. Set up all infrastructure upfront
3. Migrate all 18 features in parallel (if multiple developers)
4. Cutover weekend

---

## Implementation Phases

### Phase 1: Foundation (2 days)

**Tasks**:
- [ ] Create new ASP.NET Core 9 solution with Minimal APIs
- [ ] Set up project structure (Api, Web, Tests)
- [ ] Install core packages (MediatR, FluentValidation, Carter, EF Core, Serilog)
- [ ] Configure Program.cs with middleware pipeline
- [ ] Set up multi-tenancy infrastructure:
  - [ ] ITenantAccessor service
  - [ ] TenantMiddleware
  - [ ] EF Core global query filters
- [ ] Create ApplicationDbContext with migrations
- [ ] Set up shared contracts (Result, Error, PagedResult)
- [ ] Create MediatR pipeline behaviors:
  - [ ] ValidationBehavior
  - [ ] LoggingBehavior
  - [ ] UnitOfWorkBehavior
- [ ] Set up React app with Vite, TypeScript, TanStack Query
- [ ] Configure API client with tenant support

**Deliverables**:
- ✅ Empty project compiles and runs
- ✅ Health check endpoint working
- ✅ Database migrations run successfully
- ✅ React dev server running
- ✅ Multi-tenancy resolves correctly

### Phase 2: Core Entities & Authentication (3 days)

**Tasks**:
- [ ] Migrate domain entities (26 entities)
  - Add IMultiTenant interface where needed
  - Configure EF Core entity configurations
  - Create indexes for TenantId + common query patterns
- [ ] Implement authentication feature slice:
  - [ ] POST /auth/login (JWT token generation)
  - [ ] POST /auth/refresh (refresh token)
  - [ ] POST /auth/logout
  - [ ] GET /auth/me (current user)
- [ ] Implement tenant management:
  - [ ] GET /tenants/:id
  - [ ] POST /tenants (create tenant)
  - [ ] PUT /tenants/:id
- [ ] React authentication:
  - [ ] Login page
  - [ ] Auth context provider
  - [ ] Protected routes
  - [ ] Tenant switcher (if user has multiple tenants)

**Deliverables**:
- ✅ All entities in database
- ✅ Authentication working end-to-end
- ✅ JWT tokens include tenant_id claim
- ✅ User can log in via React UI

### Phase 3: Migrate Feature Slices (8 days, ~2 features/day)

Migrate each of the 18 features as vertical slices:

#### Week 1 (Days 1-5):
1. **Citizens** (Day 1)
   - Register, GetProfile, UpdateProfile, Search
2. **Issues** (Day 1-2)
   - ReportIssue, GetIssue, ListIssues, UpdateStatus, AddComment, UploadImage
3. **Applications** (Day 2-3)
   - CreateApplication, GetApplication, ListApplications, SubmitApplication, ReviewApplication
4. **Billing** (Day 3)
   - GenerateBill, GetBill, ListBills
5. **Payments** (Day 4)
   - CreatePayment, ProcessPayment, GetReceipt, ListPayments
6. **Documents** (Day 4-5)
   - UploadDocument, DownloadDocument, ListDocuments, SignDocument
7. **Notifications** (Day 5)
   - SendNotification, ListNotifications, MarkAsRead

#### Week 2 (Days 6-10):
8. **Properties** (Day 6)
   - RegisterProperty, GetProperty, ListProperties
9. **Appointments** (Day 6-7)
   - BookAppointment, GetAvailableSlots, ListAppointments, CancelAppointment
10. **Service Requests** (Day 7)
    - CreateRequest, GetRequest, ListRequests, UpdateRequest
11. **Support Tickets** (Day 8)
    - CreateTicket, GetTicket, ListTickets, AddMessage, ResolveTicket
12. **Certificates** (Day 8-9)
    - RequestCertificate, GetCertificate, VerifyCertificate, ListRequests
13. **Emergency Alerts** (Day 9)
    - CreateAlert, GetActiveAlerts, AcknowledgeAlert, GetEvacuationRoutes
14. **Infrastructure Projects** (Day 9-10)
    - CreateProject, GetProject, ListProjects, AddUpdate
15. **Surveys** (Day 10)
    - CreateSurvey, GetSurvey, SubmitResponse
16. **Consultations** (Day 10)
    - CreateConsultation, GetConsultation, SubmitFeedback
17. **Voting** (Day 10)
    - CreatePoll, GetPoll, CastVote
18. **Forums** (Day 10)
    - CreateTopic, GetTopic, CreatePost, ListTopics

**Per-Feature Checklist**:
- [ ] Create feature folder
- [ ] Define commands/queries with MediatR
- [ ] Implement validators (FluentValidation)
- [ ] Implement handlers with business logic
- [ ] Create minimal API endpoints
- [ ] Write unit tests
- [ ] Create React components/pages
- [ ] Create API client hooks (TanStack Query)
- [ ] Test end-to-end

### Phase 4: Testing & Polish (2 days)

**Tasks**:
- [ ] Integration tests for all feature slices
- [ ] Load testing with multi-tenancy
- [ ] Security testing (tenant isolation verification)
- [ ] Performance optimization
- [ ] Error handling and logging review
- [ ] API documentation (Swagger/Scalar)
- [ ] React UI polish and responsive design

---

## Feature Slices

### 18 Feature Categories

| # | Feature | Complexity | Endpoints | Priority |
|---|---------|------------|-----------|----------|
| 1 | Citizens | Medium | 5 | P0 - Core |
| 2 | Issues | High | 8 | P0 - Core |
| 3 | Applications | High | 7 | P0 - Core |
| 4 | Billing | Medium | 4 | P0 - Core |
| 5 | Payments | High | 6 | P0 - Core |
| 6 | Documents | Medium | 6 | P1 |
| 7 | Notifications | Low | 4 | P1 |
| 8 | Properties | Medium | 4 | P1 |
| 9 | Appointments | Medium | 6 | P1 |
| 10 | Service Requests | Medium | 5 | P2 |
| 11 | Support Tickets | Medium | 7 | P2 |
| 12 | Certificates | Medium | 5 | P2 |
| 13 | Emergency Alerts | High | 6 | P0 - Core |
| 14 | Infrastructure Projects | Medium | 5 | P3 |
| 15 | Surveys | Medium | 5 | P3 |
| 16 | Consultations | Medium | 4 | P3 |
| 17 | Voting | Medium | 5 | P3 |
| 18 | Forums | Medium | 6 | P3 |

**Total**: ~98 endpoints

---

## Next Steps

1. **Review & Approve Plan**: Discuss and finalize approach
2. **Set Up Environment**: Install .NET 9 SDK, Node.js 20+
3. **Start Phase 1**: Create new solution structure
4. **Migrate Incrementally**: Follow phase plan

---

## References

- **VSA by Jimmy Bogard**: https://www.jimmybogard.com/vertical-slice-architecture/
- **Minimal APIs**: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis
- **Carter**: https://github.com/CarterCommunity/Carter
- **MediatR**: https://github.com/jbogard/MediatR
- **Multi-Tenancy in EF Core**: https://learn.microsoft.com/en-us/ef/core/miscellaneous/multitenancy
- **TanStack Query**: https://tanstack.com/query/latest
