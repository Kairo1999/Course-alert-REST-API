using CourseAlert.Domain.Entities;
using CourseAlert.Domain.Enums;

namespace CourseAlert.Application.Alerts.Common;

public sealed record AlertDto(
    Guid Id,
    string BaseCurrency,
    string TargetCurrency,
    decimal Threshold,
    AlertDirection Direction,
    DateTimeOffset CreatedAt)
{
    public static AlertDto FromEntity(Alert alert)
    {
        return new AlertDto(
            alert.Id,
            alert.BaseCurrency,
            alert.TargetCurrency,
            alert.Threshold,
            alert.Direction,
            alert.CreatedAt);
    }
}
