namespace Api.Pix.Application.Dtos;
public class PixDto
{
    public string PixCopyAndPaste { get; set; }

    public DateTime Creation { get; set; }

    public int Expiration { get; set; }

    public string Amount { get; set; }

    public string? DescriptionDebt { get; set; }

}