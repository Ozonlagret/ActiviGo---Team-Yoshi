using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IActivitySessionRepository
    {
        Task<ActivitySession?> GetByIdAsync(int id);
        Task<ActivitySession?> GetByIdWithDetailsAsync(int id); 
        Task<IEnumerable<ActivitySession>> GetAllAsync();
        Task<IEnumerable<ActivitySession>> GetByActivityIdAsync(int activityId);
        Task<IEnumerable<ActivitySession>> GetByLocationIdAsync(int locationId);
        Task<IEnumerable<ActivitySession>> GetAvailableSessionsAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? categoryId,
            bool? isIndoor,
            int? locationId);
        Task<IEnumerable<ActivitySession>> GetUpcomingSessionsAsync();
        Task<int> GetAvailableSpotsAsync(int sessionId);
        Task AddAsync(ActivitySession session);
        Task UpdateAsync(ActivitySession session);
        Task DeleteAsync(int id);
    }
}
