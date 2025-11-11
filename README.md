# Citizens Portal

A comprehensive multi-tenant web application for local authorities to manage citizen services, built with ABP Framework.

## Features

- **Multi-Tenant Architecture** - Separate data for each local authority
- **Issue Reporting** - Citizens report municipal issues with photos and location
- **Application Management** - Housing, licensing, permits, and more
- **Billing System** - Property rates, utilities, and payment tracking
- **Notifications** - Real-time alerts and inbox messages
- **Support Tickets** - Help desk system for citizen inquiries
- **Payment Processing** - Multi-gateway payment integration
- **Document Vault** - Digital document management with signatures
- **Property Registry** - Complete property information system
- **Appointment Booking** - Schedule meetings with departments
- **Certificate Requests** - Digital certificate issuance
- **Infrastructure Projects** - Track municipal projects with GIS
- **Community Engagement** - Surveys, consultations, voting, forums
- **Emergency Services** - Alert broadcasting and evacuation routes
- **Service Requests** - Request municipal services online

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server 2019+
- ABP CLI

### Installation

1. **Setup Development Environment**
   ```bash
   # Linux/Mac
   ./setup-dev.sh

   # Windows
   setup-dev.bat
   ```

2. **Configure Database**

   Edit `src/CitizensPortal.HttpApi.Host/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "Default": "Server=localhost;Database=CitizensPortal;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

3. **Create Database & Seed Data**
   ```bash
   # Create initial migration
   cd src/CitizensPortal.EntityFrameworkCore
   dotnet ef migrations add InitialCreate

   # Run database migrator (creates DB and seeds admin user)
   cd ../..
   dotnet run --project src/CitizensPortal.DbMigrator/
   ```

4. **Run Application**
   ```bash
   cd src/CitizensPortal.HttpApi.Host
   dotnet run
   ```

5. **Access Swagger UI**

   Navigate to: https://localhost:44300/swagger

   **Default Login:**
   - Email: admin@abp.io
   - Password: 1q2w3E*

   **📖 See [AUTHENTICATION.md](AUTHENTICATION.md) for complete authentication & authorization guide**

## Documentation

- **[Quick Start Guide](QUICKSTART.md)** - Get up and running quickly
- **[ABP CLI Guide](ABP-CLI-GUIDE.md)** - Comprehensive ABP CLI reference
- **[Full Documentation](claude.md)** - Complete feature documentation

## Technology Stack

- ABP Framework 8.3.0
- .NET 8.0
- Entity Framework Core
- SQL Server
- OpenIddict (OAuth/OpenID Connect)

## Project Structure

```
├── src/
│   ├── CitizensPortal.Domain.Shared      # Shared constants, enums
│   ├── CitizensPortal.Domain             # Domain entities, repositories
│   ├── CitizensPortal.Application.Contracts  # DTOs, service interfaces
│   ├── CitizensPortal.Application        # Application services
│   ├── CitizensPortal.EntityFrameworkCore    # EF Core, migrations
│   ├── CitizensPortal.HttpApi            # API controllers
│   └── CitizensPortal.HttpApi.Host       # Main entry point
├── test/
│   ├── CitizensPortal.Application.Tests
│   └── CitizensPortal.Domain.Tests
├── setup-dev.sh                          # Linux/Mac setup script
├── setup-dev.bat                         # Windows setup script
├── QUICKSTART.md                         # Quick start guide
├── ABP-CLI-GUIDE.md                      # ABP CLI reference
└── claude.md                             # Complete documentation
```

## Key Features

### Phase 1 - Core Features
✅ Citizen Management
✅ Issue Reporting
✅ Application Management
✅ Billing & Statements
✅ Notifications
✅ Support Tickets

### Phase 2 - Enhanced Services
✅ Payment Processing
✅ Document Management
✅ Property Registry
✅ Appointment Booking
✅ Certificate Requests

### Phase 3 - Advanced Features
✅ Infrastructure Projects
✅ Community Engagement
✅ Emergency Services
✅ Service Requests
✅ Dashboard & Analytics

## API Endpoints

All endpoints are documented in Swagger UI at `/swagger`

Major endpoint groups:
- `/api/citizens` - Citizen management
- `/api/issue-reports` - Issue reporting
- `/api/applications` - Applications
- `/api/bills` - Billing
- `/api/payments` - Payment processing
- `/api/documents` - Document vault
- `/api/properties` - Property registry
- `/api/appointments` - Appointments
- `/api/service-requests` - Service requests
- `/api/emergency-alerts` - Emergency alerts

See `ABP-CLI-GUIDE.md` for generating client proxies.

## ABP Infrastructure

This project includes comprehensive ABP Framework infrastructure:

### Permissions System
- **160+ fine-grained permissions** organized by feature
- Hierarchical permission structure (parent/child relationships)
- Role-based and policy-based authorization
- Located in `src/CitizensPortal.Application.Contracts/Permissions/`

Key permission groups:
- Citizens, IssueReports, Applications, Bills
- Payments, Documents, Properties, Appointments
- Certificates, InfrastructureProjects
- Surveys, Consultations, Votes, Forums
- EmergencyAlerts, ServiceRequests

### Localization
- English localization with 160+ resource strings
- Extensible for multiple languages
- Located in `src/CitizensPortal.Domain.Shared/Localization/CitizensPortal/`
- Add new languages by creating `{culture}.json` files

### Application Services
- Example services with proper authorization attributes
- `CitizenAppService` - Citizen management with permissions
- `IssueReportAppService` - Issue tracking with role-based access
- All services inherit from ABP's `CrudAppService` for standard operations

### Database Migrator
- Standalone console application for running migrations
- Located in `src/CitizensPortal.DbMigrator/`
- Run with: `dotnet run --project src/CitizensPortal.DbMigrator/`
- Automatically applies pending migrations and seeds data

### Multi-Tenancy
- Built-in support for tenant isolation
- All entities implement `IMultiTenant` where appropriate
- Tenant resolver configured for subdomain/header-based resolution

## Development

### Add a Migration
```bash
cd src/CitizensPortal.EntityFrameworkCore
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

### Run with Hot Reload
```bash
cd src/CitizensPortal.HttpApi.Host
dotnet watch run
```

### Run Tests
```bash
dotnet test
```

### Update ABP Packages
```bash
abp update
```

## Contributing

1. Create a feature branch
2. Make your changes
3. Add migrations if needed
4. Update documentation
5. Submit a pull request

## License

All rights reserved.

## Support

- **ABP Documentation**: https://docs.abp.io
- **ABP Community**: https://community.abp.io
- **Project Docs**: See claude.md
