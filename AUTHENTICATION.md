# Authentication & Authorization Guide

Citizens Portal now includes full ABP Framework authentication and authorization infrastructure with OpenIddict for OAuth 2.0/OpenID Connect.

## Features

✅ **OpenIddict** - Industry-standard OAuth 2.0 and OpenID Connect server
✅ **Identity Management** - User, role, and organization unit management
✅ **Multi-Tenancy** - Complete tenant isolation with subdomain/header resolution
✅ **Permission System** - 160+ fine-grained permissions across all features
✅ **JWT Authentication** - Secure token-based API access
✅ **Swagger Integration** - Test APIs with authentication in Swagger UI

## Quick Start

### 1. Create Initial Migration

```bash
cd src/CitizensPortal.EntityFrameworkCore
dotnet ef migrations add InitialCreate
```

This creates a migration for:
- ABP Identity tables (Users, Roles, Claims)
- ABP Permission Management tables
- ABP Tenant Management tables
- ABP Feature Management tables
- ABP Settings Management tables
- ABP Audit Logging tables
- OpenIddict tables (Applications, Authorizations, Scopes, Tokens)
- Citizens Portal entities (60+ tables)

### 2. Run Database Migrator

```bash
dotnet run --project src/CitizensPortal.DbMigrator/
```

This will:
- Create the database if it doesn't exist
- Apply all pending migrations
- Seed default data:
  - Admin user (admin@abp.io / 1q2w3E*)
  - Admin role with all permissions
  - OAuth clients (Swagger, Web App)
  - OAuth scopes (CitizensPortal)

### 3. Run the Application

```bash
cd src/CitizensPortal.HttpApi.Host
dotnet run
```

Navigate to: https://localhost:44300/swagger

## Using Swagger UI with Authentication

### Step 1: Authorize in Swagger

1. Click the **"Authorize"** button at the top of Swagger UI
2. In the OAuth popup:
   - **client_id**: Already filled (CitizensPortal_Swagger)
   - Check the **CitizensPortal** scope
   - Click **"Authorize"**
3. You'll be redirected to the login page
4. Login with:
   - **Username**: admin@abp.io
   - **Password**: 1q2w3E*
5. Grant permission to the application
6. You'll be redirected back to Swagger with an access token

### Step 2: Test Authenticated APIs

Now you can test any API endpoint that requires authentication:

- `/api/app/citizen` - Citizen management (requires authentication)
- `/api/app/issue-report` - Issue reports (requires authentication)
- `/api/identity/users` - User management (ABP Identity)
- `/api/identity/roles` - Role management (ABP Identity)
- `/api/multi-tenancy/tenants` - Tenant management
- `/api/permission-management/permissions` - Permission management

## Default Users & Credentials

| User | Email | Password | Role | Permissions |
|------|-------|----------|------|-------------|
| Admin | admin@abp.io | 1q2w3E* | admin | All permissions |

## OAuth Clients

### Swagger Client
- **Client ID**: CitizensPortal_Swagger
- **Client Type**: Public
- **Allowed Scopes**: CitizensPortal, openid, profile, email, phone, roles
- **Redirect URI**: https://localhost:44300/swagger/oauth2-redirect.html

### Web App Client
- **Client ID**: CitizensPortal_Web
- **Client Secret**: 1q2w3e*
- **Client Type**: Confidential
- **Grant Types**: Authorization Code, Password, Refresh Token
- **Allowed Scopes**: CitizensPortal, openid, profile, email, phone, roles

## Multi-Tenancy

### Creating a New Tenant

Use the ABP Tenant Management API:

```http
POST /api/multi-tenancy/tenants
Content-Type: application/json
Authorization: Bearer {your-token}

{
  "name": "TenantName",
  "adminEmailAddress": "tenant-admin@example.com",
  "adminPassword": "1q2w3E*"
}
```

### Accessing as a Tenant

Add the tenant header or use subdomain:

**Header-based:**
```
__tenant: {tenant-id-or-name}
```

