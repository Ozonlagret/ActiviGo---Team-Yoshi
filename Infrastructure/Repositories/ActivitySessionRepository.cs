using Application.DTOs.Requests;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ActivitySessionRepository : IActivitySessionRepository
    {
        private readonly ActiviGoDbContext _db;
        public ActivitySessionRepository(ActiviGoDbContext db) => _db = db;

        public Task<ActivitySession?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.ActivitySessions.FirstOrDefaultAsync(s => s.Id == id, ct);

        public Task<ActivitySession?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
            => _db.ActivitySessions
                  .Include(s => s.Activity)
                  .Include(s => s.Location)
                  .Include(s => s.Bookings.Where(b => b.Status == BookingStatus.Active))
                  .FirstOrDefaultAsync(s => s.Id == id, ct);

        public async Task<IEnumerable<ActivitySession>> GetAllAsync(CancellationToken ct = default)
            => await _db.ActivitySessions.AsNoTracking().ToListAsync(ct);

        // Redundant methods considering the one below
        //public async Task<IEnumerable<ActivitySession>> GetByActivityIdAsync(int activityId, CancellationToken ct = default)
        //    => await _db.ActivitySessions
        //        .Where(s => s.ActivityId == activityId)
        //        .AsNoTracking()
        //        .ToListAsync(ct);

        //public async Task<IEnumerable<ActivitySession>> GetByLocationIdAsync(int locationId, CancellationToken ct = default)
        //    => await _db.ActivitySessions
        //        .Where(s => s.LocationId == locationId)
        //        .AsNoTracking()
        //        .ToListAsync(ct);

        public async Task<IEnumerable<ActivitySession>> FilterAvailableSessionsAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? categoryId,
            bool? isIndoor,
            int? locationId,
            CancellationToken ct = default)
        {
            var q = _db.ActivitySessions
                .Include(s => s.Activity)
                    .ThenInclude(a => a.Category)
                .Include(s => s.Location)
                .Where(s => !s.IsCanceled)
                .AsQueryable();

            if (startDate.HasValue) q = q.Where(s => s.StartUtc >= startDate.Value);
            if (endDate.HasValue) q = q.Where(s => s.EndUtc <= endDate.Value);
            if (categoryId.HasValue) q = q.Where(s => s.Activity.CategoryId == categoryId.Value);
            if (isIndoor.HasValue) q = q.Where(s => s.Location.IsIndoor == isIndoor.Value);
            if (locationId.HasValue) q = q.Where(s => s.LocationId == locationId.Value);

            return await q.AsNoTracking().ToListAsync(ct);
        }

        public Task<ActivitySession?> GetSessionWithBookingsAsync(int sessionId, CancellationToken ct = default)
            => _db.ActivitySessions
                  .Include(s => s.Bookings)
                  .FirstOrDefaultAsync(s => s.Id == sessionId, ct);

        public async Task AddAsync(ActivitySession session, CancellationToken ct = default)
        {
            await _db.ActivitySessions.AddAsync(session, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(ActivitySession session, CancellationToken ct = default)
        {
            _db.ActivitySessions.Update(session);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _db.ActivitySessions.FirstOrDefaultAsync(s => s.Id == id, ct);
            if (entity is null) return;

            _db.ActivitySessions.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
