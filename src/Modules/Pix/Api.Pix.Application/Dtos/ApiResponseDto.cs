namespace Api.Pix.Application.Dtos;
public class ApiResponseDto<T> where T : class
{
    public string Message { get; set; } = "Success";
    public T Data { get; set; }
}
