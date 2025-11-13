using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CitizensPortal.Api.Infrastructure.FraudDetection;

public sealed class FraudDetectionService : IFraudDetectionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FraudDetectionService> _logger;
    private static readonly Dictionary<string, List<DateTime>> _rateLimitCache = new();
    private static readonly object _rateLimitLock = new();

    public FraudDetectionService(
        ApplicationDbContext context,
        ILogger<FraudDetectionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> GenerateDocumentHashAsync(Stream documentStream)
    {
        // Reset stream position if seekable
        if (documentStream.CanSeek)
        {
            documentStream.Position = 0;
        }

        var hashBytes = await SHA256.HashDataAsync(documentStream);
        var hash = Convert.ToHexString(hashBytes).ToLowerInvariant();

        // Reset stream position for subsequent reads
        if (documentStream.CanSeek)
        {
            documentStream.Position = 0;
        }

        return hash;
    }

    public async Task<bool> IsDuplicateDocumentAsync(string documentHash)
    {
        // Check if this document hash exists in other verification requests
        // We check across all requests, not just the current citizen's
        var exists = await _context.VerificationDocuments
            .AnyAsync(d => !d.IsDeleted && d.BlobName.Contains(documentHash));

        if (exists)
        {
            _logger.LogWarning(
                "Duplicate document detected with hash {DocumentHash}",
                documentHash);
        }

        return exists;
    }

    public async Task RecordDocumentHashAsync(Guid verificationRequestId, string documentHash)
    {
        // Document hash is embedded in the blob name for tracking
        // This method is for potential future enhancements (separate tracking table)
        _logger.LogInformation(
            "Document hash {DocumentHash} recorded for verification request {RequestId}",
            documentHash, verificationRequestId);

        await Task.CompletedTask;
    }

    public Task<bool> HasExceededRateLimitAsync(string ipAddress, int windowMinutes = 60, int maxAttempts = 3)
    {
        lock (_rateLimitLock)
        {
            if (!_rateLimitCache.ContainsKey(ipAddress))
            {
                return Task.FromResult(false);
            }

            var now = DateTime.UtcNow;
            var windowStart = now.AddMinutes(-windowMinutes);

            // Get attempts within the time window
            var recentAttempts = _rateLimitCache[ipAddress]
                .Where(timestamp => timestamp >= windowStart)
                .ToList();

            // Update cache with only recent attempts
            _rateLimitCache[ipAddress] = recentAttempts;

            var exceeded = recentAttempts.Count >= maxAttempts;

            if (exceeded)
            {
                _logger.LogWarning(
                    "Rate limit exceeded for IP {IpAddress}: {Count} attempts in {Window} minutes",
                    ipAddress, recentAttempts.Count, windowMinutes);
            }

            return Task.FromResult(exceeded);
        }
    }

    public Task RecordSubmissionAttemptAsync(string ipAddress)
    {
        lock (_rateLimitLock)
        {
            if (!_rateLimitCache.ContainsKey(ipAddress))
            {
                _rateLimitCache[ipAddress] = new List<DateTime>();
            }

            _rateLimitCache[ipAddress].Add(DateTime.UtcNow);

            _logger.LogDebug(
                "Submission attempt recorded for IP {IpAddress}",
                ipAddress);
        }

        return Task.CompletedTask;
    }

    public async Task<int> GetRejectionCountAsync(Guid citizenId)
    {
        var rejectionCount = await _context.IdentityVerificationRequests
            .Where(r => r.CitizenId == citizenId)
            .Where(r => r.Status == VerificationRequestStatus.Rejected)
            .CountAsync();

        return rejectionCount;
    }

    public async Task<bool> IsSuspiciousActivityAsync(Guid citizenId, string ipAddress)
    {
        // Check for multiple rejections
        var rejectionCount = await GetRejectionCountAsync(citizenId);

        // Check for rate limit violations
        var hasExceededRateLimit = await HasExceededRateLimitAsync(ipAddress, 60, 5);

        // Check for multiple pending requests (should not happen, but check anyway)
        var pendingCount = await _context.IdentityVerificationRequests
            .Where(r => r.CitizenId == citizenId)
            .Where(r => r.Status == VerificationRequestStatus.PendingReview ||
                       r.Status == VerificationRequestStatus.UnderReview)
            .CountAsync();

        var isSuspicious = rejectionCount >= 3 || hasExceededRateLimit || pendingCount > 1;

        if (isSuspicious)
        {
            _logger.LogWarning(
                "Suspicious activity detected for citizen {CitizenId} from IP {IpAddress}: " +
                "Rejections={RejectionCount}, RateLimitExceeded={RateLimitExceeded}, PendingRequests={PendingCount}",
                citizenId, ipAddress, rejectionCount, hasExceededRateLimit, pendingCount);
        }

        return isSuspicious;
    }
}
