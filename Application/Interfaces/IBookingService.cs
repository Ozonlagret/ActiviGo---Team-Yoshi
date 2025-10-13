using Application.DTOs.Requests;
using Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(int userId, BookSessionRequest req, CancellationToken ct);
        Task<bool> CancelAsync(int userId, CancelBookingRequest req, CancellationToken ct);
        Task<MyBookingsResponse> GetMyAsync(int userId, CancellationToken ct);
    }
}
