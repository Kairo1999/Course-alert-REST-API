using CourseAlert.Application.Abstractions.ExchangeRates;
using CourseAlert.Application.Abstractions.Messaging;

namespace CourseAlert.Application.Rates.GetLatestRates;

public sealed record GetLatestRatesQuery(
    string BaseCurrency,
    IReadOnlyCollection<string> Symbols)
    : IQuery<ExchangeRateSnapshot>;
