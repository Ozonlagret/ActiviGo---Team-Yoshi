using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActiviGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin")]  // TODO: Uncomment after testing
    public class AdminController : ControllerBase
    {
        private readonly IActivitySessionService _activitySessionService;
        private readonly IActivityService _activityService;
        private readonly ICategoryService _categoryService;
        private readonly ILocationService _locationService;

        public AdminController(
            IActivitySessionService activitySessionService,
            IActivityService activityService,
            ICategoryService categoryService,
            ILocationService locationService)
        {
            _activitySessionService = activitySessionService;
            _activityService = activityService;
            _categoryService = categoryService;
            _locationService = locationService;
        }

        [HttpPost("sessions")]
        public async Task<ActionResult<ActivitySessionResponse>> CreateSession([FromBody] CreateActivitySessionRequest req, CancellationToken ct)
            => Ok(await _activitySessionService.CreateAsync(req, ct));

        [HttpPost("create/activity")]
        public async Task<ActionResult<ActivityResponse>> CreateActivity([FromBody] CreateActivityRequest req, CancellationToken ct)
            => Ok(await _activityService.CreateAsync(req, ct));
    }
}
