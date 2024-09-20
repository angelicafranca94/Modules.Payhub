using Api.Pix.Infrastructure.Settings;
using Api.Pix.Infrastructure.Utils;
using CrossCutting.PayHub.Shared.Exceptions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Api.Pix.UnitTests.Infrastructure.Utils;
public class RsaDecryptorUtilTests
{
    private readonly IOptions<KeyForDecryptCodeDebtSettings> _privateKey;
    private readonly RsaDecryptorUtil _rsaDecryptorUtil;

    public RsaDecryptorUtilTests()
    {
        _privateKey = Options.Create(new KeyForDecryptCodeDebtSettings { Path = "C:\\PayHubKeys\\keysForDecryptCodeFnDebitos\\private_key.pem" });
        _rsaDecryptorUtil = new RsaDecryptorUtil(_privateKey);
    }

    [Fact]
    public void Decrypt_ReturnsDecryptedData()
    {
        // Arrange
        var encryptedData = "IpJaCqNo82xGnepqp9sHzaPrHHV+Dle+xBOVON8tHfW1QRKdDrZpcSni+J8uUD+dnnqYaKLRH3hzT8yKuApyUKqSEgVxXSL3t26nFMDjm3qRZ2SuFFBsJUkbTCiLJBuG1IDfnwNfni26Ga0p4Gb6dJYT1FyIfRyo9gTZcqGKn+0="; // This should be a valid encrypted string
        var expectedDecryptedData = 5052295;

        // Act
        var result = _rsaDecryptorUtil.Decrypt(encryptedData);

        // Assert
        Assert.Equal(expectedDecryptedData, result);
    }

    [Fact]
    public void Decrypt_ThrowsDecryptException_WhenDecryptionFails()
    {
        // Arrange
        var encryptedData = "invalid";

        // Act & Assert
        Assert.Throws<DecryptException>(() => _rsaDecryptorUtil.Decrypt(encryptedData));
    }

    [Fact]
    public void Decrypt_ReturnsDecryptedData_WhenEncryptedDataIsDifferent()
    {
        // Arrange
        var encryptedData = "QzAS4R1wn2EMwIPuLW8GwRxlR8mg4Gsx8TGk5R8aqKpw/J6+CxB2/zm02lsXlkTq5SQJT9ZQY2XenTikNUE0TJKUOmrd7n/cy3XgzW915heLnXoGWOgkYmbq7MKsv74vhwfTu7s+1cyx0X49UKUgZg17ge8LjtaWAPozJOCoTkA="; // This should be a valid encrypted string
        var expectedDecryptedData = 5082659;

        // Act
        var result = _rsaDecryptorUtil.Decrypt(encryptedData);

        // Assert
        Assert.Equal(expectedDecryptedData, result);
    }
}
