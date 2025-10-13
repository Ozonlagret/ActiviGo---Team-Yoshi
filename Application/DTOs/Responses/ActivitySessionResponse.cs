using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public record ActivitySessionResponse(
    int Id,
    int ActivityId,
    int LocationId,
    DateTime StartUtc,
    DateTime EndUtc,
    int Capacity,
    bool IsCanceled,
    int BookedCount
    );
}
