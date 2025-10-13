using Application.Interfaces.Repository;
using Domain.Entities;
using Domain.Entities.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Repositories
{
    public class ActivitySessionRepository : IActivitySessionRepository
    {
        private readonly ActiviGoDbContext _context;

        public ActivitySessionRepository(ActiviGoDbContext context)
        {
            _context = context;
        }

        public async Task<ActivitySession?> GetByIdAsync(int id)
        {
            return await _context.ActivitySessions
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ActivitySession?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.ActivitySessions
                .Include(s => s.Activity)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<ActivitySession>> GetAllAsync()
        {
            return await _context.ActivitySessions
                .Include(s => s.Activity)
                .Include(s => s.Location)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivitySession>> GetByActivityIdAsync(int ActivityId)
        {
            return await _context.ActivitySessions
                .Where(s => s.ActivityId == ActivityId)
                .Include(s => s.Activity)
                .Include(s => s.Location)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivitySession>> GetByLocationIdAsync(int LocationId)
        {
            return await _context.ActivitySessions
                .Where(s => s.LocationId == LocationId)
                .Include(s => s.Location)
                .Include(s => s.Activity)
                .ToListAsync();
        }

        public async Task<IEnumerable<ActivitySession>> GetAvailableSessionsAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? categoryId,
            bool? isIndoor,
            int? locationId)
        {
            var query = _context.ActivitySessions
                .Include(s => s.Activity)
                .Include(s => s.Location)
                .Include(s => s.Bookings)
                .AsQueryable();

            // Filter by date range
            if (startDate.HasValue)
                query = query.Where(s => s.StartUtc >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(s => s.EndUtc <= endDate.Value);

            // Filter by category 
            if (categoryId.HasValue)
                query = query.Where(s => s.Activity.CategoryId == categoryId.Value);

            // Filter by indoor/outdoor 
            if (isIndoor.HasValue)
                query = query.Where(s => s.Location.IsIndoor == isIndoor.Value);

            // Filter by location
            if (locationId.HasValue)
                query = query.Where(s => s.LocationId == locationId.Value);

            // Only sessions not canceled and not full
            query = query.Where(s => !s.IsCanceled && s.Bookings.Count() < s.Capacity);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ActivitySession>> GetUpcomingSessionsAsync(
            UpcomingRange range, 
            string? activityName = null)
        {
            var now = DateTime.UtcNow;
            DateTime endDate = range switch
            {
                UpcomingRange.Next7Days => now.AddDays(7),
                UpcomingRange.NextMonth => now.AddMonths(1),
                _ => now.AddDays(7) // default behavior
            };

            var query = _context.ActivitySessions
                .Include(s => s.Activity)
                .Include(s => s.Location)
                .Where(s =>
                    !s.IsCanceled &&
                    s.StartUtc >= now &&
                    s.StartUtc <= endDate &&
                    s.Bookings.Count() < s.Capacity);

            if (!string.IsNullOrWhiteSpace(activityName))
                query = query.Where(s => s.Activity.Name == activityName);
            
            return await query
                .OrderBy(s => s.StartUtc)
                .ToListAsync();
        }

        public async Task<ActivitySession?> GetSessionWithBookingsAsync(int sessionId)
        {
            return await _context.ActivitySessions
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(s => s.Id == sessionId);
        }

        public async Task AddAsync(ActivitySession session)
        {
            await _context.ActivitySessions.AddAsync(session);
        }

        public void Update(ActivitySession session)
        {
            _context.ActivitySessions.Update(session);
        }

        public async Task DeleteAsync(int id)
        {
            var session = await _context.ActivitySessions.FindAsync(id);
            if (session != null)
                _context.ActivitySessions.Remove(session);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ActivitySessions.AnyAsync(a => a.Id == id);
        }
    }
}

