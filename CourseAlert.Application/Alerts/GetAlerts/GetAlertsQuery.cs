using CourseAlert.Application.Abstractions.Messaging;
using CourseAlert.Application.Alerts.Common;

namespace CourseAlert.Application.Alerts.GetAlerts;

public sealed record GetAlertsQuery()
    : IQuery<IReadOnlyList<AlertDto>>;
