# Project Context

## Purpose
Citizens Portal is a comprehensive municipal e-government/smart city platform designed for digital service delivery. It enables citizens to interact with municipal services, report issues, apply for permits, pay bills, participate in community engagement, and track infrastructure projects. The platform supports multi-tenancy to serve multiple municipalities from a single deployment.

## Tech Stack
- **Backend Framework**: ABP Framework 8.3.0 (.NET 8.0)
- **Architecture**: Clean Architecture + Domain-Driven Design (DDD)
- **Database**: SQL Server with Entity Framework Core 8
- **Authentication**: OpenIddict (OAuth 2.0 / OpenID Connect)
- **Email**: MailKit + ABP Emailing module
- **Blob Storage**: ABP Blob Storage (FileSystem provider)
- **API**: RESTful APIs with Swagger/OpenAPI documentation
- **Multi-tenancy**: Full tenant isolation built-in
- **Testing**: xUnit for unit and integration tests

## Project Conventions

### Code Style
- **C# Conventions**: Follow Microsoft C# coding conventions
- **Naming**:
  - PascalCase for classes, methods, properties
  - camelCase for private fields, parameters
  - Entities suffix aggregates with descriptive names (e.g., `IssueReport`, `InfrastructureProject`)
  - AppServices named as `[Feature]AppService` (e.g., `CitizenAppService`)
  - DTOs named as `[Feature]Dto` or `CreateUpdate[Feature]Dto`
- **File Organization**: One class per file, file name matches class name
- **Async/Await**: Use async methods for I/O operations, suffix with `Async`

### Architecture Patterns
- **Clean Architecture Layers**:
  - `Domain.Shared`: Enums, constants, shared types
  - `Domain`: Entities, domain logic, repository interfaces
  - `Application.Contracts`: DTOs, service interfaces, permissions
  - `Application`: Application services, business logic
  - `EntityFrameworkCore`: EF Core DbContext, repositories, migrations
  - `HttpApi`: API controllers
  - `HttpApi.Host`: Entry point, configuration
- **DDD Patterns**:
  - Aggregate roots extend `FullAuditedAggregateRoot<Guid>`
  - Entities extend `FullAuditedEntity<Guid>` or `CreationAuditedEntity<Guid>`
  - Repository pattern for data access
  - Domain events for cross-aggregate communication
- **Service Pattern**: Application services implement CRUD via `CrudAppService` or custom `ApplicationService`
- **Auto-generated IDs**: Use `GuidGenerator.Create()` for entity IDs
- **Reference Numbers**: Auto-generate human-readable references (e.g., `ISS-20241111-ABCD1234`)

### Testing Strategy
- **Unit Tests**: Test domain logic and application services
- **Integration Tests**: Test with in-memory database
- **Test Projects**:
  - `CitizensPortal.Domain.Tests`
  - `CitizensPortal.Application.Tests`
- **Naming**: Test methods follow `MethodName_Scenario_ExpectedResult` pattern
- **Coverage**: Aim for >80% coverage on business logic

### Git Workflow
- **Branching**: Feature branches prefixed with `claude/` followed by descriptive name and session ID
- **Commits**: Clear, descriptive messages using imperative mood
- **Format**: Multi-line commits with summary line + detailed body using heredoc
- **Push**: Always push to remote after completing work
- **Main Branch**: Merge via pull requests after review

## Domain Context

### Municipal Services Domain
The platform handles core municipal/city council operations:
- **Citizen Services**: Registration, profiles, verification
- **Service Delivery**: Applications, permits, licenses, certificates
- **Issue Management**: Report infrastructure problems (potholes, lights, water leaks)
- **Financial**: Billing (rates, utilities), payments, receipts
- **Communication**: Notifications, emergency alerts
- **Engagement**: Surveys, consultations, voting, forums
- **Transparency**: Public infrastructure project tracking with GIS

### Key Domain Concepts
- **Citizen/Ratepayer**: Registered users who pay rates and use services
- **Multi-tenancy**: Each municipality is a separate tenant with isolated data
- **Issue Categories**: Road maintenance, street lighting, water/sanitation, waste, public safety, parks, noise, illegal dumping, traffic signals
- **Application Types**: Housing, business licenses, building permits, zoning permits, event permits, health certificates, liquor licenses
- **Bill Types**: Property rates, water, electricity, waste collection
- **Emergency Alert Types**: Natural disasters, fire, health emergencies, security threats, utility outages
- **Project Categories**: Roads, water infrastructure, electricity, public buildings, parks, drainage

