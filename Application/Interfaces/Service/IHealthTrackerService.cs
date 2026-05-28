using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces.Service
{
    public interface IHealthTrackerService
    {
        Task<HealthLogResponse> SaveAsync(int userId, SaveHealthLogRequest request, CancellationToken ct = default);
        Task<IEnumerable<HealthLogResponse>> GetHistoryAsync(int userId, CancellationToken ct = default);
        Task<HealthProfileResponse?> GetProfileAsync(int userId, CancellationToken ct = default);
        Task<HealthStatsResponse> GetStatsAsync(int userId, CancellationToken ct = default);
    }
}