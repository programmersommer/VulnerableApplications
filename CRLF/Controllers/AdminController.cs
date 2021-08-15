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


        /// <summary>
        /// Just imagine that there is an admin endpoint that is protected with [Authorize]
        /// </summary>
        /// <remarks>
        ///  There is no need to call this action
        /// </remarks>
        /// <param name="param"></param>
        /// <response code="200">Just returns Ok</response>
        [HttpGet]
        public IActionResult SomeSecureAction(string param)
        {
            _logger.LogInformation(param);

            return Ok("Secret");
        }

    }
}