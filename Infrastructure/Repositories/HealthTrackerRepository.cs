using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HealthTrackerRepository : IHealthTrackerRepository
    {
        private readonly ActiviGoDbContext _db;

        public HealthTrackerRepository(ActiviGoDbContext db)
        {
            _db = db;
        }

        public Task<HealthLog?> GetByUserAndDateAsync(int userId, DateOnly logDate, CancellationToken ct = default)
            => _db.HealthLogs
                .FirstOrDefaultAsync(x => x.UserId == userId && x.LogDate == logDate, ct);

        public async Task<IEnumerable<HealthLog>> GetByUserAsync(int userId, CancellationToken ct = default)
            => await _db.HealthLogs
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);

        public Task<HealthLog?> GetLatestProfileSnapshotAsync(int userId, CancellationToken ct = default)
            => _db.HealthLogs
                .Where(x => x.UserId == userId && (x.WeightKg != null || x.HeightCm != null))
                .OrderByDescending(x => x.UpdatedAtUtc)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);

        public async Task AddAsync(HealthLog entry, CancellationToken ct = default)
        {
            await _db.HealthLogs.AddAsync(entry, ct);
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);
    }
}