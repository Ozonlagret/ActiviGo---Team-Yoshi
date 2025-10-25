using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IActivitySessionRepository
    {
        Task<ActivitySession?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ActivitySession?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<ActivitySession>> GetAllAsync(CancellationToken ct = default);
        //Task<IEnumerable<ActivitySession>> GetByActivityIdAsync(int activityId, CancellationToken ct = default);
        //Task<IEnumerable<ActivitySession>> GetByLocationIdAsync(int locationId, CancellationToken ct = default);

        // Sök/filtning
        Task<IEnumerable<ActivitySession>> FilterAvailableSessionsAsync(
                DateTime? startDate,
                DateTime? endDate,
                int? categoryId,
                bool? isIndoor,
                int? locationId,
                CancellationToken ct = default);

        Task<ActivitySession?> GetSessionWithBookingsAsync(int sessionId, CancellationToken ct = default);

        Task AddAsync(ActivitySession session, CancellationToken ct = default);
        Task UpdateAsync(ActivitySession session, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
