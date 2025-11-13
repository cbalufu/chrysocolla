# Design: Multi-Council Ratepayer Identity Linking System

## Context

The Citizens Portal currently operates as a multi-tenant system where each council is an isolated tenant. Citizens who interact with multiple councils must create separate accounts for each. This design introduces a federated identity system that allows citizens to use a single national identifier (National ID or TIN) to link and access all their accounts across different councils while maintaining the existing tenant isolation architecture.

### Stakeholders
- **Citizens/Ratepayers**: Primary users who need unified access across councils
- **Council Administrators**: Manage citizen verification and onboarding
- **System Administrators**: Oversee federation infrastructure and security

### Constraints
- Must maintain existing ABP multi-tenancy data isolation
- Must comply with POPIA (South African data privacy regulations)
- Must support councils opting out of federation
- Must not break existing single-council deployments
- Must support both online and offline verification workflows

## Goals / Non-Goals

### Goals
- Enable citizens to link accounts across multiple councils using national ID/TIN
- Provide aggregated view of properties, bills, and issues across councils
- Support council self-registration and customization
- Implement secure identity verification workflows
- Maintain complete data isolation between tenants
- Support data synchronization from council systems

### Non-Goals
- Single sign-on (SSO) across councils (out of scope for MVP)
- Real-time synchronization from external council systems (batch sync only)
- Automatic property discovery (citizens must claim properties)
- Cross-council payments in single transaction (handle separately)
- Mobile app implementation (API-only for this change)

## Decisions

### Decision 1: Federation Model - Hub and Spoke

**Choice**: Implement a "hub" citizen profile at the host tenant level that federates with "spoke" tenant-specific citizen records.

**Rationale**:
- Preserves tenant data isolation (critical for multi-tenancy)
- Allows councils to opt-in/opt-out of federation
- Supports existing ABP tenant architecture without major refactoring
- National identifier stored only in hub profile (encrypted)
- Tenant-specific data remains in respective tenants

**Alternatives Considered**:
1. **Shared identity table across tenants**: Rejected - violates ABP tenant isolation principles
2. **Duplicate national ID in each tenant**: Rejected - data consistency and security concerns
3. **External identity provider**: Rejected - requires national infrastructure not yet available

### Decision 2: National Identifier Storage

**Choice**: Store national ID/TIN encrypted in a new `CitizenFederatedProfile` table at host tenant level, with hashed lookup index.

**Rationale**:
- Encryption at rest for sensitive PII (POPIA compliance)
- Hashed index enables efficient lookups without exposing raw values
- Centralized storage prevents duplicate registrations
- Can be extended to support multiple identifier types (passport, refugee ID)

**Schema**:
```
CitizenFederatedProfile (Host tenant only)
  - Id (Guid)
  - NationalIdType (enum: NationalId, TIN, Passport)
  - NationalIdEncrypted (string, encrypted)
  - NationalIdHash (string, SHA256 hash for lookup)
  - VerificationStatus (enum: Pending, Verified, Rejected)
  - VerificationMethod (enum: SelfService, InPerson)
  - VerifiedAt (DateTime?)
  - VerifiedByTenantId (Guid?)
  - LinkedCitizenProfiles (List<TenantId, CitizenId>)
```

### Decision 3: Identity Verification Workflow

**Choice**: Implement dual-path verification with document upload and council approval.

**Paths**:
1. **Self-Service Path**:
   - Citizen uploads ID document photos
   - System validates document format and extracts data (future: OCR)
   - Council admin reviews and approves/rejects
   - Upon approval, citizen can link to properties

2. **In-Person Path**:
   - Citizen visits council office with physical documents
   - Council staff verifies identity on-site
   - Staff grants immediate verification
   - Citizen can immediately claim accounts

**Rationale**:
- Accommodates citizens with/without digital access
- Balances security with usability
- Allows councils to control verification standards
- Reduces fraud through human review
- Supports rural areas with limited internet

### Decision 4: Council Self-Registration

**Choice**: Create council registration workflow that creates tenant with initial admin, requires system admin approval.

**Workflow**:
1. Council representative submits registration form with:
   - Council name, region, contact details
   - Administrator information
   - Supporting documentation
2. System creates tenant in "pending" state
3. System admin reviews and approves/rejects
4. Upon approval:
   - Tenant activated
   - Admin account created
   - Council can customize branding

**Rationale**:
- Reduces onboarding friction for councils
- Maintains governance through approval gate
- Allows verification of legitimate councils
- Creates audit trail of council onboarding

### Decision 5: Council Customization

**Choice**: Store council branding as JSON in TenantProperties with specific structure for logo, colors, and styling.

**Customization Options**:
- Council logo (blob storage)
- Primary/secondary colors
- Font selections (from approved list)
- Custom CSS overrides (sanitized)
- Contact information and social media links

**Storage**:
```json
{
  "branding": {
    "logoUrl": "/api/blobs/...",
    "colors": {
      "primary": "#1976D2",
      "secondary": "#424242"
    },
    "fonts": {
      "heading": "Roboto",
      "body": "Open Sans"
    },
    "customCss": "..."
  }
}
```

**Rationale**:
- Flexible without schema changes
- Scoped to tenant (no cross-contamination)
- Can version branding structure
- Easy to extend with new options

### Decision 6: Cross-Council Data Aggregation

**Choice**: Implement federated queries that retrieve data from multiple tenants using ABP's `IDataFilter` and `CurrentTenant.Change()`.

**Approach**:
1. Resolve citizen's federated profile by national ID hash
2. Retrieve list of linked tenant IDs
3. For each tenant:
   - Temporarily switch tenant context
   - Query tenant-specific data (properties, bills, issues)
   - Collect results
