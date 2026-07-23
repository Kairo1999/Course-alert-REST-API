using CourseAlert.Application.Abstractions.Messaging;

namespace CourseAlert.Application.Alerts.EvaluateAlert;

public sealed record EvaluateAlertQuery(Guid Id)
    : IQuery<EvaluateAlertResult>;
