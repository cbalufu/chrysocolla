namespace CitizensPortal.Api.Infrastructure.Security;

/// <summary>
/// Service for encrypting and hashing national identifiers for POPIA compliance.
/// </summary>
public interface INationalIdEncryptionService
{
    /// <summary>
    /// Encrypts a national ID using AES-256 encryption.
    /// </summary>
    /// <param name="nationalId">The national ID to encrypt</param>
    /// <returns>Base64-encoded encrypted national ID</returns>
    string Encrypt(string nationalId);

    /// <summary>
    /// Decrypts an encrypted national ID.
    /// </summary>
    /// <param name="encryptedNationalId">The Base64-encoded encrypted national ID</param>
    /// <returns>The decrypted national ID</returns>
    string Decrypt(string encryptedNationalId);

    /// <summary>
    /// Generates a SHA-256 hash of a national ID for lookups.
    /// Uses a salt for additional security.
    /// </summary>
    /// <param name="nationalId">The national ID to hash</param>
    /// <returns>Hex-encoded hash</returns>
    string GenerateHash(string nationalId);
}
