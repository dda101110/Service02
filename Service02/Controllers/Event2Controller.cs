using Microsoft.AspNetCore.Mvc;
using Service02.Models;

namespace Service02.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Event2Controller : ControllerBase
    {
        private readonly EventQueue _eventQueue;

        public Event2Controller(EventQueue eventQueue)
        {
            _eventQueue = eventQueue;
        }

        [Route("{userId:long}/{ipAddress}")]
        public IActionResult CreateEvent(long userId, string ipAddress)
        {
            var @event = new EventDto()
            {
                UserId = userId,
                IpAddress = ipAddress, 
                Connection = DateTime.UtcNow,
            };

            _eventQueue.Enqueue(@event);

            return Ok("OK");
        }
    }
}
