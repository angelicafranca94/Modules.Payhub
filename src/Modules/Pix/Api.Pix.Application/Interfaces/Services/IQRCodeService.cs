using Api.Pix.Application.Dtos;

namespace Api.Pix.Application.Interfaces.Services;
public interface IQRCodeService
{
    Task<ApiResponseDto<PixDto>> CreatePixAndTxIdAsync(PayloadPixDto payloadPixDto);
}
