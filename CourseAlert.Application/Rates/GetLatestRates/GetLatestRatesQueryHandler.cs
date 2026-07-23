using CourseAlert.Application.Abstractions.ExchangeRates;
using CourseAlert.Application.Abstractions.Messaging;
using CourseAlert.Application.Common.Exceptions;

namespace CourseAlert.Application.Rates.GetLatestRates;

public sealed class GetLatestRatesQueryHandler
    : IQueryHandler<GetLatestRatesQuery, ExchangeRateSnapshot>
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetLatestRatesQueryHandler(
        IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<ExchangeRateSnapshot> Handle(
        GetLatestRatesQuery query,
        CancellationToken cancellationToken)
    {
        var baseCurrency = NormalizeCurrency(query.BaseCurrency);

        if (query.Symbols is null || query.Symbols.Count == 0)
        {
            throw new ValidationException(
                "At least one target currency is required.");
        }

        var symbols = query.Symbols
            .Select(NormalizeCurrency)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var supportedCurrencies =
            await _exchangeRateProvider.GetSupportedCurrenciesAsync(
                cancellationToken);

        EnsureCurrencyIsSupported(
            baseCurrency,
            supportedCurrencies);

        foreach (var symbol in symbols)
        {
            EnsureCurrencyIsSupported(
                symbol,
                supportedCurrencies);
        }

        return await _exchangeRateProvider.GetLatestAsync(
            baseCurrency,
            symbols,
            cancellationToken);
    }

    private static string NormalizeCurrency(string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ValidationException(
                "Currency code is required.");
        }

        var normalizedCurrency =
            currency.Trim().ToUpperInvariant();

        if (normalizedCurrency.Length != 3 ||
            normalizedCurrency.Any(
                character => character is < 'A' or > 'Z'))
        {
            throw new ValidationException(
                "Currency code must contain exactly three letters.");
        }

        return normalizedCurrency;
    }

    private static void EnsureCurrencyIsSupported(
        string currency,
        IReadOnlySet<string> supportedCurrencies)
    {
        if (!supportedCurrencies.Contains(currency))
        {
            throw new ValidationException(
                $"Currency '{currency}' is not supported.");
        }
    }
}
