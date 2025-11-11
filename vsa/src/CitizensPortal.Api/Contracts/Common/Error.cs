namespace CitizensPortal.Api.Contracts.Common;

/// <summary>
/// Represents an error with a code and message.
/// Immutable record for functional error handling.
/// </summary>
public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    // Common application errors
    public static readonly Error NotFound = new("NotFound", "The requested resource was not found");
    public static readonly Error Unauthorized = new("Unauthorized", "You are not authorized to perform this action");
    public static readonly Error Validation = new("Validation", "One or more validation errors occurred");
    public static readonly Error Conflict = new("Conflict", "The operation resulted in a conflict");
    public static readonly Error InternalServerError = new("InternalServerError", "An internal server error occurred");

    /// <summary>
    /// Creates a custom error with the given code and message.
    /// </summary>
    public static Error Custom(string code, string message) => new(code, message);

    /// <summary>
    /// Creates a not found error for a specific entity type.
    /// </summary>
    public static Error NotFound(string entityName, object key) =>
        new($"{entityName}.NotFound", $"{entityName} with key '{key}' was not found");

    /// <summary>
    /// Creates a validation error with a specific message.
    /// </summary>
    public static Error ValidationError(string message) =>
        new("Validation", message);
}
