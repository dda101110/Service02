using Microsoft.AspNetCore.Mvc;
using Service02.Services.IpService;

namespace Service02.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private IIpService _ipService;

        public IpController(IIpService service)
        {
            _ipService = service;
        }

        [Route("{userId:long}")]
        public async Task<IActionResult> GetIp(long userId)
        {
            var result = await _ipService.GetIpAddressesAsync(userId);

            return Ok(result);
        }
    }
}
