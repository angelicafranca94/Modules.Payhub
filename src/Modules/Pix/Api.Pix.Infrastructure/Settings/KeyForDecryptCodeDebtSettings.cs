using System.ComponentModel.DataAnnotations;

namespace Api.Pix.Infrastructure.Settings;
public class KeyForDecryptCodeDebtSettings
{
    [Required]
    public string Path { get; set; }
}
