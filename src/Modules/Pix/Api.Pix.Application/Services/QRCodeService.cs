using Api.Pix.Application.Dtos;
using Api.Pix.Application.Interfaces.HttpClients;
using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Application.Interfaces.Services;
using Api.Pix.Domain.Interfaces;
using Api.Pix.Domain.Interfaces.Utils;
using Api.Pix.Domain.Models;
using Api.Pix.Domain.Models.Payloads;
using Api.Pix.Domain.Models.PixObjects;
using Api.Pix.Domain.Models.Responses;
using AutoMapper;
using CrossCutting.PayHub.Shared.Constants;
using CrossCutting.PayHub.Shared.Enums;
using CrossCutting.PayHub.Shared.Exceptions;
using CrossCutting.PayHub.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;

namespace Api.Pix.Application.Services;

public class QRCodeService : IQRCodeService
{
    private readonly ILogger<QRCodeService> _logger;
    private readonly IDebtRepository _debitoRepository;
    private readonly IPixControlRepository _pixControleRepository;
    private readonly IItauPixClient _itauPixClient;
    private readonly IRsaDecryptorUtil _rsaDecryptor;
    private readonly IMapper _mapper;
    private readonly IAccountsBankRepository _accountsBankRepository;
    private readonly ICertificateService _certificateService;

    public QRCodeService(
        ILogger<QRCodeService> logger,
        IDebtRepository debitoRepository,
        IPixControlRepository pixControleRepository,
        IItauPixClient itauPixClient,
        IRsaDecryptorUtil rsaDecryptor,
        IMapper mapper,
        IAccountsBankRepository accountsBankRepository,

        ICertificateService certificateService)
    {
        _logger = logger;
        _debitoRepository = debitoRepository;
        _pixControleRepository = pixControleRepository;
        _itauPixClient = itauPixClient;
        _rsaDecryptor = rsaDecryptor;
        _mapper = mapper;
        _accountsBankRepository = accountsBankRepository;
        _certificateService = certificateService;
    }



