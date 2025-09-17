using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service02.Features.Command.CreateEvent;

namespace Service02.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("{userId:long}/{ipAddress}")]
        public async Task<IActionResult> CreateEvent(long userId, string ipAddress)
        {
            var request = new CreateEventCommand()
            {
                UserId = userId,
                IpAddress = ipAddress,
                Connection = DateTime.UtcNow,
            };

            _mediator.Send(request);

            return Ok("OK");
        }
    }
}
