namespace CitizensPortal.Api.Infrastructure.Database.Entities;

public enum VerificationRequestStatus
{
    PendingReview = 1,
    UnderReview = 2,
    Approved = 3,
    Rejected = 4,
    Expired = 5
}