    public async Task<ApiResponseDto<PixDto>> CreatePixAndTxIdAsync(PayloadPixDto payloadPixDto)
    {
        try
        {
            _logger.LogInformation(LoggerConstants.StartService, nameof(QRCodeService), nameof(CreatePixAndTxIdAsync));

            var debtCode = _rsaDecryptor.Decrypt(payloadPixDto.Data);

            var debtEntity = await _debitoRepository.GetDebitosByCodeAsync(debtCode);

            if (debtEntity is null || debtEntity.Code == 0)
                throw new NotFoundException(ErrorsConstants.DebitNotFound);

            var pixControleEntity = await _pixControleRepository.GetPixControlByDebtIdAndActiveAsync(debtCode);

            if (pixControleEntity is null 
                || VerifyExpireTimeTransaction(pixControleEntity.ExpirationTime, pixControleEntity.DateTransaction)
                || CheckCurrentDebtAmount(debtEntity!.DebtValue, pixControleEntity.Amount))
            {
                var accountBankEntity = await _accountsBankRepository.GetPixKeyByCode(debtEntity.AccountBankCode);

                ValidateAccountBank(accountBankEntity);

                var jsonOutput = FactoryPostAndPutQRCodeImmediateRequestContent(debtEntity!.DebtValue, accountBankEntity.PixKey);

                var (crtPath, keyPath) = _certificateService.GetCertificatePaths(accountBankEntity.CNPJ);

                var responseObj = await _itauPixClient.CreatePixAndTxIdAsync(debtCode, crtPath, keyPath, jsonOutput, accountBankEntity);

                var jsonInput = await responseObj.Content.ReadAsStringAsync();

                var pixResponse = JsonSerializer.Deserialize<PixResponse>(jsonInput);

                var pixControl = GeneratePixControlForCreatePixAndTxId(pixResponse, debtCode, jsonInput, jsonOutput);

                var mappedPixResponse = _mapper.Map<PixDto>(pixResponse);

                await _pixControleRepository.InsertAsync(pixControl);

                _logger.LogInformation(LoggerConstants.EndService, nameof(QRCodeService), nameof(CreatePixAndTxIdAsync));

                return GenerateApiResponseDto(mappedPixResponse, debtEntity.DebtDescription);
            }

            _logger.LogInformation(LoggerConstants.EndService, nameof(QRCodeService), nameof(CreatePixAndTxIdAsync));

            var mappedPixControl = _mapper.Map<PixDto>(pixControleEntity);

            mappedPixControl.Amount = Formater.FormatDebtAmout(pixControleEntity.Amount);

            return GenerateApiResponseDto(mappedPixControl, debtEntity.DebtDescription);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggerConstants.ErrorLog, nameof(QRCodeService), nameof(CreatePixAndTxIdAsync), ex.Message);

            throw;
        }
    }

    private void ValidateAccountBank(AccountsBankModel? accountBankEntity)
    {
        if (accountBankEntity is null)
            throw new NotFoundException(ErrorsConstants.AccountBankNotFound);

        if (string.IsNullOrEmpty(accountBankEntity.PixKey))
            throw new NotFoundException(ErrorsConstants.PixKeyNotFound);

        if (string.IsNullOrEmpty(accountBankEntity.ClientId))
            throw new NotFoundException(ErrorsConstants.ClientIdNotFound);

        if (string.IsNullOrEmpty(accountBankEntity.ClientSecret))
            throw new NotFoundException(ErrorsConstants.ClientSecretNotFound);

    }

    private static ApiResponseDto<PixDto> GenerateApiResponseDto(PixDto pix, string debtDescription)
    {
        return new ApiResponseDto<PixDto>
        {
            Data = new PixDto
            {
                Amount = pix.Amount,
                Creation = pix.Creation,
                PixCopyAndPaste = pix.PixCopyAndPaste,
                Expiration = pix.Expiration,
                DescriptionDebt = debtDescription
            }
        };
    }

    private static PixControlModel GeneratePixControlForCreatePixAndTxId(PixResponse qrCodeImediatoEntity, int debtCode, string jsonInput, string jsonOutput)
    {
        var pixControlModel = new PixControlModel
        {
            SourceSystem = OrigemSystemConstants.PortalAluno,
            SourceSystemTable = OrigemSystemConstants.FnDebtsTable,
            OriginSystemIdentifierCode = debtCode,
            DictKey = qrCodeImediatoEntity!.Key,
            Amount = Convert.ToDecimal(qrCodeImediatoEntity!.Amount.Original, CultureInfo.InvariantCulture),
            TransactionId = qrCodeImediatoEntity.Txid,
            DateTransaction = qrCodeImediatoEntity!.Calendary.Creation,
            PixCopyAndPaste = qrCodeImediatoEntity.PixCopyAndPaste,
            ExpirationTime = Convert.ToInt32(qrCodeImediatoEntity.Calendary.Expiration),
            JsonOutput = jsonOutput,
            JsonInput = jsonInput,
            CodePixStatusProcessing = (int)Enum.Parse(typeof(StatusPixTransaction), qrCodeImediatoEntity.Status),
            DateTimeRegistration = DateTime.Now
        };

        return pixControlModel;
    }

    private static bool VerifyExpireTimeTransaction(long expireTimeSeconds, DateTime referenceTime)
    {
        DateTime now = DateTime.Now;

        long timeDiference = (long)(now - referenceTime).TotalSeconds;

        return timeDiference > expireTimeSeconds;
    }

    private static bool CheckCurrentDebtAmount(decimal currentDebtAmount, decimal pixAmountPrevious)
    {
        return currentDebtAmount > pixAmountPrevious;
    }


    private string FactoryPostAndPutQRCodeImmediateRequestContent(decimal amount, string pixKey)
    {
        var amountEntity = new AmountQRCodeImmediate { Original = Formater.FormatValue(amount) };
        var data = new QRCodeImmediatePayload { Amount = amountEntity, Key = pixKey };
        var json = JsonSerializer.Serialize(data);

        return json;
    }
}