### User Roles (Permissions)
- **Citizens**: Submit issues, apply for services, pay bills, participate in engagement
- **Staff**: Review applications, respond to issues, manage tickets
- **Managers**: Assign work, approve applications, manage projects
- **Administrators**: System configuration, tenant management, user administration

## Important Constraints

### Technical Constraints
- **.NET 8.0 LTS**: Long-term support requirement
- **SQL Server**: Database must be SQL Server (Azure SQL compatible)
- **Multi-tenancy**: All entities must implement `IMultiTenant` interface
- **Audit Logging**: All business entities must track creation, modification, deletion
- **Authorization**: All operations must check permissions via ABP authorization
- **Blob Storage**: Documents stored in blob storage, not database

### Business Constraints
- **Data Isolation**: Tenant data must be completely isolated (security requirement)
- **Audit Trail**: Government regulations require full audit history
- **Document Retention**: Documents must be retained per regulatory requirements
- **Authentication**: Must support OAuth 2.0 / OpenID Connect
- **Accessibility**: APIs must support future frontend accessibility requirements
- **Localization**: Support for multiple languages (English, other local languages)

### Regulatory Constraints
- **POPIA Compliance**: South African Protection of Personal Information Act
- **MFMA**: Municipal Finance Management Act (financial transparency)
- **Data Privacy**: Personal data handling per privacy regulations
- **Public Records**: Certain data must be publicly accessible (projects, consultations)
- **Security**: Government-grade security for citizen data

## External Dependencies

### ABP Framework Modules
- **Volo.Abp.Account**: Account management, registration, login
- **Volo.Abp.Identity**: User and role management
- **Volo.Abp.OpenIddict**: OAuth 2.0 / OIDC authentication
- **Volo.Abp.PermissionManagement**: Permission system
- **Volo.Abp.TenantManagement**: Multi-tenant management
- **Volo.Abp.BlobStoring**: Document/file storage abstraction
- **Volo.Abp.Emailing**: Email sending infrastructure
- **Volo.Abp.BackgroundJobs**: Async job processing

### Third-Party Libraries
- **MailKit**: SMTP email sending
- **Entity Framework Core**: ORM for database access
- **Newtonsoft.Json**: JSON serialization (for flexible data fields)
- **AutoMapper**: DTO mapping
- **Swashbuckle**: Swagger/OpenAPI documentation

### External Services (Future)
- **Payment Gateway**: Integration needed for online payments
- **SMS Gateway**: SMS notifications for emergency alerts
- **GIS Mapping Service**: Integration for infrastructure project mapping
- **Email Service**: SMTP server or email service (SendGrid, AWS SES)

## Current Status

### Completed (Backend)
- ✅ 26 domain entities with full relationships
- ✅ 18 application services with business logic
- ✅ 8 HTTP API controllers
- ✅ Multi-tenancy architecture
- ✅ Authentication & authorization (160+ permissions)
- ✅ Blob storage for documents
- ✅ Email notification system
- ✅ Audit logging for all entities

### Missing
- ❌ Frontend UI (no Angular/React/Blazor yet)
- ❌ 10 services lack HTTP controllers (Bills, Notifications, Surveys, Forums, Votes, Consultations, Certificates, Tickets, Infrastructure)
- ❌ Payment gateway integration
- ❌ SMS notification integration
- ❌ GIS mapping integration
- ❌ Production deployment configuration

## Notes for AI Assistants

- **Always use async/await** for database operations
- **Check permissions** before executing operations using `[Authorize]` attributes
- **Validate tenant context** for multi-tenant operations
- **Use AutoMapper** for DTO conversions (configured in `ApplicationAutoMapperProfile`)
- **Follow ABP conventions** for service naming, DTOs, and repository usage
- **Reference numbers** should be human-readable and include dates
- **GeoJSON format** for location data (projects, service areas)
- **HTTP 403** on git push failures indicates branch naming issue (must start with 'claude/' and end with session ID)
