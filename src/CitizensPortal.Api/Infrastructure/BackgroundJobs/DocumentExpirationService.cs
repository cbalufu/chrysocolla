using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitizensPortal.Api.Infrastructure.BackgroundJobs;

public sealed class DocumentExpirationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DocumentExpirationService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6); // Run every 6 hours

    public DocumentExpirationService(
        IServiceProvider serviceProvider,
        ILogger<DocumentExpirationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Document Expiration Service started");

        // Wait a bit before first run to allow application to fully start
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessExpiredDocumentsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing expired documents");
            }

            // Wait for next interval
            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Document Expiration Service stopped");
    }

    private async Task ProcessExpiredDocumentsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting expired document cleanup");

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var now = DateTime.UtcNow;

        // Find and mark expired verification requests
        var expiredRequests = await context.IdentityVerificationRequests
            .Where(r => r.ExpiresAt <= now)
            .Where(r => r.Status == VerificationRequestStatus.PendingReview ||
                       r.Status == VerificationRequestStatus.UnderReview)
            .ToListAsync(cancellationToken);

        foreach (var request in expiredRequests)
        {
            request.Status = VerificationRequestStatus.Expired;
            _logger.LogInformation(
                "Marked verification request {RequestId} (Reference: {ReferenceNumber}) as expired",
                request.Id, request.ReferenceNumber);
        }

        if (expiredRequests.Any())
        {
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Marked {Count} verification requests as expired", expiredRequests.Count);
        }

        // Find expired documents (90+ days old)
        var documentExpiryDate = now.AddDays(-90);
        var expiredDocuments = await context.VerificationDocuments
            .Where(d => d.ExpiresAt <= now)
            .Where(d => !d.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var document in expiredDocuments)
        {
            document.IsDeleted = true;
            document.DeletedAt = now;
            _logger.LogInformation(
                "Marked document {DocumentId} as deleted (expired on {ExpiresAt})",
                document.Id, document.ExpiresAt);
        }

        if (expiredDocuments.Any())
        {
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Marked {Count} documents as deleted (expired)", expiredDocuments.Count);
        }

        // Find documents from completed/rejected requests that are old (keep for 90 days, then delete)
        var oldCompletedDocuments = await context.VerificationDocuments
            .Include(d => d.VerificationRequest)
            .Where(d => !d.IsDeleted)
            .Where(d => d.VerificationRequest.Status == VerificationRequestStatus.Approved ||
                       d.VerificationRequest.Status == VerificationRequestStatus.Rejected ||
                       d.VerificationRequest.Status == VerificationRequestStatus.Expired)
            .Where(d => d.VerificationRequest.ReviewedAt.HasValue &&
                       d.VerificationRequest.ReviewedAt.Value <= documentExpiryDate)
            .ToListAsync(cancellationToken);

        foreach (var document in oldCompletedDocuments)
        {
            document.IsDeleted = true;
            document.DeletedAt = now;
            _logger.LogInformation(
                "Marked old document {DocumentId} from completed request as deleted",
                document.Id);
        }

        if (oldCompletedDocuments.Any())
        {
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation(
                "Marked {Count} documents from old completed requests as deleted",
                oldCompletedDocuments.Count);
        }

        var totalProcessed = expiredRequests.Count + expiredDocuments.Count + oldCompletedDocuments.Count;
        _logger.LogInformation(
            "Completed expired document cleanup. Total items processed: {TotalProcessed}",
            totalProcessed);

        // TODO: In production, also delete actual blob files from storage
        // Example: await _blobStorageService.DeleteBlobsAsync(expiredDocuments.Select(d => d.BlobName));
    }
}
