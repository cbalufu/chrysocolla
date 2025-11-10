# ABP CLI Quick Reference

## Installation

### Global Installation
```bash
dotnet tool install -g Volo.Abp.Cli
```

### Update Existing Installation
```bash
dotnet tool update -g Volo.Abp.Cli
```

### Local Installation (Project-specific)
```bash
dotnet tool restore
```

## Common Commands

### Check Version
```bash
abp --version
```

### Get Help
```bash
abp --help
abp [command] --help
```

## Project Commands

### Update ABP Packages
```bash
# Update all ABP packages to latest stable version
abp update

# Update to specific version
abp update --version 8.3.0

# Check for available updates
abp update --check-all
```

### Clean Solution
```bash
# Clean bin and obj folders
abp clean
```

### Generate Client Proxies
```bash
# Generate JavaScript proxies for API
abp generate-proxy -t js -u https://localhost:44300

# Generate C# proxies
abp generate-proxy -t csharp
```

### Bundle Assets
```bash
# Bundle and minify CSS/JS files
abp bundle
```

## Module Management

### Add Module
```bash
# Add a module to the solution
abp add-module Volo.Blogging

# Add module with specific version
abp add-module Volo.Blogging --version 8.3.0
```

### Add Package
```bash
# Add ABP package to specific project
abp add-package Volo.Abp.Account.Application --project src/CitizensPortal.Application

# Add with specific version
abp add-package Volo.Abp.Account.Application --version 8.3.0 --project src/CitizensPortal.Application
```

## Database Commands

### Entity Framework Core
```bash
# Install EF Core tools globally (if not already installed)
dotnet tool install -g dotnet-ef

# Add migration
cd src/CitizensPortal.EntityFrameworkCore
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove

# List migrations
dotnet ef migrations list

# Generate SQL script
dotnet ef migrations script

# Drop database
dotnet ef database drop
```

## Build and Run

### Restore Dependencies
```bash
dotnet restore
```

### Build Solution
```bash
dotnet build
```

### Run Application
```bash
cd src/CitizensPortal.HttpApi.Host
dotnet run
```

### Run with Watch (Hot Reload)
```bash
dotnet watch run
```

## Useful ABP Commands for Citizens Portal

### Generate Proxy for Frontend
```bash
# For Angular frontend
abp generate-proxy -t ng

# For React/Vue
abp generate-proxy -t js -u https://localhost:44300
```

### Install Suite (Commercial License Required)
```bash
dotnet tool install -g Volo.Abp.Suite
abp suite
```

### Login to ABP
```bash
abp login <username>
```

### Logout
```bash
abp logout
```

## Troubleshooting

### Clear NuGet Cache
```bash
dotnet nuget locals all --clear
```

### Reinstall ABP CLI
```bash
dotnet tool uninstall -g Volo.Abp.Cli
dotnet tool install -g Volo.Abp.Cli
```

### Check ABP CLI Installation
```bash
dotnet tool list -g
```

## Environment-Specific Commands

### Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet run --environment Production
```

### Staging
```bash
dotnet run --environment Staging
```

## Testing

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Project
```bash
dotnet test test/CitizensPortal.Application.Tests
```

### Run with Coverage
```bash
dotnet test /p:CollectCoverage=true
```

## Publishing

### Publish for Production
```bash
cd src/CitizensPortal.HttpApi.Host
dotnet publish -c Release -o ./publish
```

### Publish Self-Contained
```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

## Quick Start Workflow

1. **Initial Setup**
   ```bash
   ./setup-dev.sh  # On Linux/Mac
   setup-dev.bat   # On Windows
   ```

2. **Update Connection String**
   Edit `src/CitizensPortal.HttpApi.Host/appsettings.json`

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

5. **Access Application**
   - API: https://localhost:44300
   - Swagger: https://localhost:44300/swagger

## Additional Resources

- **ABP Documentation**: https://docs.abp.io
- **ABP CLI Documentation**: https://docs.abp.io/en/abp/latest/CLI
- **ABP Community**: https://community.abp.io
- **GitHub**: https://github.com/abpframework/abp
