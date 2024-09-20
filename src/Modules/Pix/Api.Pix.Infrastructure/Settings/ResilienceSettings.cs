using System.ComponentModel.DataAnnotations;

namespace Api.Pix.Infrastructure.Settings;

public class ResilienceSettings
{
    [Required]
    public int CircuitBreakTime { get; set; }

    [Required]
    public int RetryTime { get; set; }
}
