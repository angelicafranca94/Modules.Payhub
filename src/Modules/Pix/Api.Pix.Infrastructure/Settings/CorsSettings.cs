using System.ComponentModel.DataAnnotations;

namespace Api.Pix.Infrastructure.Settings;
public class CorsSettings
{
    [Required]
    public required string[] Urls { get; set; }
}
