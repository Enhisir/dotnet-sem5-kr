using System.Security.Cryptography;
using TicTacToe.Common.Helpers.Abstractions;

namespace TicTacToe.Common.Helpers;

public class PasswordHasherService : IPasswordHasherService
{
    private const int SaltSize = 16; //128 / 8, length in bytes
    private const int KeySize = 32; //256 / 8, length in bytes
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;
    private const char SaltDelimiter = ';';

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);
        return string.Join(SaltDelimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool Validate(string passwordHash, string password)
    {
        var pwdElements = passwordHash.Split(SaltDelimiter);
        var salt = Convert.FromBase64String(pwdElements[0]);
        var hash = Convert.FromBase64String(pwdElements[1]);
        var hashInput = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName, KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}