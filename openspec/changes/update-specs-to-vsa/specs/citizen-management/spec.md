## ADDED Requirements

### Requirement: JWT Authentication
The system SHALL provide JWT-based authentication for citizens to access protected endpoints.

#### Scenario: Successful login
- **WHEN** citizen provides valid email and password credentials
- **THEN** the system verifies password using BCrypt
- **AND** generates new access token (expires in 60 minutes)
- **AND** generates new refresh token (expires in 7 days)
- **AND** returns tokens with citizen details (id, name, email, role)

#### Scenario: Invalid credentials
- **WHEN** citizen provides incorrect password
- **THEN** the system returns error "Invalid credentials"
- **AND** does not reveal whether email exists

### Requirement: Token Refresh
The system SHALL allow citizens to refresh expired access tokens using valid refresh tokens.

#### Scenario: Refresh with valid token
- **WHEN** citizen provides valid refresh token
- **THEN** the system validates token signature and expiration
- **AND** retrieves associated citizen
- **AND** generates new access token
- **AND** generates new refresh token
- **AND** returns new token pair

#### Scenario: Refresh with invalid token
- **WHEN** citizen provides invalid or expired refresh token
- **THEN** the system returns error "Invalid refresh token"
- **AND** requires re-authentication

### Requirement: Role-Based Authorization
The system SHALL support role-based access control with roles: Citizen, Staff, Admin.

#### Scenario: Citizen role assignment
- **WHEN** new citizen registers
- **THEN** the system assigns default role "Citizen"
- **AND** includes role claim in JWT tokens

#### Scenario: Role-based endpoint access
- **WHEN** user with "Citizen" role accesses citizen-only endpoint
- **THEN** the system grants access
- **AND** when user with "Citizen" role accesses admin endpoint
- **THEN** the system returns 403 Forbidden

## MODIFIED Requirements

### Requirement: Citizen Registration
The system SHALL allow new citizens to register with their personal information and receive JWT authentication tokens.

#### Scenario: Successful registration
- **WHEN** a new citizen provides valid registration data (first name, last name, email, password, phone number)
- **THEN** the system creates a citizen record with hashed password using BCrypt
- **AND** assigns default role "Citizen"
- **AND** generates JWT access token and refresh token
- **AND** returns authentication response with tokens and citizen details

#### Scenario: Duplicate email registration
- **WHEN** a citizen attempts to register with an email that already exists
- **THEN** the system rejects the registration
- **AND** returns error "Email already in use"

## MODIFIED Data Model

### Citizen Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **FirstName**: string (Required, max 100 chars)
- **LastName**: string (Required, max 100 chars)
- **Email**: string (Required, max 256 chars, Unique within tenant)
- **PasswordHash**: string (Required, BCrypt hash)
- **PhoneNumber**: string (Required, max 20 chars)
- **Role**: string (Required, default "Citizen", max 50 chars)
- **Address**: string? (Optional, max 500 chars)
- **City**: string? (Optional, max 100 chars)
- **PostalCode**: string? (Optional, max 20 chars)
- **CreatedAt**: DateTime (UTC)
- **UpdatedAt**: DateTime? (UTC)

### RefreshToken Entity
- **Id**: Guid (Primary Key)
- **TenantId**: Guid? (Multi-tenancy)
- **CitizenId**: Guid (Foreign Key to Citizen)
- **Token**: string (Required, Unique, indexed)
- **ExpiresAt**: DateTime (UTC, 7 days from creation)
- **IsRevoked**: bool (Default false)
- **CreatedAt**: DateTime (UTC)

## MODIFIED Implementation Notes

**Architecture**: Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs

**Authentication Features** (located in `src/CitizensPortal.Api/Features/Auth/`):
- **Register**: Command, Validator, Handler, Carter endpoint
  - POST /api/auth/register
  - Files: RegisterCommand.cs, RegisterCommandValidator.cs, RegisterCommandHandler.cs, Register.cs
- **Login**: Command, Validator, Handler, Carter endpoint
  - POST /api/auth/login
  - Files: LoginCommand.cs, LoginCommandValidator.cs, LoginCommandHandler.cs, Login.cs
- **RefreshToken**: Command, Validator, Handler, Carter endpoint
  - POST /api/auth/refresh
  - Files: RefreshCommand.cs, RefreshCommandValidator.cs, RefreshCommandHandler.cs, RefreshToken.cs
- **Logout**: Command, Handler, Carter endpoint
  - POST /api/auth/logout
  - Files: LogoutCommand.cs, LogoutCommandHandler.cs, Logout.cs

**Infrastructure** (located in `src/CitizensPortal.Api/Infrastructure/`):
- Entities: `Database/Entities/Citizen.cs`, `Database/Entities/RefreshToken.cs`
- EF Configuration: `Database/Configurations/CitizenConfiguration.cs`, `Database/Configurations/RefreshTokenConfiguration.cs`
- JWT Service: `Authentication/JwtTokenService.cs` (implements `IJwtTokenService`)
- Password Hashing: BCrypt.Net-Next with work factor 11

**Patterns Used**:
- CQRS via MediatR (IRequest/IRequestHandler)
- ErrorOr for functional error handling
- FluentValidation for request validation
- Carter for Minimal API endpoint organization
- Multi-tenancy with global query filters and AsyncLocal<Guid?> TenantContext
