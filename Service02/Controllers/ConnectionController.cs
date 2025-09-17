using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service02.Services.ConnectionService;

namespace Service02.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private IConnectionService _service;

        public ConnectionController(IConnectionService service)
        {
            _service = service;    
        }

        [Route("{userId:long}")]
        public async Task<IActionResult> GetLastConnection(long userId)
        {
            var result = await _service.GetLastConnectionAsync(userId);

            return Ok(result);
        }

        [Route("{userId:int}/{ipAddress}")]
        public async Task<IActionResult> GetLastTimeConnection(int userId, string ipAddress)
        {
            var result = await _service.GetLastTimeConnectionAsync(userId, ipAddress);

            return Ok(result);
        }
    }
}
