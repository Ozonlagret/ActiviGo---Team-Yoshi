using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Service;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookings;
        private readonly IActivitySessionRepository _sessions;

        public BookingService(IBookingRepository bookings, IActivitySessionRepository sessions)
        {
            _bookings = bookings;
            _sessions = sessions;
        }

        public async Task<BookingResponse> CreateBookingAsync(int userId, BookSessionRequest req, CancellationToken ct)
        {
            var session = await _sessions.GetByIdWithDetailsAsync(req.ActivitySessionId, ct);
            if (session is null || session.IsCanceled)
                throw new KeyNotFoundException("Tillfället hittades inte eller är inställt.");

            // Kapacitetskontroll
            var activeCount = await _bookings.GetActiveBookingCountForSessionAsync(session.Id, ct);
            if (activeCount >= session.Capacity)
                throw new InvalidOperationException("Tillfället är fullbokat.");

            // Dubbelbokningsskydd, överlapp
            var overlap = await _bookings.HasOverlappingBookingAsync(userId, session.StartUtc, session.EndUtc, ct);
            if (overlap)
                throw new InvalidOperationException("Du har redan en bokning som överlappar denna tidsperiod.");

            var entity = new Booking
            {
                UserId = userId,
                ActivitySessionId = session.Id,
                BookingTimeUtc = DateTime.UtcNow,
                Status = BookingStatus.Active
            };

            await _bookings.AddAsync(entity, ct); // repo sparar själv

            return new BookingResponse(
                entity.Id,
                entity.ActivitySessionId,
                session.Activity.Name,
                entity.BookingTimeUtc,
                entity.Status.ToString(),
                session.StartUtc.ToString("o"),
                session.EndUtc.ToString("o")
            );
        }

        public async Task<bool> CancelAsync(int userId, CancelBookingRequest req, CancellationToken ct)
        {
            var b = await _bookings.GetByIdWithDetailsAsync(req.BookingId, ct);
            if (b is null || b.UserId != userId)
                throw new KeyNotFoundException("Bokningen kunde inte hittas.");

            if (b.Status != BookingStatus.Active)
                return false;

            // Cutoff
            var cutoffHours = b.ActivitySession.Activity.CancellationCutoffHours;
            var cutoffPoint = b.ActivitySession.StartUtc.AddHours(-cutoffHours);
            if (DateTime.UtcNow >= cutoffPoint)
                throw new InvalidOperationException($"Avbokningsgränsen har passerats ({cutoffHours} timmar före start).");

            b.Status = BookingStatus.Canceled;
            await _bookings.UpdateAsync(b, ct); // repo sparar själv
            return true;
        }

        public async Task<MyBookingsResponse> GetMyAsync(int userId, CancellationToken ct)
        {
            var upcoming = await _bookings.GetUserUpcomingBookingsAsync(userId, ct);
            var past = await _bookings.GetUserPastBookingsAsync(userId, ct);
            var all = await _bookings.GetByUserIdAsync(userId, ct);
            var canceled = all.Where(b => b.Status == BookingStatus.Canceled);

            var upcomingDtos = upcoming.Select(x => new BookingResponse(
                x.Id,
                x.ActivitySessionId,
                x.ActivitySession.Activity.Name,
                x.BookingTimeUtc,
                x.Status.ToString(),
                x.ActivitySession.StartUtc.ToString("o"),
                x.ActivitySession.EndUtc.ToString("o")
            )).ToList();

            var pastDtos = past.Select(x => new BookingResponse(
                x.Id,
                x.ActivitySessionId,
                x.ActivitySession.Activity.Name,
                x.BookingTimeUtc,
                x.Status.ToString(),
                x.ActivitySession.StartUtc.ToString("o"),
                x.ActivitySession.EndUtc.ToString("o")
            )).ToList();

            var canceledDtos = canceled.Select(x => new BookingResponse(
                x.Id,
                x.ActivitySessionId,
                x.ActivitySession.Activity.Name,
                x.BookingTimeUtc,
                x.Status.ToString(),
                x.ActivitySession.StartUtc.ToString("o"),
                x.ActivitySession.EndUtc.ToString("o")
            )).ToList();

            return new MyBookingsResponse(upcomingDtos, pastDtos, canceledDtos);
        }

    }
}
