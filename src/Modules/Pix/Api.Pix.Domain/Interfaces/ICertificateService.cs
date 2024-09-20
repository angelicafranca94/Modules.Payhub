namespace Api.Pix.Domain.Interfaces;
public interface ICertificateService
{
    (string crtPath, string keyPath) GetCertificatePaths(string cnpj);



}
