using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActiviGo.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingsController(IBookingService bookingService) => _bookingService = bookingService;

        private int GetUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name);
            return int.TryParse(sub, out var id) ? id : 0;
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Create([FromBody] BookSessionRequest req, CancellationToken ct)
            => Ok(await _bookingService.CreateBookingAsync(GetUserId(), req, ct));

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _bookingService.CancelAsync(GetUserId(), new CancelBookingRequest(id), ct);
            return ok ? NoContent() : BadRequest();
        }

        [HttpGet("my")]
        public async Task<ActionResult<MyBookingsResponse>> Mine(CancellationToken ct)
            => Ok(await _bookingService.GetMyAsync(GetUserId(), ct));
    }
}
