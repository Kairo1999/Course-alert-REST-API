using CourseAlert.Application.Abstractions.Messaging;

namespace CourseAlert.Application.Alerts.DeleteAlert;

public sealed record DeleteAlertCommand(Guid Id)
    : ICommand<bool>;
