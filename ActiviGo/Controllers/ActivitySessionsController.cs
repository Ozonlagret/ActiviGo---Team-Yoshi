using Application.Interfaces.Service;
using Application.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitySessionsController : ControllerBase
    {
        private readonly IActivitySessionService _activitySessionService;
        public ActivitySessionsController(IActivitySessionService activitySessionService)
            => _activitySessionService = activitySessionService;

        [HttpPost("filter")]
        public async Task<IActionResult> FilterAvailableSessions([FromBody] FilterSessionsRequest request, CancellationToken ct)
        {
            // Convert dates to UTC for PostgreSQL
            if (request.StartDate.HasValue)
                request.StartDate = DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Utc);
            if (request.EndDate.HasValue)
                request.EndDate = DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Utc);
            
            var sessions = await _activitySessionService.FilterAvailableSessionsAsync(request, ct);
            return Ok(sessions);
        }

    }
}
