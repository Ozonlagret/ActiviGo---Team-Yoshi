using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking?> GetByIdWithDetailsAsync(int id); 
        Task<IEnumerable<Booking>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetUserUpcomingBookingsAsync(int userId);
        Task<IEnumerable<Booking>> GetUserPastBookingsAsync(int userId);
        Task<bool> HasOverlappingBookingAsync(int userId, DateTime startTime, DateTime endTime);
        Task<int> GetActiveBookingCountForSessionAsync(int sessionId);

        Task<Dictionary<int, int>> GetBookingCountByActivityAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<int, int>> GetBookingCountByLocationAsync(DateTime startDate, DateTime endDate);

        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
    }
}
