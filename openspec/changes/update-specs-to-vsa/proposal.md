# Change: Update Specs to Reflect VSA Architecture

## Why

The existing OpenSpec specifications describe an ABP Framework architecture with layered separation (Application, Domain, HttpApi), but the actual implementation uses Vertical Slice Architecture (VSA) with ASP.NET Core Minimal APIs, Carter, and MediatR. The specs need to be updated to accurately reflect what has been built.

## What Changes

- Update all implemented capability specs to reflect VSA architecture patterns
- Replace ABP references (Controllers, AppServices) with VSA references (Features, Carter modules, MediatR handlers)
- Update file paths to match actual VSA structure: `src/CitizensPortal.Api/Features/`
- Update data models to reflect actual entity implementations in VSA
- Update endpoint patterns from ABP REST controllers to Minimal API routes
- Preserve all functional requirements while updating implementation details

**Affected Capabilities:**
- citizen-management (Authentication)
- issue-reporting
- applications-permits
- billing
- payments
- document-management
- appointments
- service-requests
- support-tickets
- emergency-alerts

## Impact

- **Affected specs:** 10 capability specs (all currently implemented features)
- **Affected code:** None (specs updated to match existing code)
- **Breaking changes:** None (documentation update only)
- **Architecture shift:** Documented shift from ABP Framework to VSA with Minimal APIs

This is purely a documentation update to align specs with the actual codebase. No code changes required.
