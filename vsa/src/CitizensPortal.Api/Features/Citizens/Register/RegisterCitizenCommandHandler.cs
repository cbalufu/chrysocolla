using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Features.Citizens.Register;

public sealed class RegisterCitizenCommandHandler
    : IRequestHandler<RegisterCitizenCommand, ErrorOr<RegisterCitizenResponse>>
{
    private readonly ApplicationDbContext _context;

    public RegisterCitizenCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<RegisterCitizenResponse>> Handle(
        RegisterCitizenCommand request,
        CancellationToken cancellationToken)
    {
        // Check if email already exists for this tenant
        var emailExists = await _context.Citizens
            .AnyAsync(c => c.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            return Error.Conflict(
                code: "Citizen.EmailAlreadyExists",
                description: $"A citizen with email '{request.Email}' already exists");
        }

        // Check if national ID already exists for this tenant (if provided)
        if (!string.IsNullOrEmpty(request.NationalId))
        {
            var nationalIdExists = await _context.Citizens
                .AnyAsync(c => c.NationalId == request.NationalId, cancellationToken);

            if (nationalIdExists)
            {
                return Error.Conflict(
                    code: "Citizen.NationalIdAlreadyExists",
                    description: $"A citizen with national ID '{request.NationalId}' already exists");
            }
        }

        // Hash password using BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create new citizen
        var citizen = new Citizen
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            PhoneNumber = request.PhoneNumber,
            NationalId = request.NationalId,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
            // TenantId will be set automatically by ApplicationDbContext.SaveChangesAsync
        };

        _context.Citizens.Add(citizen);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterCitizenResponse(
            citizen.Id,
            citizen.FirstName,
            citizen.LastName,
            citizen.Email,
            citizen.PhoneNumber,
            citizen.NationalId,
            citizen.DateOfBirth,
            citizen.Address,
            citizen.CreatedAt
        );
    }
}
