# Citizens Portal

A comprehensive multi-tenant web application for local authorities to manage citizen services, built with ABP Framework.

## Features

- **Multi-Tenant Architecture** - Separate data for each local authority
- **Issue Reporting** - Citizens report municipal issues with photos and location
- **Application Management** - Housing, licensing, permits, and more
- **Billing System** - Property rates, utilities, and payment tracking
- **Notifications** - Real-time alerts and inbox messages
- **Support Tickets** - Help desk system for citizen inquiries

## Quick Start

```bash
# Install .NET 8.0 SDK
# Install ABP CLI
dotnet tool install -g Volo.Abp.Cli

# Update connection string in appsettings.json

# Create and migrate database
cd src/CitizensPortal.EntityFrameworkCore
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run the application
cd ../CitizensPortal.HttpApi.Host
dotnet run

# Access at https://localhost:44300
# Swagger: https://localhost:44300/swagger
```

## Documentation

See [claude.md](./claude.md) for comprehensive documentation including:
- Detailed feature descriptions
- API endpoints
- Development guidelines
- Deployment instructions
- Suggested additional features

## Technology Stack

- ABP Framework 8.3.0
- .NET 8.0
- Entity Framework Core
- SQL Server
- OpenIddict (OAuth/OpenID Connect)

## Project Structure

```
├── src/
│   ├── CitizensPortal.Domain.Shared
│   ├── CitizensPortal.Domain
│   ├── CitizensPortal.Application.Contracts
│   ├── CitizensPortal.Application
│   ├── CitizensPortal.EntityFrameworkCore
│   ├── CitizensPortal.HttpApi
│   └── CitizensPortal.HttpApi.Host
└── test/
    ├── CitizensPortal.Application.Tests
    └── CitizensPortal.Domain.Tests
```

## License

All rights reserved.
