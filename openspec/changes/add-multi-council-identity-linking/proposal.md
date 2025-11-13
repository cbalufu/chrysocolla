# Change: Multi-Council Ratepayer Identity Linking System

## Why

Currently, the platform operates with isolated council tenants where citizens must maintain separate identities for each council they interact with. This creates friction for citizens who own property or require services across multiple councils. A unified identity system will enable seamless cross-council interactions while maintaining data sovereignty for each council.

Citizens need a single digital identity that links all their properties, bills, and service interactions across different councils, accessible through a national identifier (ID or TIN).

## What Changes

- **BREAKING**: Citizen identity model enhanced with national identifier support (ID/TIN) for cross-council linking
- Add cross-tenant citizen profile federation allowing citizens to view aggregated data from all councils they're registered with
- Implement council self-registration workflow enabling councils to onboard themselves as tenants
- Add council branding and customization system for council-specific theming
- Implement dual-path citizen verification:
  - **Self-service**: Citizens upload identity documents for online verification
  - **In-person**: Citizens visit council offices for verification and account claiming
- Add data synchronization hooks for councils to sync balances and account data from external systems
- Implement cross-council property, billing, and issue aggregation views
- Add identity verification workflow with document upload and approval processes

## Impact

**Affected Specs:**
- `citizen-management`: New national ID linking, cross-council profile federation
- `tenant-management`: Council self-registration, branding/customization system
- `identity-verification` (new capability): Document-based identity verification
- `property-management`: Cross-council property aggregation views
- `billing`: Cross-council bill aggregation views
- `issue-reporting`: Cross-council issue aggregation views

**Affected Code:**
- `Domain/Citizens/Citizen.cs`: Add national identifier fields, verification status
- `Application.Contracts/Citizens/CitizenDto.cs`: Add cross-council profile data
- `Application/Citizens/CitizenAppService.cs`: Implement federation logic
- `Domain/Tenants/*`: Add council metadata, branding configuration
- `Application/IdentityVerification/*` (new): Verification workflows
- `HttpApi/Controllers/*`: New endpoints for verification and aggregation

**Migration Requirements:**
- Database schema changes for citizen national identifiers
- Existing citizens will need to link their national IDs (optional migration)
- Council tenants can opt-in to federation features

**Security Considerations:**
- National ID/TIN numbers must be encrypted at rest
- Document verification requires secure blob storage with expiration
- Cross-tenant queries must maintain data isolation and authorization
- Audit logging for all identity verification activities
