# Database Migration Instructions

## Phase 1: Federated Profiles and Identity Linking

The foundation for multi-council identity linking has been implemented. To apply the database changes, run the following commands:

### Generate Migration

```bash
cd src/CitizensPortal.Api
dotnet ef migrations add AddFederatedProfilesAndIdentityLinking --context ApplicationDbContext
```

### Apply Migration

```bash
dotnet ef database update --context ApplicationDbContext
```

### What's Included

This migration adds:

1. **New Enums**:
   - `NationalIdType` (NationalId, TIN, Passport)
   - `VerificationStatus` (Pending, Verified, Rejected)
   - `VerificationMethod` (SelfService, InPerson, BulkImport)
   - `ConsentStatus` (NotProvided, Granted, Revoked)

2. **New Tables**:
   - `CitizenFederatedProfiles` - Hub for cross-council citizen identity
   - `LinkedCitizenProfiles` - Links federated profiles to tenant-specific citizens

3. **Updated Tables**:
   - `Citizens` - Added `NationalIdType`, `HasFederatedProfile`, `FederatedProfileId`

4. **New Indexes**:
   - Unique index on `CitizenFederatedProfiles.NationalIdHash` for fast lookups
   - Composite indexes on `LinkedCitizenProfiles` for efficient queries
   - Index on `Citizens.FederatedProfileId`

5. **Security**:
   - Encryption service for national IDs (AES-256)
   - Hashing service for national ID lookups (SHA-256)
   - Configuration keys added to appsettings.json

### Security Configuration

**IMPORTANT**: Before deploying to production, update the following in appsettings.json or environment variables:

- `Security:NationalIdEncryptionKey` - Must be a strong 32-byte+ key
- `Security:NationalIdHashSalt` - Must be a unique salt value

For production, use Azure Key Vault or similar secure key management system.

### Rollback

If needed, rollback the migration:

```bash
dotnet ef database update <PreviousMigrationName> --context ApplicationDbContext
```

Then remove the migration:

```bash
dotnet ef migrations remove --context ApplicationDbContext
```
