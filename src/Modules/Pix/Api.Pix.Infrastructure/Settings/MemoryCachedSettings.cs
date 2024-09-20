using System.ComponentModel.DataAnnotations;

namespace Api.Pix.Infrastructure.Settings;
public class MemoryCachedSettings
{
    [Required]
    public string Key { get; set; }
}
