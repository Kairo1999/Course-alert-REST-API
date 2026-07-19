namespace CourseAlert.Application.Abstractions.ExchangeRates;

public interface IExchangeRateProvider
{
    Task<ExchangeRateSnapshot> GetLatestAsync(
        string baseCurrency,
        IReadOnlyCollection<string> symbols,
        CancellationToken cancellationToken);

    Task<IReadOnlySet<string>> GetSupportedCurrenciesAsync(
        CancellationToken cancellationToken);
}
