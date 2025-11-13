using System.Security.Cryptography;
using System.Text;

namespace CitizensPortal.Api.Infrastructure.Security;

/// <summary>
/// Implementation of national ID encryption service using AES-256 and SHA-256.
/// </summary>
public sealed class NationalIdEncryptionService : INationalIdEncryptionService
{
    private readonly byte[] _encryptionKey;
    private readonly byte[] _hashSalt;

    public NationalIdEncryptionService(IConfiguration configuration)
    {
        // Get encryption key from configuration
        var keyString = configuration["Security:NationalIdEncryptionKey"]
            ?? throw new InvalidOperationException("National ID encryption key not configured");

        var saltString = configuration["Security:NationalIdHashSalt"]
            ?? throw new InvalidOperationException("National ID hash salt not configured");

        // Convert to bytes - key must be 32 bytes for AES-256
        _encryptionKey = DeriveKey(keyString, 32);
        _hashSalt = Encoding.UTF8.GetBytes(saltString);
    }

    public string Encrypt(string nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            throw new ArgumentException("National ID cannot be empty", nameof(nationalId));
        }

        using var aes = Aes.Create();
        aes.Key = _encryptionKey;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(nationalId);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Prepend IV to encrypted data for decryption
        var result = new byte[aes.IV.Length + encryptedBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string encryptedNationalId)
    {
        if (string.IsNullOrWhiteSpace(encryptedNationalId))
        {
            throw new ArgumentException("Encrypted national ID cannot be empty", nameof(encryptedNationalId));
        }

        var fullCipher = Convert.FromBase64String(encryptedNationalId);

        using var aes = Aes.Create();
        aes.Key = _encryptionKey;

        // Extract IV from the beginning
        var iv = new byte[aes.IV.Length];
        var cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        var decryptedBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

        return Encoding.UTF8.GetString(decryptedBytes);
    }

    public string GenerateHash(string nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            throw new ArgumentException("National ID cannot be empty", nameof(nationalId));
        }

        // Combine national ID with salt
        var inputBytes = Encoding.UTF8.GetBytes(nationalId);
        var saltedInput = new byte[inputBytes.Length + _hashSalt.Length];
        Buffer.BlockCopy(inputBytes, 0, saltedInput, 0, inputBytes.Length);
        Buffer.BlockCopy(_hashSalt, 0, saltedInput, inputBytes.Length, _hashSalt.Length);

        // Generate SHA-256 hash
        var hashBytes = SHA256.HashData(saltedInput);

        // Convert to hex string
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    /// <summary>
    /// Derives a key of specified length from a passphrase using PBKDF2.
    /// </summary>
    private static byte[] DeriveKey(string passphrase, int keyLength)
    {
        // Use a fixed salt for key derivation (in production, this should be from config)
        var salt = Encoding.UTF8.GetBytes("CitizensPortal.FederatedIdentity.Salt.V1");

        using var deriveBytes = new Rfc2898DeriveBytes(
            passphrase,
            salt,
            iterations: 10000,
            HashAlgorithmName.SHA256);

        return deriveBytes.GetBytes(keyLength);
    }
}
