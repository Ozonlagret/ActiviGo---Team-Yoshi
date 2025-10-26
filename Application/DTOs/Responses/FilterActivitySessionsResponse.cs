using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public record FilterActivitySessionResponse(
    int Id,
    int ActivityId,
    string ActivityName,
    int LocationId,
    DateTime StartUtc,
    DateTime EndUtc,
    int Capacity,
    bool IsCanceled,
    int BookedCount
    );
}
