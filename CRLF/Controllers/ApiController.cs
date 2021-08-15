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
        /// </summary>
        /// <remarks>
        ///  <a href="https://owasp.org/www-community/vulnerabilities/CRLF_Injection" target="_blank">OWASP CRLF</a>
        ///  <br /><br /> 
        ///  <a href="https://localhost:44340/api/getok?param=123%0d%0aCRLF.Controllers.AdminController:%20Information:%20555" target="_blank">https://localhost:44340/api/getok?param=123%0d%0aCRLF.Controllers.AdminController:%20Information:%20555</a>
        /// </remarks>
        /// <param name="param"></param>
        /// <response code="200">Just returns Ok</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult GetOk(string param)
        {
            //  solution: do a simple sanitization 
            //  for example: param.Replace(System.Environment.NewLine, "");
            _logger.LogInformation(param);

            return Ok("Ok");
        }
    }
}