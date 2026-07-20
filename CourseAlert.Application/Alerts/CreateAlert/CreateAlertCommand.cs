using CourseAlert.Application.Abstractions.Messaging;
using CourseAlert.Domain.Enums;

namespace CourseAlert.Application.Alerts.CreateAlert
{
    public sealed record CreateAlertCommand(
        string BaseCurrency,
        string TargetCurrency,
        decimal Threshold,
        AlertDirection Direction) : ICommand<Guid>;
}
