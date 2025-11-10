# Citizens Portal - Multi-Tenant Local Authority Platform

## Project Overview

Citizens Portal is a comprehensive multi-tenant web application built with ABP Framework (ASP.NET Boilerplate) for local authorities to manage their interactions with citizens/ratepayers. Each tenant represents a different local authority (municipality, county, city council, etc.), and their citizens can access a personalized portal to interact with government services.

## Technology Stack

- **Framework**: ABP Framework 8.3.0 (ASP.NET Core 8.0)
- **Database**: SQL Server with Entity Framework Core
- **Architecture**: Domain-Driven Design (DDD) with Clean Architecture
- **Multi-Tenancy**: Built-in ABP multi-tenancy support
- **API**: RESTful Web API with Swagger/OpenAPI documentation
- **Authentication**: ABP Identity with OpenIddict (OAuth 2.0/OpenID Connect)

## Project Structure

```
CitizensPortal/
├── src/
│   ├── CitizensPortal.Domain.Shared/      # Shared constants, enums
│   ├── CitizensPortal.Domain/             # Domain entities, repositories
│   ├── CitizensPortal.Application.Contracts/  # DTOs, service interfaces
│   ├── CitizensPortal.Application/        # Application services, business logic
│   ├── CitizensPortal.EntityFrameworkCore/    # EF Core DbContext, migrations
│   ├── CitizensPortal.HttpApi/            # Web API controllers
│   └── CitizensPortal.HttpApi.Host/       # Main application entry point
├── test/
│   ├── CitizensPortal.Application.Tests/
│   └── CitizensPortal.Domain.Tests/
├── CitizensPortal.sln
└── claude.md
```

## Core Features Implemented

### 1. **Citizen Management**
- Citizen registration and profile management
- National ID and email verification
- Property reference linkage for ratepayers
- Multi-tenant isolation (each authority manages its own citizens)

**Entities**: `Citizen`

### 2. **Issue Reporting System**
- Report municipal issues (road maintenance, street lighting, water/sanitation, etc.)
- Issue categorization and priority management
- Location tracking (address, GPS coordinates)
- Photo/document attachments
- Comment threads for updates
- Status tracking (New → Under Review → Assigned → In Progress → Resolved)
- Unique reference numbers for tracking

**Entities**: `IssueReport`, `IssueAttachment`, `IssueComment`

**Issue Categories**:
- Road Maintenance
- Street Lighting
- Water and Sanitation
- Waste Management
- Public Safety
- Parks
- Noise Complaints
- Illegal Dumping
- Traffic Signals
- Other

### 3. **Applications Management**
Citizens can submit and track various applications:
- **Housing Applications**
- **Business Licenses**
- **Building Permits**
- **Zoning Permits**
- **Event Permits**
- **Parking Permits**
- **Health Certificates**
- **Fire Safety Certificates**
- **Liquor Licenses**
- **Trade Licenses**

**Features**:
- Draft → Submit → Review workflow
- Document uploads (ID, proof of residence, certificates)
- Status history tracking
- Review notes from officials
- Automatic application number generation

**Entities**: `Application`, `ApplicationDocument`, `ApplicationStatusHistory`

### 4. **Billing & Statements**
- Property rates billing
- Utility bills (water, electricity, waste, sewerage)
- Bill history and statements
- Payment tracking (unpaid, partial, paid, overdue)
- Balance calculations
- Period-based billing (monthly, quarterly, annual)

**Entities**: `Bill`

**Bill Types**:
- Property Rates
- Water
- Electricity
- Waste Collection
- Sewerage
- Other Charges

### 5. **Notifications & Alerts**
- Intelligent notification system
- Push notifications for important events
- Read/unread tracking
- Priority levels (Low, Normal, High, Urgent)
- Linked to related entities (bills, applications, issues)

**Notification Types**:
- Bill Generated/Due/Overdue
- Payment Received
- Application Status Changed
- Application Approved/Rejected
- Issue Status Changed/Resolved
- Ticket Responses
- General Announcements
- System Alerts

**Entities**: `Notification`

### 6. **Support Ticket System**
- Create support tickets for inquiries
- Message threads between citizens and staff
- File attachments
- Status tracking (Open → In Progress → Resolved → Closed)
- Priority management

**Ticket Categories**:
- Technical Support
- Billing Inquiry
- Application Support
- Account Issues
- General Inquiry
- Complaints
- Feature Requests

