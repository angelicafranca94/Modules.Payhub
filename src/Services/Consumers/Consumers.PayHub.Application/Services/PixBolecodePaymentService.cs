using Consumers.PayHub.Application.Interfaces.Repositories;
using Consumers.PayHub.Application.Interfaces.Services;
using Consumers.PayHub.Domain.Models;
using CrossCutting.PayHub.Shared.Constants;
using CrossCutting.PayHub.Shared.Dtos.Messages;
using CrossCutting.PayHub.Shared.Enums;
using CrossCutting.PayHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Consumers.PayHub.Application.Services;

public class PixBolecodePaymentService : IPixBolecodePaymentService
{
    private readonly ILogger<PixBolecodePaymentService> _logger;
    private readonly IPixControlRepository _pixControleRepository;
    private readonly IDebtRepository _debtRepository;
    private readonly IBMRemessaRepository _bmRemessaRepository;
    private readonly IBOControlRepository _boControlRepository;
    private readonly IWebhookItauBolecodePixRepository _webhookItauBolecodePixRepository;

    public PixBolecodePaymentService(ILogger<PixBolecodePaymentService> logger,
        IPixControlRepository pixControleRepository,
        IDebtRepository debtRepository,
        IBMRemessaRepository bmRemessaRepository,
        IBOControlRepository boControlRepository,
        IWebhookItauBolecodePixRepository webhookItauPaymentJsonRepository
        )
    {
        _logger = logger;
        _pixControleRepository = pixControleRepository;
        _debtRepository = debtRepository;
        _bmRemessaRepository = bmRemessaRepository;
        _boControlRepository = boControlRepository;
        _webhookItauBolecodePixRepository = webhookItauPaymentJsonRepository;

    }

    public async Task DebtPaymentProcessAsync(ItauWebhookMessage itauWebhook)
    {

        try
        {
            _logger.LogInformation(LoggerConstants.EndService, nameof(PixBolecodePaymentService), nameof(DebtPaymentProcessAsync));

            int debtCode = 0;

            //se não começar com BL, é um pix qrcode
            if (!itauWebhook.Txid.StartsWith(OrigemSystemConstants.BolecodeAcronym))
            {
                _logger.LogInformation("Is a pix");
                var pixControleEntity = await _pixControleRepository.GetPixControlByTxIdAsync(itauWebhook.Txid);

                if (pixControleEntity is null)
                    throw new NotFoundException(ErrorsConstants.PixTransactionNotFound);

                debtCode = pixControleEntity.OriginSystemIdentifierCode;

                await UpdateDatasPaymentAsync(itauWebhook, debtCode, pixControleEntity);
            }
            else
            {

                _logger.LogInformation("Is a BOLECODE");

                _logger.LogInformation("Verifying BMRemessa...");
                var debtCodeRemessaRepo = await _bmRemessaRepository.GetDebtCodeByTxIdAsync(itauWebhook.Txid);

                debtCode = GetDebtCode(debtCodeRemessaRepo);

                if (debtCode == 0)
                {
                    _logger.LogInformation("Boleto in BMRemessa NOT FOUND");

                    var ourNumber = itauWebhook.Txid.Substring(16).Replace("0", "");
                    _logger.LogInformation("Nosso numero: {ourNumber}", ourNumber);

                    _logger.LogInformation("Verifying BOControle...");
                    var debtCodeBOControlRepo = await _boControlRepository.GetDebtCodeByOurNumberAsync(ourNumber);

                    debtCode = GetDebtCode(debtCodeBOControlRepo);

                    if (debtCode == 0)
                        throw new NotFoundException(ErrorsConstants.DebitNotFound);
                }

                await UpdateDatasPaymentAsync(itauWebhook, debtCode, null);
            }

            _logger.LogInformation(LoggerConstants.EndService, nameof(PixBolecodePaymentService), nameof(DebtPaymentProcessAsync));

        }
        catch (Exception ex)
        {
            _logger.LogError(LoggerConstants.ErrorLog, nameof(PixBolecodePaymentService), nameof(DebtPaymentProcessAsync), ex.Message);
            throw;
        }

    }

    private async Task UpdateDatasPaymentAsync(ItauWebhookMessage itauWebhook, int debtCode, PixControlModel? pixControleEntity)
    {
        _logger.LogInformation("Getting debt...");
        var fndebts = await _debtRepository.GetDebitosByCodeAsync(debtCode);

        if (fndebts is null)
            throw new NotFoundException(ErrorsConstants.DebitNotFound);

        _logger.LogInformation("Updating PixControle...");
        if (pixControleEntity is not null)
            await UpdatePixControleAsync(Convert.ToDateTime(itauWebhook.Timestamp), pixControleEntity);

        var webhookItauBolecodePix = await _webhookItauBolecodePixRepository.GetByTxIdAsync(itauWebhook.Txid);
        if (webhookItauBolecodePix is null)
            throw new NotFoundException(ErrorsConstants.PixTransactionNotFound);

        _logger.LogInformation("Updating FnDebitos...");
        await UpdateDebtAsync(Convert.ToDateTime(itauWebhook.Timestamp), fndebts, Convert.ToDecimal(itauWebhook.Amount), webhookItauBolecodePix.Code);

        _logger.LogInformation("Updating WebhookItauBolecodePix...");
        await UpdateControlWebhookAsync(itauWebhook.Txid);
    }

    private int GetDebtCode(int? debtCode)
    {
        return debtCode.HasValue ? debtCode.Value : 0;
    }

    private async Task UpdateDebtAsync(DateTime paymentDate, FNDebtsModel fndebts, decimal webhookItauBolecodePixAmount, int webhookItauBolecodePixCode)
    {
        fndebts.PaymentDate = paymentDate;
        fndebts.PaymentAmount = webhookItauBolecodePixAmount;
        fndebts.WebhookItauBolecodePixCode = webhookItauBolecodePixCode;

        await _debtRepository.PayDebtAsync(fndebts);
    }

    private async Task UpdatePixControleAsync(DateTime paymentDate, PixControlModel pixControleEntity)
    {
        pixControleEntity.DateTransaction = paymentDate;
        pixControleEntity.CodePixStatusProcessing = (int)StatusPixTransaction.CONCLUIDA;
        pixControleEntity.DateTimeUpdated = DateTime.Now;

        await _pixControleRepository.UpdateAsync(pixControleEntity);
    }

    private async Task UpdateControlWebhookAsync(string txid)
    {
        var webhook = await _webhookItauBolecodePixRepository.GetByTxIdAsync(txid);

        if (webhook is null)
            throw new NotFoundException(ErrorsConstants.WebhookJsonNotFound);

        webhook.RabbitMQQueueProcessed = true;
        webhook.DatetimeRabbitMQQueueProcessed = DateTime.Now.AddHours(-3);

        await _webhookItauBolecodePixRepository.UpdateAsync(webhook);
    }
}
