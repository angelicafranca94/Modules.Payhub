using System.ComponentModel.DataAnnotations;

namespace Api.Pix.Infrastructure.Settings;
public class GenerateTokenPixSettings
{
    [Required]
    public string BaseUrl { get; set; }

    [Required]
    public string Path { get; set; }
}
