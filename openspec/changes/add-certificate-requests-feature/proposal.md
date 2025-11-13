# Change: Add Certificate Requests Feature

## Why

Citizens need to request official certificates such as birth, death, marriage, residence, good conduct, and tax clearance certificates. This feature streamlines the certificate application process with status tracking, fee management, and delivery options.

## What Changes

- Implement certificate request submission and tracking
- Support certificate types: Birth, Death, Marriage, Residence, GoodConduct, TaxClearance
- Status workflow: Submitted → UnderReview → Approved → Ready → Collected/Rejected
- Delivery options: Collection, Email, Postal
- Application fee calculation and payment linkage
- Citizen endpoints for request management
- Admin endpoints for request processing and approval

**Features to implement:**
- Citizen: Submit certificate request (POST /api/certificate-requests)
- Citizen: View my requests (GET /api/certificate-requests/my)
- Citizen: View request details (GET /api/certificate-requests/{id})
- Admin: View all requests (GET /api/admin/certificate-requests)
- Admin: Update request status (PUT /api/admin/certificate-requests/{id}/status)
- Admin: Approve request (PUT /api/admin/certificate-requests/{id}/approve)

## Impact

- **Affected specs:** certificate-requests (new implementation)
- **Affected code:** New feature slice in `src/CitizensPortal.Api/Features/CertificateRequests/`
- **Dependencies:** Payment system integration for fee processing
- **Database changes:** CertificateRequest entity, CertificateRequestConfiguration

This is a new feature implementation following VSA patterns established in previous phases.
