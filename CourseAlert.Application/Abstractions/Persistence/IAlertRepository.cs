using CourseAlert.Domain.Entities;

namespace CourseAlert.Application.Abstractions.Persistence;

public interface IAlertRepository
{
    Task AddAsync(
        Alert alert,
        CancellationToken cancellationToken);

    Task<Alert?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Alert>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);
}
