namespace CitizensPortal.Api.Infrastructure.MultiTenancy;

/// <summary>
/// Middleware that resolves the current tenant from the HTTP request.
/// Priority: JWT claim -> Subdomain -> X-Tenant-Id header
/// </summary>
public sealed class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantAccessor tenantAccessor)
    {
        var tenantId = ResolveTenantId(context);
        tenantAccessor.SetTenant(tenantId);

        await _next(context);
    }

    private static Guid? ResolveTenantId(HttpContext context)
    {
        // Priority 1: JWT claim (tenant_id)
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = context.User.FindFirst("tenant_id");
            if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantIdFromClaim))
            {
                return tenantIdFromClaim;
            }
        }

        // Priority 2: Subdomain (tenant1.citizensportal.com -> tenant1)
        var host = context.Request.Host.Host;
        if (!string.IsNullOrEmpty(host))
        {
            var parts = host.Split('.');
            if (parts.Length > 2 && parts[0] != "www")
            {
                var subdomain = parts[0];
                // In a real app, you'd lookup tenant by subdomain from cache/database
                // For now, we'll just try to parse if it's a GUID
                if (Guid.TryParse(subdomain, out var tenantIdFromSubdomain))
                {
                    return tenantIdFromSubdomain;
                }
            }
        }

        // Priority 3: X-Tenant-Id header
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantHeader))
        {
            if (Guid.TryParse(tenantHeader.ToString(), out var tenantIdFromHeader))
            {
                return tenantIdFromHeader;
            }
        }

        // No tenant context (could be public endpoints or admin operations)
        return null;
    }
}
