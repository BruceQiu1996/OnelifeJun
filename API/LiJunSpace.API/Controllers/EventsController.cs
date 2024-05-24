using LiJunSpace.API.Services;
using LiJunSpace.Common.Dtos.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiJunSpace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventsService _eventsService;
        private readonly ILogger<AccountController> _logger;
        public EventsController(EventsService eventsService, ILogger<AccountController> logger)
        {
            _eventsService = eventsService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(EventCreationDto eventCreationDto) 
        {
            try 
            {
                await _eventsService.CreateEventAsync(eventCreationDto, HttpContext.User.Identity!.Name!);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return Problem();
            }
        }

        [Authorize]
        [HttpGet("main")]
        public async Task<ActionResult> GetMainPageEvents()
        {
            try
            {
                var events = await _eventsService.GetMainPageEventsAsync();

                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return Problem();
            }
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult> GetEvents()
        {
            try
            {
                var events = await _eventsService.GetEventsAsync();

                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return Problem();
            }
        }

        [Authorize]
        [HttpGet("delete/{eventId}")]
        public async Task<ActionResult> DeleteEvent(string eventId)
        {
            try
            {
                await _eventsService.DeleteEventAsync(eventId, HttpContext.User.Identity!.Name!);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return Problem();
            }
        }
    }
}
