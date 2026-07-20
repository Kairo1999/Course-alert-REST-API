using CourseAlert.Application.Abstractions.ExchangeRates;
using CourseAlert.Application.Abstractions.Messaging;
using CourseAlert.Application.Abstractions.Persistence;
using CourseAlert.Application.Common.Exceptions;
using CourseAlert.Domain.Entities;

namespace CourseAlert.Application.Alerts.CreateAlert;

public sealed class CreateAlertCommandHandler
    : ICommandHandler<CreateAlertCommand, Guid>
{
    private readonly IAlertRepository _alertRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public CreateAlertCommandHandler(
        IAlertRepository alertRepository,
        IExchangeRateProvider exchangeRateProvider)
    {
        _alertRepository = alertRepository;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<Guid> Handle(
        CreateAlertCommand command,
        CancellationToken cancellationToken)
    {
        var alert = new Alert(
            command.BaseCurrency,
            command.TargetCurrency,
            command.Threshold,
            command.Direction);

        var supportedCurrencies =
            await _exchangeRateProvider.GetSupportedCurrenciesAsync(
                cancellationToken);

        if (!supportedCurrencies.Contains(alert.BaseCurrency))
        {
            throw new ValidationException(
                $"Currency '{alert.BaseCurrency}' is not supported.");
        }

        if (!supportedCurrencies.Contains(alert.TargetCurrency))
        {
            throw new ValidationException(
                $"Currency '{alert.TargetCurrency}' is not supported.");
        }

        await _alertRepository.AddAsync(alert, cancellationToken);

        return alert.Id;
    }
}