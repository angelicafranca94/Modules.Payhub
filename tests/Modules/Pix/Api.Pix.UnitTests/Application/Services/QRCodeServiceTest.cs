using CrossCutting.PayHub.Shared.Constants;
using Api.Pix.Application.Dtos;
using Api.Pix.Application.Interfaces.HttpClients;
using Api.Pix.Application.Interfaces.Repositories;
using Api.Pix.Application.Services;
using Api.Pix.Domain.Interfaces.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;
using Xunit;
using Api.Pix.Domain.Models.Responses;
using Api.Pix.Domain.Models;
using Api.Pix.Domain.Models.PixObjects;
using Api.Pix.Domain.Models.Payloads;
using AutoMapper;
using CrossCutting.PayHub.Shared.Exceptions;
using Api.Pix.Application.AutoMapper;
using Api.Pix.Infrastructure.Repositories;
using Api.Pix.Domain.Interfaces;

namespace Api.Pix.UnitTests.Application.Services;
public class QRCodeServiceTest
{
    private readonly QRCodeService _qRCodeImmediateService;
    private readonly ILogger<QRCodeService> _logger = Substitute.For<ILogger<QRCodeService>>();
    private readonly IDebtRepository _debitoRepositoryMock = Substitute.For<IDebtRepository>();
    private readonly IPixControlRepository _pixControleRepositoryMock = Substitute.For<IPixControlRepository>();
    private readonly IItauPixClient _itauPixClientMock = Substitute.For<IItauPixClient>();
    private readonly IRsaDecryptorUtil _rsaDecryptorMock = Substitute.For<IRsaDecryptorUtil>();
    private readonly IAccountsBankRepository _accountsBankRepository = Substitute.For<IAccountsBankRepository>();
    private readonly ICertificateService _certificateService = Substitute.For<ICertificateService>();
    private readonly IMapper _mapper = new Mapper(
           new MapperConfiguration(config => config.AddProfiles(
               new List<Profile>
               {
                    new PixMap(),
                  //  new WebhookItauBolecodePixMap()
               }
           ))
       );

    public QRCodeServiceTest()
    {
        this._qRCodeImmediateService = new QRCodeService(_logger,
            _debitoRepositoryMock, _pixControleRepositoryMock, _itauPixClientMock, _rsaDecryptorMock, _mapper, _accountsBankRepository, _certificateService);
    }

    [Fact]
    public async Task CreatePixAndTxIdAsync_ShouldReturnApiResponseDto_WhenPayloadIsValid()
    {
        //Arrange
        var payloadPixDto = GeneratePayloadDto();
        var apiResponseDto = GenerateApiResponseDto();
        var debtEntity = GenerateDebtEntity();
        var debtCode = 1;
        var jsonOutput = GenerateJsonOutput();
        var qrCodeImediatoEntity = GenerateResponsePostQRCodeImmediateEntity();
        var responseObj = new HttpResponseMessage { Content = new StringContent(JsonSerializer.Serialize(qrCodeImediatoEntity)) };
        var pixControlEntity = GeneratePixControlForCreatePixAndTxId();


        _rsaDecryptorMock.Decrypt(Arg.Any<string>()).Returns(debtCode);

        _debitoRepositoryMock.GetDebitosByCodeAsync(Arg.Any<int>()).Returns(debtEntity);

        _accountsBankRepository.GetPixKeyByCode(Arg.Any<int>()).Returns(new AccountsBankModel { ClientId = "1", ClientSecret = "2", CNPJ="3", Code=4, PixKey="5"});

      ///  _itauPixClientMock.FactoryPostAndPutQRCodeImmediateRequestContent(Arg.Any<decimal>(), Arg.Any<string>()).Returns(jsonOutput);

        _itauPixClientMock.CreatePixAndTxIdAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<AccountsBankModel>()).Returns(responseObj);

        _pixControleRepositoryMock.GetPixControlByTxIdAsync(Arg.Any<string>()).Returns(pixControlEntity);

        await _pixControleRepositoryMock.InsertAsync(pixControlEntity);

        _mapper.Map<PixDto>(qrCodeImediatoEntity);

        //Act
        var result = await _qRCodeImmediateService.CreatePixAndTxIdAsync(payloadPixDto);

