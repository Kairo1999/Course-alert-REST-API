using CourseAlert.Application.Abstractions.ExchangeRates;
using CourseAlert.Application.Abstractions.Messaging;
using CourseAlert.Application.Abstractions.Persistence;
using CourseAlert.Application.Common.Exceptions;

namespace CourseAlert.Application.Alerts.EvaluateAlert;

public sealed class EvaluateAlertQueryHandler
    : IQueryHandler<EvaluateAlertQuery, EvaluateAlertResult>
{
    private readonly IAlertRepository _alertRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public EvaluateAlertQueryHandler(
        IAlertRepository alertRepository,
        IExchangeRateProvider exchangeRateProvider)
    {
        _alertRepository = alertRepository;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<EvaluateAlertResult> Handle(
        EvaluateAlertQuery query,
        CancellationToken cancellationToken)
    {
        var alert = await _alertRepository.GetByIdAsync(
            query.Id,
            cancellationToken);

        if (alert is null)
        {
            throw new NotFoundException(
                $"Alert with ID '{query.Id}' was not found.");
        }

        var rates = await _exchangeRateProvider.GetLatestAsync(
            alert.BaseCurrency,
            new[] { alert.TargetCurrency },
            cancellationToken);

        if (!rates.Rates.TryGetValue(
                alert.TargetCurrency,
                out var currentRate))
        {
            throw new ExternalServiceException(
                $"The exchange rate for '{alert.TargetCurrency}' " +
                "was not returned by the provider.");
        }

        var isTriggered = alert.IsTriggered(currentRate);

        return new EvaluateAlertResult(
            alert.Id,
            alert.BaseCurrency,
            alert.TargetCurrency,
            alert.Threshold,
            alert.Direction,
            currentRate,
            isTriggered,
            rates.Date);
    }
}