**Entities**: `Ticket`, `TicketMessage`, `TicketAttachment`

## Phase 2 - Enhanced Services (IMPLEMENTED)

### 7. **Online Payments Integration** ✅
- Payment processing with multiple gateways (Stripe, PayPal, Square, etc.)
- Payment allocation to multiple bills
- Payment history tracking
- Receipt generation
- Refund management
- Transaction status tracking

**Entities**: `Payment`, `PaymentAllocation`, `PaymentReceipt`

**Payment Methods**: Credit Card, Debit Card, Bank Transfer, EFT, Cash, Cheque, Mobile Payment, Online Payment

### 8. **Document Management** ✅
- Digital document vault for citizens
- Document categorization (Personal, Property, Tax, Certificates, etc.)
- Document status tracking (Active, Expired, Revoked, Archived)
- Digital signature support
- Document expiry and renewal reminders
- Access logging and audit trail
- File integrity verification (hash)
- Document access levels (Private, Shared, Public)

**Entities**: `Document`, `DocumentAccess`

### 9. **Property Information** ✅
- Complete property registry
- Property details (type, size, location, GPS coordinates)
- Property valuations and history
- Multiple valuation methods (Market Value, Replacement Cost, etc.)
- Property ownership tracking (Owner, Tenant, Co-Owner, Leaseholder)
- Property tax history
- Ward and suburb information

**Entities**: `Property`, `PropertyOwnership`, `PropertyValuation`, `PropertyTaxHistory`

### 10. **Appointment Booking** ✅
- Book appointments with municipal departments
- Appointment types (In-Person, Virtual, Phone)
- Time slot management
- Staff assignment
- Appointment reminders
- Virtual meeting link generation
- Cancellation and rescheduling
- No-show tracking

**Entities**: `Appointment`, `AppointmentSlot`, `Department`

### 11. **Public Records & Certificates** ✅
- Request official certificates (Birth, Death, Marriage, etc.)
- Multiple certificate types supported
- Document upload for verification
- Payment integration
- Collection/delivery options
- Processing workflow
- Digital certificate generation
- Verification codes for authenticity

**Entities**: `CertificateRequest`, `CertificateRequestDocument`, `Certificate`

**Certificate Types**: Birth, Death, Marriage, Divorce, Property, Tax Clearance, Residence, Good Conduct, and more

## Phase 3 - Advanced Features (IMPLEMENTED)

### 12. **GIS & Mapping Integration** ✅
- Infrastructure project tracking with GeoJSON support
- Project visualization on maps
- Service area coverage mapping
- Project progress tracking
- Budget vs spending monitoring
- Contractor management
- Project updates and photo documentation
- Boundary polygon support for area projects

**Entities**: `InfrastructureProject`, `ProjectUpdate`, `ProjectImage`, `ServiceArea`

**Project Categories**: Roads, Water, Electricity, Parks, Buildings, Bridges, Drainage, Street Lighting

### 13. **Community Engagement** ✅
- **Surveys & Polls**
  - Create public surveys
  - Multiple question types (Single/Multiple Choice, Text, Rating, Yes/No)
  - Anonymous responses option
  - Real-time response tracking

- **Public Consultations**
  - Policy and project consultations
  - Document attachments
  - Public commenting
  - View tracking

- **Community Voting**
  - Vote on initiatives and budgets
  - Multiple vote types (Initiative, Budget, Project Priority, Policy Change)
  - Vote verification option
  - Real-time vote counting

- **Community Forums**
  - Topic-based discussions
  - Threaded replies
  - Like/upvote system
  - Pinned and locked posts
  - Moderation (Active, Flagged, Hidden)

**Entities**: `Survey`, `SurveyQuestion`, `SurveyResponse`, `SurveyAnswer`, `Consultation`, `ConsultationComment`, `Vote`, `VoteOption`, `CitizenVote`, `ForumTopic`, `ForumPost`, `ForumReply`

### 14. **Emergency Services** ✅
- Emergency alert broadcasting
- Alert severity levels (Info, Warning, Severe, Critical)
- Alert types (Weather, Fire, Flood, Earthquake, Power/Water Outage, etc.)
- Affected area mapping (GeoJSON polygons)
- Ward-based targeting
- Safety instructions
- Citizen acknowledgement tracking
- Evacuation route mapping
- Emergency contact directory

**Entities**: `EmergencyAlert`, `AlertAcknowledgement`, `EvacuationRoute`, `EmergencyContact`