        //Assert
        result.Should().BeEquivalentTo(apiResponseDto);
    }

    [Fact]
    public async Task CreatePixAndTxIdAsync_ThrowsNotFoundException_WhenDebitNotFound()
    {
        // Arrange
        var payloadPixDto = GeneratePayloadDto();
        var debtCode = 1;

        _rsaDecryptorMock.Decrypt(payloadPixDto.Data).Returns(debtCode);

        _debitoRepositoryMock.GetDebitosByCodeAsync(Arg.Any<int>()).Returns(Task.FromResult<FNDebtsModel>(null));

        // Act and Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _qRCodeImmediateService.CreatePixAndTxIdAsync(payloadPixDto));
        Assert.Equal(ErrorsConstants.DebitNotFound, exception.Message);
    }


    [Fact]
    public async Task CreatePixAndTxIdAsync_ThrowsNotFoundException_WhenAccountBankNotFound()
    {
        // Arrange
        var payloadPixDto = GeneratePayloadDto();
        var debtCode = 1;
        var debtEntity = GenerateDebtEntity();

        _rsaDecryptorMock.Decrypt(payloadPixDto.Data).Returns(debtCode);

        _debitoRepositoryMock.GetDebitosByCodeAsync(Arg.Any<int>()).Returns(debtEntity);

        _accountsBankRepository.GetPixKeyByCode(Arg.Any<int>()).Returns(Task.FromResult<AccountsBankModel>(null));

        // Act and Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _qRCodeImmediateService.CreatePixAndTxIdAsync(payloadPixDto));
        Assert.Equal(ErrorsConstants.AccountBankNotFound, exception.Message);
    }

    [Fact]
    public async Task CreatePixAndTxIdAsync_ThrowsNotFoundException_WhenPixKeyIsNullOrEmpty()
    {
        // Arrange
        var payloadPixDto = GeneratePayloadDto();
        var debtCode = 1;
        var debtEntity = GenerateDebtEntity();
        var accout = GenerateAccountsBank();
        accout.PixKey = string.Empty;

        _rsaDecryptorMock.Decrypt(payloadPixDto.Data).Returns(debtCode);

        _debitoRepositoryMock.GetDebitosByCodeAsync(Arg.Any<int>()).Returns(debtEntity);

        _accountsBankRepository.GetPixKeyByCode(Arg.Any<int>()).Returns(accout);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _qRCodeImmediateService.CreatePixAndTxIdAsync(payloadPixDto));
        Assert.Equal(ErrorsConstants.PixKeyNotFound, exception.Message);
    }

    [Fact]
    public async Task CreatePixAndTxIdAsync_ThrowsNotFoundException_WhenClientIdIsNullOrEmpty()
    {
        // Arrange
        var payloadPixDto = GeneratePayloadDto();
        var debtCode = 1;
        var debtEntity = GenerateDebtEntity();
        var accout = GenerateAccountsBank();
        accout.ClientId = string.Empty;

        _rsaDecryptorMock.Decrypt(payloadPixDto.Data).Returns(debtCode);

        _debitoRepositoryMock.GetDebitosByCodeAsync(Arg.Any<int>()).Returns(debtEntity);

        _accountsBankRepository.GetPixKeyByCode(Arg.Any<int>()).Returns(accout);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _qRCodeImmediateService.CreatePixAndTxIdAsync(payloadPixDto));
        Assert.Equal(ErrorsConstants.ClientIdNotFound, exception.Message);
    }

    [Fact]
    public async Task CreatePixAndTxIdAsync_ThrowsNotFoundException_WhenClientSecretIsNullOrEmpty()
    {
        // Arrange
        var payloadPixDto = GeneratePayloadDto();
        var debtCode = 1;
        var debtEntity = GenerateDebtEntity();
        var accout = GenerateAccountsBank();
        accout.ClientSecret = string.Empty;

        _rsaDecryptorMock.Decrypt(payloadPixDto.Data).Returns(debtCode);

        _debitoRepositoryMock.GetDebitosByCodeAsync(Arg.Any<int>()).Returns(debtEntity);

        _accountsBankRepository.GetPixKeyByCode(Arg.Any<int>()).Returns(accout);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _qRCodeImmediateService.CreatePixAndTxIdAsync(payloadPixDto));
        Assert.Equal(ErrorsConstants.ClientSecretNotFound, exception.Message);
    }

    private static string GenerateJsonOutput()
    {
        var amountEntity = new AmountQRCodeImmediate { Original = "10.00" };
        var data = new QRCodeImmediatePayload { Amount = amountEntity, Key = "12345" };
        var json = JsonSerializer.Serialize(data);

        return json;
    }

    private static PixResponse GenerateResponsePostQRCodeImmediateEntity() => new()
    {
        Amount = new AmountQRCodeImmediate { Original = "100.00" },
        Txid = "1232131",
        PixCopyAndPaste = "pixCopiaECola",
        Calendary = new Calendary { Expiration = "3600", Creation = Convert.ToDateTime("2022-12-31 21:00:00") },
        Status = "ATIVA"
    };

    private static FNDebtsModel GenerateDebtEntity() => new()
    {
        Code = 1,
        DebtValue = 10.00m
    };

    private static PayloadPixDto GeneratePayloadDto() => new("IpJaCqNo82xGnepqp9sHzaPrHHV+Dle+xBOVON8tHfW1QRKdDrZpcSni+J8uUD+dnnqYaKLRH3hzT8yKuApyUKqSEgVxXSL3t26nFMDjm3qRZ2SuFFBsJUkbTCiLJBuG1IDfnwNfni26Ga0p4Gb6dJYT1FyIfRyo9gTZcqGKn+0=") {};

    private static ApiResponseDto<PixDto> GenerateApiResponseDto() => new()
    {
        Data = new PixDto
        {
            Amount = "100.00",
            Creation = Convert.ToDateTime("2022-12-31 21:00:00"),
            Expiration = 3600,
            PixCopyAndPaste = "pixCopiaECola"
        },
        Message = "Success"
    };

    private static PixControlModel GeneratePixControlForCreatePixAndTxId() => new()
    {
        SourceSystem = OrigemSystemConstants.Vestibular,
        SourceSystemTable = OrigemSystemConstants.FnDebtsTable,
        OriginSystemIdentifierCode = 1,
        DictKey = string.Empty,
        Amount = 10.00m,
        TransactionId = string.Empty,
        DateTransaction = DateTime.Now,
        PixCopyAndPaste = string.Empty,
        ExpirationTime = 34600,
        JsonInput = string.Empty,
        JsonOutput = string.Empty,
        CodePixStatusProcessing = 1
    };

    private static AccountsBankModel GenerateAccountsBank() => new()
    {
        ClientId = "sfdsfds",
        ClientSecret = "dfgdfg",
        CNPJ = "11111",
        PixKey= "ewertetert",
        Code = 1

    };
}
