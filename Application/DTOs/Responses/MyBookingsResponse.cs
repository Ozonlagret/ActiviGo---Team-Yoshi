using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public sealed record MyBookingsResponse(
    IEnumerable<BookingResponse> Upcoming,
    IEnumerable<BookingResponse> Past,
    IEnumerable<BookingResponse> Canceled
    );
}
