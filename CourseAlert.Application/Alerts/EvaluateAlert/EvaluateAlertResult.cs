using CourseAlert.Domain.Enums;

namespace CourseAlert.Application.Alerts.EvaluateAlert;

public sealed record EvaluateAlertResult(
    Guid AlertId,
    string BaseCurrency,
    string TargetCurrency,
    decimal Threshold,
    AlertDirection Direction,
    decimal CurrentRate,
    bool IsTriggered,
    DateOnly RateDate);
