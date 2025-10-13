using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
    public record CreateActivitySessionRequest(
        int ActivityId,
        int LocationId,
        DateTime StartUtc,
        DateTime EndUtc,
        int Capacity
        );
}
