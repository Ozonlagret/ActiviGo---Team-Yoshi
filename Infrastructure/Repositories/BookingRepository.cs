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
    public class BookingRepository : IBookingRepository
    {
        private readonly ActiviGoDbContext _db;
        public BookingRepository(ActiviGoDbContext db) => _db = db;

        public Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, ct);

        public Task<Booking?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
            => _db.Bookings
                  .Include(b => b.ActivitySession)
                      .ThenInclude(s => s.Activity)
                  .Include(b => b.ActivitySession.Location)
                  .AsNoTracking()
                  .FirstOrDefaultAsync(b => b.Id == id, ct);

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(int userId, CancellationToken ct = default)
            => await _db.Bookings
                .Include(b => b.ActivitySession)
                    .ThenInclude(s => s.Activity)
                .Where(b => b.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Booking>> GetUserUpcomingBookingsAsync(int userId, CancellationToken ct = default)
            => await _db.Bookings
                .Include(b => b.ActivitySession)
                    .ThenInclude(s => s.Activity)
                .Where(b => b.UserId == userId
                         && b.Status == BookingStatus.Active
                         && b.ActivitySession.EndUtc > DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Booking>> GetUserPastBookingsAsync(int userId, CancellationToken ct = default)
            => await _db.Bookings
                .Include(b => b.ActivitySession)
                    .ThenInclude(s => s.Activity)
                .Where(b => b.UserId == userId
                         && b.Status == BookingStatus.Active
                         && b.ActivitySession.EndUtc <= DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync(ct);

        public Task<bool> HasOverlappingBookingAsync(int userId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default)
            => _db.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == userId && b.Status == BookingStatus.Active)
                .Join(_db.ActivitySessions, b => b.ActivitySessionId, s => s.Id, (b, s) => s)
                .AnyAsync(s => s.StartUtc < endUtc && startUtc < s.EndUtc, ct);

        public Task<int> GetActiveBookingCountForSessionAsync(int sessionId, CancellationToken ct = default)
            => _db.Bookings.AsNoTracking().CountAsync(b => b.ActivitySessionId == sessionId && b.Status == BookingStatus.Active, ct);

        public async Task<Dictionary<int, int>> GetBookingCountByActivityAsync(DateTime startUtc, DateTime endUtc, CancellationToken ct = default)
            => await _db.Bookings
                .AsNoTracking()
                .Where(b => b.ActivitySession.StartUtc >= startUtc && b.ActivitySession.EndUtc <= endUtc)
                .GroupBy(b => b.ActivitySession.ActivityId)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count, ct);

        public async Task<Dictionary<int, int>> GetBookingCountByLocationAsync(DateTime startUtc, DateTime endUtc, CancellationToken ct = default)
            => await _db.Bookings
                .AsNoTracking()
                .Where(b => b.ActivitySession.StartUtc >= startUtc && b.ActivitySession.EndUtc <= endUtc)
                .GroupBy(b => b.ActivitySession.LocationId)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count, ct);

        public async Task AddAsync(Booking booking, CancellationToken ct = default)
        {
            await _db.Bookings.AddAsync(booking, ct);
            await _db.SaveChangesAsync(ct); 
        }

        public async Task UpdateAsync(Booking booking, CancellationToken ct = default)
        {
            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);
            if (entity is null) return;

            _db.Bookings.Remove(entity);
            await _db.SaveChangesAsync(ct); 
        }
    }
}
