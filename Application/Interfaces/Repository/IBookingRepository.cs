using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Booking?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<Booking>> GetUserUpcomingBookingsAsync(int userId, CancellationToken ct = default);
        Task<IEnumerable<Booking>> GetUserPastBookingsAsync(int userId, CancellationToken ct = default);

        // Överlapp
        Task<bool> HasOverlappingBookingAsync(int userId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default);

        // Antal aktiva bokningar i en session för kapacitetskontroll
        Task<int> GetActiveBookingCountForSessionAsync(int sessionId, CancellationToken ct = default);

        // Enkel statistik (antal bokningar per aktivitet/plats)
        Task<Dictionary<int, int>> GetBookingCountByActivityAsync(DateTime startUtc, DateTime endUtc, CancellationToken ct = default);
        Task<Dictionary<int, int>> GetBookingCountByLocationAsync(DateTime startUtc, DateTime endUtc, CancellationToken ct = default);

        Task AddAsync(Booking booking, CancellationToken ct = default);
        Task UpdateAsync(Booking booking, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
