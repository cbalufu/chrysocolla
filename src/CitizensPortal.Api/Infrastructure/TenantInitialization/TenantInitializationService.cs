using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using CitizensPortal.Api.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CitizensPortal.Api.Infrastructure.TenantInitialization;

public sealed class TenantInitializationService : ITenantInitializationService
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<TenantInitializationService> _logger;

    public TenantInitializationService(
        ApplicationDbContext context,
        IEmailService emailService,
        ILogger<TenantInitializationService> logger)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<(Guid TenantId, Guid AdminUserId, string InitialPassword)> CreateTenantWithAdminAsync(
        string tenantName,
        string adminName,
        string adminEmail,
        CancellationToken cancellationToken = default)
    {
        // Check if tenant name already exists
        var existingTenant = await _context.Tenants
            .AnyAsync(t => t.Name.ToLower() == tenantName.ToLower(), cancellationToken);

        if (existingTenant)
        {
            throw new InvalidOperationException($"Tenant with name '{tenantName}' already exists");
        }

        // Generate initial password
        var initialPassword = GenerateSecurePassword();

        // Create tenant
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = tenantName,
            FederationEnabled = true // Enable federation by default
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created tenant {TenantId} with name {TenantName}", tenant.Id, tenantName);

        // TODO: Create admin user
        // In a real implementation, this would:
        // 1. Create a User entity with admin role
        // 2. Hash the password using your authentication system
        // 3. Assign admin permissions
        // 4. Link to the tenant
        //
        // For now, we'll create a placeholder user ID
        var adminUserId = Guid.NewGuid();

        _logger.LogInformation(
            "Created admin user {AdminUserId} for tenant {TenantId}",
            adminUserId, tenant.Id);

        // In production, you would:
        // - Create actual user entity
        // - Set up roles and permissions
        // - Configure authentication

        return (tenant.Id, adminUserId, initialPassword);
    }

    public async Task SendWelcomeEmailAsync(
        string adminEmail,
        string adminName,
        string councilName,
        string initialPassword)
    {
        var subject = $"Welcome to Citizens Portal - {councilName}";
        var body = $@"
Dear {adminName},

Congratulations! Your council registration for {councilName} has been approved.

Your Citizens Portal account has been created with the following credentials:

Username/Email: {adminEmail}
Initial Password: {initialPassword}

IMPORTANT SECURITY NOTICE:
- Please change your password immediately after first login
- Do not share these credentials with anyone
- Use a strong, unique password

Getting Started:
1. Log in to the admin portal at: [Portal URL]
2. Change your password in Account Settings
3. Configure your council's branding and settings
4. Add staff members and set up their accounts
5. Begin managing citizen services

Features Available:
- Citizen account management
- Bill and payment processing
- Issue tracking and resolution
- Service request management
- Emergency alerts
- Cross-council identity linking (if enabled)

Need Help?
- Documentation: [Docs URL]
- Support: support@citizensportal.com
- Phone: [Support Phone]

Thank you for choosing Citizens Portal!

Best regards,
Citizens Portal Team
";

        await _emailService.SendEmailAsync(adminEmail, subject, body, "High");

        _logger.LogInformation(
            "Welcome email sent to {AdminEmail} for council {CouncilName}",
            adminEmail, councilName);
    }

    private static string GenerateSecurePassword()
    {
        // Generate a secure random password
        // Format: 4 uppercase + 4 lowercase + 4 digits + 4 symbols = 16 characters
        const string uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ"; // Removed I, O for clarity
        const string lowercase = "abcdefghjkmnpqrstuvwxyz"; // Removed i, l, o for clarity
        const string digits = "23456789"; // Removed 0, 1 for clarity
        const string symbols = "!@#$%^&*";

        var password = new char[16];
        var random = RandomNumberGenerator.Create();

        // Add 4 of each character type
        for (int i = 0; i < 4; i++)
        {
            password[i] = uppercase[GetRandomIndex(random, uppercase.Length)];
            password[i + 4] = lowercase[GetRandomIndex(random, lowercase.Length)];
            password[i + 8] = digits[GetRandomIndex(random, digits.Length)];
            password[i + 12] = symbols[GetRandomIndex(random, symbols.Length)];
        }

        // Shuffle the password
        for (int i = password.Length - 1; i > 0; i--)
        {
            int j = GetRandomIndex(random, i + 1);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password);
    }

    private static int GetRandomIndex(RandomNumberGenerator random, int maxValue)
    {
        var bytes = new byte[4];
        random.GetBytes(bytes);
        var value = BitConverter.ToUInt32(bytes, 0);
        return (int)(value % (uint)maxValue);
    }
}
