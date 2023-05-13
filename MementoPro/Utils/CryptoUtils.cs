using Konscious.Security.Cryptography;
using System.Text;

namespace MementoPro.Utilities;
public static class CryptoUtils
{
    private static readonly UTF8Encoding _utf8Encoding = new();

    public static string Encrypt(this string str, int symbolsCount)
    {
        var strBytes = _utf8Encoding.GetBytes(str);

        var argon2 = new Argon2i(strBytes)
        {
            DegreeOfParallelism = 1,
            MemorySize = 4,
            Iterations = 1
        };

        var encryptedStrBytes = argon2.GetBytes(symbolsCount);
        var encryptedStr = _utf8Encoding.GetString(encryptedStrBytes);

        return encryptedStr;
    }
}