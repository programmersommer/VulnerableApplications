using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRLF.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }


        // CRLF injection example
        // https://owasp.org/www-community/vulnerabilities/CRLF_Injection
        // https://localhost:44340/api/getok?param=123%0d%0aCRLF.Controllers.AdminController:%20Information:%20555
        // simple solution - param.Replace(System.Environment.NewLine, "");
        [HttpGet]
        public IActionResult GetOk(string param)
        {
            _logger.LogInformation(param);

            return Ok("Ok");
        }

    }
}