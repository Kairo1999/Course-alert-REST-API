using CourseAlert.Application.Abstractions.Messaging;
using CourseAlert.Application.Abstractions.Persistence;
using CourseAlert.Application.Alerts.Common;

namespace CourseAlert.Application.Alerts.GetAlerts;

public sealed class GetAlertsQueryHandler
    : IQueryHandler<GetAlertsQuery, IReadOnlyList<AlertDto>>
{
    private readonly IAlertRepository _alertRepository;

    public GetAlertsQueryHandler(IAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    public async Task<IReadOnlyList<AlertDto>> Handle(
        GetAlertsQuery query,
        CancellationToken cancellationToken)
    {
        var alerts =
            await _alertRepository.GetAllAsync(cancellationToken);

        return alerts
            .OrderByDescending(alert => alert.CreatedAt)
            .Select(alert => AlertDto.FromEntity(alert))
            .ToList();
    }
}
