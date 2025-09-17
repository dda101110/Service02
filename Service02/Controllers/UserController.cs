using Microsoft.AspNetCore.Mvc;
using Service02.Services.UserService;

namespace Service02.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ILogger<UserController> _logger;
        private IUserService _userService { get; set; }

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Route("{ipAddress}")]
        public async Task<IActionResult> GetUser(string ipAddress)
        {
            var result = await _userService.GetUsersByIpAddressAsync(ipAddress);

            return Ok(result);
        }
    }
}
