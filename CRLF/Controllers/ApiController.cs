using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

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


        /// <summary>
        /// CRLF injection example
        /// https://owasp.org/www-community/vulnerabilities/CRLF_Injection
        /// https://localhost:44340/api/getok?param=123%0d%0aCRLF.Controllers.AdminController:%20Information:%20555
        /// simple solution - param.Replace(System.Environment.NewLine, "");
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult GetOk(string param)
        {
            _logger.LogInformation(param);

            return Ok("Ok");
        }
    }
}