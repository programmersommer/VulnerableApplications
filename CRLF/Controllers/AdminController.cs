using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRLF.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public AdminController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult SomeSecureAction(string param)
        {
            _logger.LogInformation(param);

            return Ok("Secret");
        }

    }
}