**Subdomain-based (requires DNS configuration):**
```
https://tenant1.yourdomain.com/api/app/citizen
```

## Permission Management

### View All Permissions

```http
GET /api/permission-management/permissions
  ?providerName=R&providerKey=admin
```

### Grant Permission to Role

```http
PUT /api/permission-management/permissions
  ?providerName=R&providerKey=admin

{
  "permissions": [
    {
      "name": "CitizensPortal.IssueReports.Create",
      "isGranted": true
    }
  ]
}
```

## API Authentication Flow

### Option 1: Password Grant (for testing)

```bash
curl -X POST https://localhost:44300/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=CitizensPortal_Web" \
  -d "client_secret=1q2w3e*" \
  -d "username=admin@abp.io" \
  -d "password=1q2w3E*" \
  -d "scope=CitizensPortal"
```

Response:
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsIn...",
  "token_type": "Bearer",
  "expires_in": 3600,
  "refresh_token": "CfDJ8..."
}
```

### Option 2: Authorization Code Flow (for web apps)

1. Redirect user to authorization endpoint:
```
https://localhost:44300/connect/authorize
  ?client_id=CitizensPortal_Web
  &redirect_uri=https://yourapp.com/signin-oidc
  &response_type=code
  &scope=openid CitizensPortal
```

2. Exchange authorization code for tokens:
```bash
curl -X POST https://localhost:44300/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=authorization_code" \
  -d "client_id=CitizensPortal_Web" \
  -d "client_secret=1q2w3e*" \
  -d "code={authorization-code}" \
  -d "redirect_uri=https://yourapp.com/signin-oidc"
```

### Using the Access Token

Include the token in the Authorization header:

```bash
curl -X GET https://localhost:44300/api/app/citizen \
  -H "Authorization: Bearer {access-token}"
```

## Security Configuration

### Change Default Passwords

**Important:** Change these default credentials in production!

1. Update `appsettings.secrets.json`:
```json
{
  "OpenIddict": {
    "Swagger": {
      "ClientSecret": "your-strong-secret-here"
    },
    "Web": {
      "ClientSecret": "your-strong-secret-here"
    }
  }
}
```

2. Change admin password via API or database

### Configure HTTPS

For production, configure proper SSL certificates in `appsettings.json`:

```json
{
  "App": {
    "SelfUrl": "https://yourdomain.com"
  },
  "AuthServer": {
    "Authority": "https://yourdomain.com",
    "RequireHttpsMetadata": "true"
  }
}
```

## Troubleshooting

### "Unauthorized" Error in Swagger

1. Click "Authorize" button
2. Make sure CitizensPortal scope is checked
3. Complete the login flow
4. Verify the lock icon shows authenticated

### "Invalid Token" Error

- Token may have expired (default: 1 hour)
- Re-authorize in Swagger UI
- For API clients, use refresh token to get new access token

### Database Migration Errors

```bash
# Drop and recreate database (development only!)
dotnet ef database drop --project src/CitizensPortal.EntityFrameworkCore
dotnet ef database update --project src/CitizensPortal.EntityFrameworkCore
```

## Next Steps

1. **Create Additional Roles**: Use `/api/identity/roles` endpoint
2. **Create Users**: Use `/api/identity/users` endpoint
3. **Assign Permissions**: Use `/api/permission-management/permissions` endpoint
4. **Create Tenants**: Use `/api/multi-tenancy/tenants` endpoint
5. **Integrate Frontend**: Use one of the ABP frontend frameworks (Angular, React, Blazor, MVC)

## Resources

- [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Modules/Identity)
- [ABP Permission Management](https://docs.abp.io/en/abp/latest/Authorization)
- [ABP Multi-Tenancy](https://docs.abp.io/en/abp/latest/Multi-Tenancy)
- [OpenIddict Documentation](https://documentation.openiddict.com/)
- [OAuth 2.0 Specification](https://oauth.net/2/)
