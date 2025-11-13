namespace CitizensPortal.Api.Infrastructure.FraudDetection;

public interface IFraudDetectionService
{
    Task<string> GenerateDocumentHashAsync(Stream documentStream);

    Task<bool> IsDuplicateDocumentAsync(string documentHash);

    Task RecordDocumentHashAsync(Guid verificationRequestId, string documentHash);

    Task<bool> HasExceededRateLimitAsync(string ipAddress, int windowMinutes = 60, int maxAttempts = 3);

    Task RecordSubmissionAttemptAsync(string ipAddress);

    Task<int> GetRejectionCountAsync(Guid citizenId);

    Task<bool> IsSuspiciousActivityAsync(Guid citizenId, string ipAddress);
}
