# Implementation Tasks

## 1. Database Setup
- [ ] 1.1 Create CertificateRequest entity in `Infrastructure/Database/Entities/CertificateRequest.cs`
- [ ] 1.2 Create CertificateRequestConfiguration in `Infrastructure/Database/Configurations/`
- [ ] 1.3 Add DbSet<CertificateRequest> to ApplicationDbContext
- [ ] 1.4 Create and apply EF migration

## 2. Citizen Features
- [ ] 2.1 Implement SubmitCertificateRequest (POST /api/certificate-requests)
- [ ] 2.2 Implement GetUserCertificateRequests (GET /api/certificate-requests/my)
- [ ] 2.3 Implement GetCertificateRequestById (GET /api/certificate-requests/{id})
- [ ] 2.4 Add FluentValidation validators
- [ ] 2.5 Add MediatR handlers
- [ ] 2.6 Add Carter endpoint modules

## 3. Admin Features
- [ ] 3.1 Implement GetAllCertificateRequests (GET /api/admin/certificate-requests)
- [ ] 3.2 Implement UpdateRequestStatus (PUT /api/admin/certificate-requests/{id}/status)
- [ ] 3.3 Implement ApproveRequest (PUT /api/admin/certificate-requests/{id}/approve)
- [ ] 3.4 Add validation and authorization (StaffOrAdmin)
- [ ] 3.5 Add MediatR handlers
- [ ] 3.6 Add Carter endpoint modules

## 4. Business Logic
- [ ] 4.1 Implement fee calculation logic per certificate type
- [ ] 4.2 Add delivery method handling
- [ ] 4.3 Add status workflow validation
- [ ] 4.4 Add reference number generation (CERT-{TYPE}-{YYYYMMDD}-{GUID})

## 5. Testing
- [ ] 5.1 Build and test implementation
- [ ] 5.2 Test all certificate types
- [ ] 5.3 Test status workflow
- [ ] 5.4 Test multi-tenancy isolation

## 6. Documentation
- [ ] 6.1 Update OpenAPI documentation
- [ ] 6.2 Commit and push changes
