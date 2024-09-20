namespace Api.Pix.Application.Dtos;
public class PayloadPixDto
{
    public string Data { get; set; }

    public PayloadPixDto(string data)
    {
        Data = data;
    }

}
