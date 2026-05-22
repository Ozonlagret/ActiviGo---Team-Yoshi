using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;

namespace Application.Services
{
    public class HealthTrackerService : IHealthTrackerService
    {
        private readonly IHealthTrackerRepository _repository;

        public HealthTrackerService(IHealthTrackerRepository repository)
        {
            _repository = repository;
        }

        public async Task<HealthLogResponse> SaveAsync(int userId, SaveHealthLogRequest request, CancellationToken ct = default)
        {
            if (request.Steps is null && request.Calories is null && request.WeightKg is null && request.HeightCm is null)
                throw new ArgumentException("At least one field must be provided.", nameof(request));

            var logDate = request.LogDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var existing = await _repository.GetByUserAndDateAsync(userId, logDate, ct);
            var now = DateTime.UtcNow;

            if (existing is null)
            {
                existing = new HealthLog
                {
                    UserId = userId,
                    LogDate = logDate,
                    Steps = request.Steps,
                    Calories = request.Calories,
                    WeightKg = request.WeightKg,
                    HeightCm = request.HeightCm,
                    CreatedAtUtc = now,
                    UpdatedAtUtc = now
                };

                await _repository.AddAsync(existing, ct);
            }
            else
            {
                existing.Steps = request.Steps ?? existing.Steps;
                existing.Calories = request.Calories ?? existing.Calories;
                existing.WeightKg = request.WeightKg ?? existing.WeightKg;
                existing.HeightCm = request.HeightCm ?? existing.HeightCm;
                existing.UpdatedAtUtc = now;
            }

            await _repository.SaveChangesAsync(ct);
            return Map(existing);
        }

        public async Task<IEnumerable<HealthLogResponse>> GetHistoryAsync(int userId, CancellationToken ct = default)
        {
            var entries = await _repository.GetByUserAsync(userId, ct);
            return entries
                .OrderByDescending(x => x.LogDate)
                .Select(Map)
                .ToList();
        }

        public async Task<HealthProfileResponse?> GetProfileAsync(int userId, CancellationToken ct = default)
        {
            var latest = await _repository.GetLatestProfileSnapshotAsync(userId, ct);
            return latest is null
                ? null
                : new HealthProfileResponse(latest.WeightKg, latest.HeightCm, latest.LogDate, latest.UpdatedAtUtc);
        }

        private static HealthLogResponse Map(HealthLog log) => new(
            log.Id,
            log.LogDate,
            log.Steps,
            log.Calories,
            log.WeightKg,
            log.HeightCm,
            log.CreatedAtUtc,
            log.UpdatedAtUtc);
    }
}