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
        public async Task<ActionResult<IEnumerable<GetActivityDto>>> GetAll()
        {
            var activities = await _activityService.GetAllAsync();
            return Ok(activities);
        }

        [HttpGet]
        public async Task<ActionResult<GetActivityDto>> GetById(int id)
        {
            var activity = await _activityService.GetByIdAsync(id);
            return Ok(activity);
        }
    }
}