4. Aggregate and return unified view

**Rationale**:
- Leverages existing ABP multi-tenancy infrastructure
- Maintains authorization checks per tenant
- No schema changes to existing entities
- Clear separation of concerns

**Performance Considerations**:
- Cache federated profile lookups
- Implement pagination for aggregated views
- Consider async parallel queries for multiple tenants
- Add circuit breakers for tenant queries

### Decision 7: Data Synchronization

**Choice**: Provide webhook endpoints and batch import APIs for councils to push data from their systems.

**Synchronization Methods**:
1. **REST API Endpoints**: For real-time updates
   - Require tenant-specific API keys
   - Validate and transform incoming data
   - Trigger background jobs for processing

2. **Batch CSV/Excel Import**: For bulk updates
   - Upload files to blob storage
   - Process via background jobs
   - Generate reconciliation reports

3. **Webhook Subscriptions**: For event-driven updates
   - Councils configure webhook URLs
   - System sends events (bill created, payment received)

**Rationale**:
- Flexible integration options for different council systems
- Async processing prevents performance impact
- Audit trail via import logs
- Councils control sync frequency

## Risks / Trade-offs

### Risk 1: Performance Impact of Cross-Tenant Queries
**Mitigation**:
- Implement aggressive caching of federated profiles
- Use pagination for all aggregated views
- Add database indexes on national ID hash
- Monitor query performance and optimize hot paths
- Consider read replicas for heavy aggregation queries

### Risk 2: National ID Data Breach
**Mitigation**:
- Encrypt national IDs at rest using AES-256
- Use separate encryption keys per environment
- Store keys in secure key vault (Azure Key Vault)
- Implement key rotation policy
- Audit all access to federated profile table
- Rate-limit national ID lookups

### Risk 3: Identity Verification Fraud
**Mitigation**:
- Require high-quality document photos
- Implement liveness detection (future)
- Council admin final approval required
- Log all verification attempts with IP/device fingerprinting
- Implement cooling-off period after rejection
- Flag suspicious patterns (multiple IDs, rapid submissions)

### Risk 4: Council Impersonation
**Mitigation**:
- Require documentation during registration
- System admin approval gate
- Email/phone verification for council contact
- Audit log of all council registrations
- Periodic review of active councils

### Risk 5: POPIA Compliance
**Mitigation**:
- Encrypt PII at rest
- Implement data subject access request (DSAR) workflows
- Provide citizen data export functionality
- Document data retention policies
- Obtain explicit consent for cross-council data sharing
- Allow citizens to unlink councils

### Trade-off 1: Complexity vs. Flexibility
**Decision**: Accept increased system complexity for federation benefits
**Rationale**: Citizens need unified experience across councils; complexity is manageable with good architecture

### Trade-off 2: Performance vs. Data Freshness
**Decision**: Use eventual consistency for aggregated views (cache with TTL)
**Rationale**: Acceptable for most citizen use cases; critical operations query directly

### Trade-off 3: Security vs. Usability
**Decision**: Require document verification but support offline path
**Rationale**: Balances fraud prevention with accessibility

## Migration Plan

### Phase 1: Foundation (Weeks 1-2)
1. Create `CitizenFederatedProfile` entity and table
2. Implement encryption/hashing utilities
3. Add national ID fields to Citizen registration
4. Deploy database migrations

### Phase 2: Identity Verification (Weeks 3-4)
1. Implement document upload and storage
2. Create verification workflows
3. Build admin review interface (API endpoints)
4. Add verification status tracking

### Phase 3: Council Management (Weeks 5-6)
1. Implement council registration workflow
2. Add tenant approval system
3. Build branding customization
4. Implement branding API endpoints

### Phase 4: Federation (Weeks 7-8)
1. Implement cross-tenant query logic
2. Create aggregated view APIs
3. Add caching layer
4. Performance testing and optimization

### Phase 5: Data Sync (Weeks 9-10)
1. Build sync API endpoints
2. Implement batch import processing
3. Create webhook infrastructure
4. Documentation and testing

### Rollback Plan
- Feature flags for federation functionality
- Can disable cross-council features without affecting single-council operations
- Data migrations are additive (no data loss on rollback)
- Federated profiles can be orphaned if feature disabled (cleaned up later)

### Testing Strategy
- Unit tests for encryption/hashing utilities
- Integration tests for cross-tenant queries
- Load tests for aggregated views with 100+ linked properties
- Security tests for authorization bypass attempts
- End-to-end tests for both verification workflows

## Open Questions

1. **Q**: Should we support automatic property discovery based on national ID from external registries?
   **A**: Deferred to Phase 2 - requires integration with national property registries

2. **Q**: How do we handle citizens moving between councils?
   **A**: Properties remain linked; citizen views all historical data; bills continue from relevant councils

3. **Q**: Should councils be able to reject linking requests?
   **A**: Yes - councils can disable federation; citizens can only link to councils with federation enabled

4. **Q**: What happens if a citizen's national ID changes (marriage, legal name change)?
   **A**: Support ID update workflow with re-verification; maintain historical link for audit

5. **Q**: How granular should council customization be?
   **A**: Start with logo, colors, fonts; extend based on feedback; no custom JavaScript for security

6. **Q**: Should we support bulk citizen import with pre-verified identities?
   **A**: Yes - add admin import feature for councils migrating from existing systems

## Success Metrics

- % of citizens linking multiple councils (target: 30% within 6 months)
- Average verification turnaround time (target: <24 hours for self-service)
- Council adoption rate (target: 50% of councils opt-in within 1 year)
- System performance: <500ms for aggregated property views (p95)
- Zero security breaches related to federated identities
