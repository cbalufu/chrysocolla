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

3. **Create Database**
   ```bash
   cd src/CitizensPortal.EntityFrameworkCore
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run Application**
   ```bash
   cd ../CitizensPortal.HttpApi.Host
   dotnet run
   ```

5. **Access**
   - API: https://localhost:44300
   - Swagger: https://localhost:44300/swagger

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
