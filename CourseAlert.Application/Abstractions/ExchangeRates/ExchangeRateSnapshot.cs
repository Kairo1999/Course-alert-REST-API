namespace CourseAlert.Application.Abstractions.ExchangeRates;

public sealed record ExchangeRateSnapshot(
    string BaseCurrency,
    DateOnly Date,
    IReadOnlyDictionary<string, decimal> Rates);
