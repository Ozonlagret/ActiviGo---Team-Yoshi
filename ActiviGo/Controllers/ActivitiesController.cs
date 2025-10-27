using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Responses;
using Application.Interfaces.Service;

namespace ActiviGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityResponse>>> GetAll()
        {
            var activities = await _activityService.GetAllAsync();
            return Ok(activities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActivityResponse>> GetById(int id)
        {
            var activity = await _activityService.GetByIdAsync(id);
            if (activity == null) return NotFound();
            return Ok(activity);
        }

        [HttpGet("{id:int}/exists")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var exists = await _activityService.ExistsAsync(id);
            return Ok(exists);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ActivityResponse>>> GetActive()
        {
            var activities = await _activityService.GetAllActiveAsync();
            return Ok(activities);
        }

        [HttpGet("category/{categoryId:int}")]
        public async Task<ActionResult<IEnumerable<ActivityResponse>>> GetByCategory(int categoryId)
        {
            var activities = await _activityService.GetByCategoryAsync(categoryId);
            return Ok(activities);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ActivityResponse>>> Search([FromQuery] string? category, [FromQuery] bool? isIndoor)
        {
            var activities = await _activityService.SearchAsync(category, isIndoor);
            return Ok(activities);
        }
    }
}
