# Implementation Tasks

## 1. Database Setup
- [ ] 1.1 Create Notification entity in `Infrastructure/Database/Entities/Notification.cs`
- [ ] 1.2 Create NotificationConfiguration in `Infrastructure/Database/Configurations/`
- [ ] 1.3 Add DbSet<Notification> to ApplicationDbContext
- [ ] 1.4 Create and apply EF migration

## 2. Email Infrastructure
- [ ] 2.1 Install email package (MailKit or similar)
- [ ] 2.2 Create IEmailService interface in `Infrastructure/Email/`
- [ ] 2.3 Implement EmailService with SMTP support
- [ ] 2.4 Add email configuration to appsettings.json
- [ ] 2.5 Register EmailService in DI container

## 3. Citizen Features
- [ ] 3.1 Implement GetUserNotifications (GET /api/notifications/my)
- [ ] 3.2 Implement MarkAsRead (PUT /api/notifications/{id}/read)
- [ ] 3.3 Add FluentValidation validators
- [ ] 3.4 Add MediatR handlers
- [ ] 3.5 Add Carter endpoint modules

## 4. Admin Features
- [ ] 4.1 Implement SendCustomNotification (POST /api/admin/notifications)
- [ ] 4.2 Add validation and authorization
- [ ] 4.3 Add MediatR handler
- [ ] 4.4 Add Carter endpoint module

## 5. Notification Triggers (Optional)
- [ ] 5.1 Add notification on bill creation
- [ ] 5.2 Add notification on application status change
- [ ] 5.3 Add notification on issue status change
- [ ] 5.4 Add notification on ticket response

## 6. Testing
- [ ] 6.1 Build and test implementation
- [ ] 6.2 Verify email sending works
- [ ] 6.3 Test notification history retrieval
- [ ] 6.4 Test multi-tenancy isolation

## 7. Documentation
- [ ] 7.1 Update OpenAPI documentation
- [ ] 7.2 Commit and push changes
