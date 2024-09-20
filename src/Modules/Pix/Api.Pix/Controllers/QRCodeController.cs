using Api.Pix.Application.Dtos;
using Api.Pix.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pix.Controllers;

[ApiController]
[Route("[controller]")]
public class QRCodeController : ControllerBase
{
    private readonly IQRCodeService _qRCodeImediatoService;

    public QRCodeController(IQRCodeService qRCodeImediatoService)
    {
        _qRCodeImediatoService = qRCodeImediatoService;
    }

    /// <summary>
    /// API para emitir um QR Codigo imediato em que o Itaú é responsável por criar o identificador do QR Codigo (txid).
    /// </summary>
    /// <param name="payloadDto"> Objeto que contém o id do débito criptografado </param>
    /// <returns></returns>

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PayloadPixDto payloadDto)
    {
        return Ok(await _qRCodeImediatoService.CreatePixAndTxIdAsync(payloadDto));
    }

    /// <summary>
    /// Endpoint responsável por "ouvir" as notificações de pagamento enviadas pelo Itau
    /// </summary>
    /// <param name="itauWebhookDto">Dados que o itaú envia com informações do pagamento</param>
    /// <returns></returns>
    [HttpGet("hello")]
    public async Task<IActionResult> Get()
    {
        return Ok("A API ESTÁ FUNCIONANDO - CONTROLLER QRCodeController");
    }
}