### 15. **Service Requests** ✅
- Request municipal services
- Multiple service types (Bulk Waste Pickup, Tree Trimming, Street Maintenance, Inspections, etc.)
- Priority management
- Preferred scheduling
- GPS location tracking
- Photo attachments
- Staff and department assignment
- Service completion tracking
- Citizen feedback and ratings
- Status updates

**Entities**: `ServiceRequest`, `ServiceRequestAttachment`, `ServiceRequestUpdate`

**Service Types**: Bulk Waste Pickup, Garden Waste, Street Maintenance, Tree Trimming, Pothole Repair, Street Light Repair, Drainage Cleaning, Pest Control, Building/Health/Fire Safety Inspections

### 16. **Dashboard & Analytics** ✅
- Personalized citizen dashboard
- Real-time statistics aggregation
- Outstanding balance tracking
- Application status overview
- Issue and service request tracking
- Notification and ticket counts
- Upcoming appointments
- Monthly spending analysis by category
- Service usage logging for analytics

**Entities**: `CitizenDashboardStats`, `ServiceUsageLog`, `MonthlySpendingAnalysis`

## Phase 4 - Future Enhancements (Suggested)

### Mobile Applications
- Native iOS and Android apps
- Push notifications
- Offline mode
- Biometric authentication

### Third-Party Integrations
- Social media integration
- SMS notifications
- Email marketing platforms
- CRM systems integration

### Reporting & Transparency
- Public spending reports
- Project status tracking
- Budget visualization
- Performance metrics dashboard

### Multilingual Support
- Multiple language support
- RTL language support
- Localized content
- Translation management

### Accessibility Features
- WCAG 2.1 compliance
- Screen reader support
- High contrast modes
- Keyboard navigation

## Database Schema

### Phase 1 - Core Tables
- `AppCitizens` - Citizen profiles
- `AppIssueReports` - Issue reports
- `AppIssueAttachments` - Issue photos/documents
- `AppIssueComments` - Issue comments
- `AppApplications` - Application submissions
- `AppApplicationDocuments` - Application documents
- `AppApplicationStatusHistories` - Application audit trail
- `AppBills` - Bills and statements
- `AppNotifications` - Notification inbox
- `AppTickets` - Support tickets
- `AppTicketMessages` - Ticket conversations
- `AppTicketAttachments` - Ticket attachments

### Phase 2 - Enhanced Services Tables
- `AppPayments` - Payment transactions
- `AppPaymentAllocations` - Payment-to-bill mappings
- `AppPaymentReceipts` - Payment receipts
- `AppDocuments` - Digital document vault
- `AppDocumentAccesses` - Document access audit log
- `AppProperties` - Property registry
- `AppPropertyOwnerships` - Property-citizen relationships
- `AppPropertyValuations` - Property valuation history
- `AppPropertyTaxHistories` - Property tax payment history
- `AppDepartments` - Municipal departments
- `AppAppointments` - Citizen appointments
- `AppAppointmentSlots` - Available appointment time slots
- `AppCertificateRequests` - Certificate requests
- `AppCertificateRequestDocuments` - Supporting documents for requests
- `AppCertificates` - Issued certificates

### Phase 3 - Advanced Features Tables
- `AppInfrastructureProjects` - Municipal infrastructure projects
- `AppProjectUpdates` - Project progress updates
- `AppProjectImages` - Project photos
- `AppServiceAreas` - Service coverage areas (GeoJSON)
- `AppSurveys` - Public surveys
- `AppSurveyQuestions` - Survey questions
- `AppSurveyResponses` - Citizen survey responses
- `AppSurveyAnswers` - Individual question answers
- `AppConsultations` - Public consultations
- `AppConsultationComments` - Consultation feedback
- `AppVotes` - Community votes
- `AppVoteOptions` - Vote choices
- `AppCitizenVotes` - Citizen voting records
- `AppForumTopics` - Forum categories
- `AppForumPosts` - Forum posts
- `AppForumReplies` - Forum post replies
- `AppEmergencyAlerts` - Emergency alerts
- `AppAlertAcknowledgements` - Alert citizen acknowledgements
- `AppEvacuationRoutes` - Evacuation route mappings
- `AppEmergencyContacts` - Emergency contact directory
- `AppServiceRequests` - Service requests
- `AppServiceRequestAttachments` - Service request attachments
- `AppServiceRequestUpdates` - Service request status updates
- `AppCitizenDashboardStats` - Aggregated dashboard statistics
- `AppServiceUsageLogs` - Service usage analytics
- `AppMonthlySpendingAnalyses` - Monthly spending breakdowns

