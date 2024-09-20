using Api.Pix.Domain.Interfaces;
using Api.Pix.Infrastructure.Settings;
using CrossCutting.PayHub.Shared.Constants;
using Microsoft.Extensions.Options;

namespace Api.Pix.Infrastructure.Services;

public class CertificateService(IOptions<PixSettings> pixSettings) : ICertificateService
{
    private readonly PixSettings _pixSettings = pixSettings.Value;


    public (string crtPath, string keyPath) GetCertificatePaths(string cnpj)
    {
        string directoryPath = _pixSettings.CertificateCrtPath; //Pasta que ficará os certificados

        string crtFile = $"{cnpj}.crt";
        string privateKeyFile = $"{cnpj}.key";

        return (GetFileAsync(directoryPath, crtFile), GetFileAsync(directoryPath, privateKeyFile));
    }


    static string GetFileAsync(string directoryPath, string fileNamePattern)
    {
        string[] files = Directory.GetFiles(directoryPath, fileNamePattern, SearchOption.TopDirectoryOnly);
        return files.Length > 0 ? files[0] : throw new FileNotFoundException(ErrorsConstants.ItauFilesNotFound);
    }
}