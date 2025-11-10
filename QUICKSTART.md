# Citizens Portal - Quick Start Guide

## Prerequisites

- .NET 8.0 SDK or later
- SQL Server 2019+ (or SQL Server Express/LocalDB)
- Visual Studio 2022, JetBrains Rider, or VS Code
- Node.js 18+ (for frontend development)

## Step 1: Install ABP CLI

Run the appropriate setup script for your platform:

### Linux/Mac
```bash
./setup-dev.sh
```

### Windows
```cmd
setup-dev.bat
```

Or manually install ABP CLI:
```bash
dotnet tool install -g Volo.Abp.Cli
```

## Step 2: Restore Local Tools
```bash
dotnet tool restore
```

## Step 3: Configure Database

Edit `src/CitizensPortal.HttpApi.Host/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=CitizensPortal;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

For SQL Server with credentials:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=CitizensPortal;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}
```

## Step 4: Create Database

```bash
cd src/CitizensPortal.EntityFrameworkCore
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Step 5: Run the Application

```bash
cd ../CitizensPortal.HttpApi.Host
dotnet run
```

## Step 6: Access the Application

- **API Base URL**: https://localhost:44300
- **Swagger UI**: https://localhost:44300/swagger
- **Health Check**: https://localhost:44300/health

## Common ABP CLI Commands

### Check ABP CLI Version
```bash
abp --version
```

### Update ABP Packages
```bash
abp update
```

### Clean Solution
```bash
abp clean
```

### Generate API Proxies
```bash
abp generate-proxy -t js -u https://localhost:44300
```

## Troubleshooting

### Port Already in Use
Edit `src/CitizensPortal.HttpApi.Host/Properties/launchSettings.json` to change ports.

### Database Connection Issues
- Ensure SQL Server is running
- Verify connection string
- Check firewall settings

### EF Core Tools Not Found
```bash
dotnet tool install -g dotnet-ef
```

### ABP CLI Not Found
```bash
dotnet tool install -g Volo.Abp.Cli
```

## Development Workflow

1. **Make Changes** to domain entities, services, or controllers
2. **Add Migration** (if database changes):
   ```bash
   cd src/CitizensPortal.EntityFrameworkCore
   dotnet ef migrations add YourMigrationName
   ```
3. **Update Database**:
   ```bash
   dotnet ef database update
   ```
4. **Run Application**:
   ```bash
   cd ../CitizensPortal.HttpApi.Host
   dotnet run
   ```
5. **Test API** via Swagger UI

## Building for Production

```bash
cd src/CitizensPortal.HttpApi.Host
dotnet publish -c Release -o ./publish
```

## Docker Support

Create a Dockerfile in the root directory:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/CitizensPortal.HttpApi.Host/CitizensPortal.HttpApi.Host.csproj"
RUN dotnet build "src/CitizensPortal.HttpApi.Host/CitizensPortal.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/CitizensPortal.HttpApi.Host/CitizensPortal.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CitizensPortal.HttpApi.Host.dll"]
```

Build and run:
```bash
docker build -t citizensportal .
docker run -d -p 8080:80 citizensportal
```

## Next Steps

See `ABP-CLI-GUIDE.md` for comprehensive ABP CLI documentation.
See `claude.md` for complete feature documentation.

## Support

- **ABP Documentation**: https://docs.abp.io
- **ABP Community**: https://community.abp.io
- **Project Documentation**: See claude.md
