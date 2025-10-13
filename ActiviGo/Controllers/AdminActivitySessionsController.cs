using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.Controllers
{
    [ApiController]
    [Route("api/admin/sessions")]
    [Authorize(Roles = "Admin")]
    public class AdminActivitySessionsController : ControllerBase
    {
        private readonly IActivitySessionService _activitySessionService;
        public AdminActivitySessionsController(IActivitySessionService activitySessionService)
            => _activitySessionService = activitySessionService;

        [HttpPost]
        public async Task<ActionResult<ActivitySessionResponse>> Create([FromBody] CreateActivitySessionRequest req, CancellationToken ct)
            => Ok(await _activitySessionService.CreateAsync(req, ct));
    }
}
