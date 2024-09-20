using System.ComponentModel.DataAnnotations;

namespace Api.Pix.Infrastructure.Settings;
public class PixUriSettings
{
    [Required]
    public string BaseUrl { get; set; }

    [Required]
    public string QrCodeImediatoPath { get; set; }

    [Required]
    public string QrCodeComVencimentoPath { get; set; }

    [Required]
    public string LoteQrComVencimentoPath { get; set; }

    [Required]
    public string LocationPath { get; set; }

    [Required]
    public string PixReciboDevolucaoPath { get; set; }

    [Required]
    public string WebhookPath { get; set; }
}