**Total: 60+ database tables** covering all aspects of citizen service delivery

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server 2019 or later (or SQL Server Express)
- Visual Studio 2022 or JetBrains Rider (or VS Code)
- Node.js (for frontend if building separate SPA)

### Installation Steps

1. **Install .NET SDK**
   ```bash
   # Download from https://dotnet.microsoft.com/download
   dotnet --version  # Verify installation
   ```

2. **Install ABP CLI**
   ```bash
   dotnet tool install -g Volo.Abp.Cli
   abp --version
   ```

3. **Update Connection String**

   Edit `src/CitizensPortal.HttpApi.Host/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "Default": "Server=localhost;Database=CitizensPortal;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

4. **Create Database**
   ```bash
   cd src/CitizensPortal.EntityFrameworkCore
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   cd src/CitizensPortal.HttpApi.Host
   dotnet run
   ```

6. **Access the Application**
   - API: https://localhost:44300
   - Swagger UI: https://localhost:44300/swagger

### Database Migrations

```bash
# Add a new migration
cd src/CitizensPortal.EntityFrameworkCore
dotnet ef migrations add YourMigrationName

# Update database
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Remove last migration (if not applied)
dotnet ef migrations remove
```

## API Endpoints

### Citizens
- `GET /api/citizens` - List all citizens
- `GET /api/citizens/{id}` - Get citizen by ID
- `GET /api/citizens/by-email/{email}` - Get citizen by email
- `GET /api/citizens/me` - Get current citizen
- `POST /api/citizens` - Register new citizen
- `PUT /api/citizens/{id}` - Update citizen
- `DELETE /api/citizens/{id}` - Delete citizen

### Issue Reports
- `GET /api/issue-reports` - List all issues
- `GET /api/issue-reports/{id}` - Get issue by ID
- `GET /api/issue-reports/my-issues` - Get current citizen's issues
- `GET /api/issue-reports/by-reference/{ref}` - Get by reference number
- `POST /api/issue-reports` - Create new issue report
- `POST /api/issue-reports/{id}/comments` - Add comment to issue
- `PUT /api/issue-reports/{id}` - Update issue
- `DELETE /api/issue-reports/{id}` - Delete issue

### Applications
- `GET /api/applications` - List all applications
- `GET /api/applications/{id}` - Get application by ID
- `GET /api/applications/my-applications` - Get current citizen's applications
- `GET /api/applications/by-number/{number}` - Get by application number
- `POST /api/applications` - Create new application
- `POST /api/applications/{id}/submit` - Submit application
- `POST /api/applications/{id}/withdraw` - Withdraw application
- `PUT /api/applications/{id}` - Update application

### Bills
- `GET /api/bills` - List all bills
- `GET /api/bills/{id}` - Get bill by ID
- `GET /api/bills/my-bills` - Get current citizen's bills
- `GET /api/bills/unpaid` - Get unpaid bills
- `GET /api/bills/total-balance` - Get total balance

### Notifications
- `GET /api/notifications/my-notifications` - Get my notifications
- `GET /api/notifications/unread` - Get unread notifications
- `GET /api/notifications/unread-count` - Get unread count
- `POST /api/notifications/{id}/mark-read` - Mark as read
- `POST /api/notifications/mark-all-read` - Mark all as read

### Tickets
- `GET /api/tickets` - List all tickets
- `GET /api/tickets/{id}` - Get ticket by ID
- `GET /api/tickets/my-tickets` - Get my tickets
- `POST /api/tickets` - Create new ticket
- `POST /api/tickets/{id}/messages` - Add message
- `POST /api/tickets/{id}/close` - Close ticket

## Multi-Tenancy

### Tenant Isolation
- Each local authority is a separate tenant
- Complete data isolation between tenants
- Tenant-specific configurations
- Shared infrastructure with isolated data

### Tenant Management
- Create tenants via ABP UI or API
- Configure connection strings per tenant
- Enable/disable tenant features
- Tenant branding customization

### Accessing as Tenant
Add tenant identifier in HTTP header:
```
__tenant: {tenant-id-or-name}
```

## Security

### Authentication
- OAuth 2.0 / OpenID Connect via OpenIddict
- JWT token-based authentication
- Refresh token support
- Social login support (Google, Facebook, etc.)

### Authorization
- Role-based access control (RBAC)
- Permission-based authorization
- Multi-tenant permission isolation
- Built-in ABP authorization policies

### Data Protection
- Multi-tenant data isolation
- Soft delete for audit trail
- Audit logging
- GDPR compliance ready

## Development Guidelines

### Adding a New Entity

1. **Create Entity** in `CitizensPortal.Domain/Entities/`
   ```csharp
   public class MyEntity : FullAuditedAggregateRoot<Guid>, IMultiTenant
   {
       public Guid? TenantId { get; set; }
       // Properties...
   }
   ```

2. **Add to DbContext** in `CitizensPortal.EntityFrameworkCore/CitizensPortalDbContext.cs`
   ```csharp
   public DbSet<MyEntity> MyEntities { get; set; }
   ```

3. **Configure in ModelBuilder** in `CitizensPortalDbContextModelCreatingExtensions.cs`

4. **Create DTOs** in `CitizensPortal.Application.Contracts/DTOs/`

5. **Create Service Interface** in `CitizensPortal.Application.Contracts/Services/`

6. **Implement Service** in `CitizensPortal.Application/Services/`

7. **Create Controller** in `CitizensPortal.HttpApi/Controllers/`

8. **Add AutoMapper mappings** in `CitizensPortalApplicationAutoMapperProfile.cs`

9. **Create migration** and update database

### Code Standards
- Follow C# coding conventions
- Use async/await for all I/O operations
- Implement proper exception handling
- Add XML documentation comments
- Write unit tests for business logic
- Follow SOLID principles
- Use dependency injection

## Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test test/CitizensPortal.Application.Tests/

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## Deployment

### Production Checklist
- [ ] Update connection strings
- [ ] Configure SMTP settings for email
- [ ] Set up SSL certificates
- [ ] Configure CORS origins
- [ ] Enable authentication
- [ ] Set up file storage (Azure Blob, AWS S3, etc.)
- [ ] Configure logging and monitoring
- [ ] Set up backup strategy
- [ ] Configure CDN for static assets
- [ ] Enable rate limiting
- [ ] Set up health checks
- [ ] Configure application insights

### Docker Deployment
```dockerfile
# Create Dockerfile in src/CitizensPortal.HttpApi.Host/
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "CitizensPortal.sln"
RUN dotnet build "CitizensPortal.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/CitizensPortal.HttpApi.Host/CitizensPortal.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CitizensPortal.HttpApi.Host.dll"]
```

```bash
# Build and run
docker build -t citizensportal .
docker run -d -p 8080:80 citizensportal
```

## Monitoring & Logging

### Serilog Configuration
Logs are written to:
- Console (development)
- File: `Logs/logs.txt`
- Can be extended to Application Insights, Elasticsearch, etc.

### Health Checks
Add health check endpoints for:
- Database connectivity
- External API availability
- Disk space
- Memory usage

## Performance Optimization

### Best Practices
- Use async/await consistently
- Implement caching (Redis, in-memory)
- Enable response compression
- Use pagination for large datasets
- Implement lazy loading carefully
- Optimize database queries
- Use indexes on frequently queried columns
- Implement background jobs for heavy tasks

### Caching Strategy
```csharp
// Example: Cache citizen data
[Cache(300)] // Cache for 5 minutes
public async Task<CitizenDto> GetAsync(Guid id)
{
    // Implementation
}
```

## Support & Resources

### ABP Framework Resources
- **Documentation**: https://docs.abp.io
- **Community**: https://community.abp.io
- **GitHub**: https://github.com/abpframework/abp
- **Premium Support**: https://commercial.abp.io

### Project-Specific Support
- Review this `claude.md` file
- Check Swagger documentation at `/swagger`
- Review inline code comments
- Examine unit tests for usage examples

## License

This project is created for [Your Local Authority Name]. All rights reserved.

## Contributing

1. Create a feature branch
2. Make your changes
3. Write/update tests
4. Update documentation
5. Submit a pull request

## Version History

### Version 1.0.0 (Initial Release)
- Core citizen management
- Issue reporting system
- Application management
- Billing and statements
- Notification system
- Support ticket system
- Multi-tenant architecture
- RESTful API with Swagger
- Entity Framework Core integration

---

**Last Updated**: 2025-11-10
**ABP Framework Version**: 8.3.0
**Target Framework**: .NET 8.0
