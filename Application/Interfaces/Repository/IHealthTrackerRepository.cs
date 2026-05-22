using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IHealthTrackerRepository
    {
        Task<HealthLog?> GetByUserAndDateAsync(int userId, DateOnly logDate, CancellationToken ct = default);
        Task<IEnumerable<HealthLog>> GetByUserAsync(int userId, CancellationToken ct = default);
        Task<HealthLog?> GetLatestProfileSnapshotAsync(int userId, CancellationToken ct = default);
        Task AddAsync(HealthLog entry, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}