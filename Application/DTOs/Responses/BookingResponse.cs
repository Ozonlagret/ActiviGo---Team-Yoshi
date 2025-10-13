using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public sealed record BookingResponse(
    int Id,
    int ActivitySessionId,
    DateTime BookingTimeUtc,
    string Status
    );
}
