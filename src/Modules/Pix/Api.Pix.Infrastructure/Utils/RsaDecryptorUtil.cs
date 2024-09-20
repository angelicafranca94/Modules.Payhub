using Api.Pix.Domain.Interfaces.Utils;
using Api.Pix.Infrastructure.Settings;
using CrossCutting.PayHub.Shared.Constants;
using CrossCutting.PayHub.Shared.Exceptions;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Api.Pix.Infrastructure.Utils;

public class RsaDecryptorUtil : IRsaDecryptorUtil
{
    private readonly KeyForDecryptCodeDebtSettings _privateKey;

    public RsaDecryptorUtil(IOptions<KeyForDecryptCodeDebtSettings> privateKey)
    {
        _privateKey = privateKey.Value;
    }

    public int Decrypt(string encryptedData)
    {
        string privateKey = File.ReadAllText(_privateKey.Path);

        try
        {
            var encryptedDataFromBase64Url = FromBase64Url(encryptedData);

            byte[] encryptedBytes = Convert.FromBase64String(encryptedDataFromBase64Url);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(privateKey);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
                var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                return int.Parse(decryptedData);
            }
        }
        catch (Exception ex)
        {
            throw new DecryptException(ErrorsConstants.DecryptNotWork + ex.Message);
        }
    }

    private static string FromBase64Url(string encryptedData)
    {
        string padding = new string('=', (4 - encryptedData.Length % 4) % 4);
        string base64 = encryptedData.Replace('-', '+').Replace('_', '/') + padding;
        return base64;
    }
}