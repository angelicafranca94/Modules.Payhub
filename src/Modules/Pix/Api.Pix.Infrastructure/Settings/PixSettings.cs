using System.ComponentModel.DataAnnotations;

namespace Api.Pix.Infrastructure.Settings;
public class PixSettings
{
    public bool UseSandbox { get; set; }

    [Required]
    public string GrantType { get; set; }

    public string CertificateCrtPath { get; set; }
}
