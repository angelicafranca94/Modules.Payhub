using System.ComponentModel.DataAnnotations;

namespace Consumers.Service.Settings;
public class RabbitMqSettings
{
    [Required]
    public string Host { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string User { get; set; }

    [Required]
    public string Pass { get; set; }

    [Required]
    public string VHost { get; set; }
}
