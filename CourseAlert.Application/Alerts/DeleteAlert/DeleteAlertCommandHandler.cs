using CourseAlert.Application.Abstractions.Messaging;
using CourseAlert.Application.Abstractions.Persistence;
using CourseAlert.Application.Common.Exceptions;

namespace CourseAlert.Application.Alerts.DeleteAlert;

public sealed class DeleteAlertCommandHandler
    : ICommandHandler<DeleteAlertCommand, bool>
{
    private readonly IAlertRepository _alertRepository;

    public DeleteAlertCommandHandler(IAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    public async Task<bool> Handle(
        DeleteAlertCommand command,
        CancellationToken cancellationToken)
    {
        var deleted = await _alertRepository.DeleteAsync(
            command.Id,
            cancellationToken);

        if (!deleted)
        {
            throw new NotFoundException(
                $"Alert with ID '{command.Id}' was not found.");
        }

        return true;
    }
}
