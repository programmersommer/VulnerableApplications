using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace PathTraversal.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public ApiController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Path Traversal example
        /// </summary>
        /// <remarks>
        /// Sometimes it is possible to guess path or get it with Full Path Disclosure vulnerability
        /// <br /> <br />
        ///  <a href="https://owasp.org/www-community/attacks/Path_Traversal" target="_blank">OWASP Path Traversal</a>
        ///  <br /><br /> 
        ///  <a href="https://localhost:44339/api/GetText?name=privacy.txt" target="_blank">https://localhost:44339/api/GetText?name=privacy.txt</a>
        /// <br /><br />
        ///  <a href="https://localhost:44339/api/GetText?name=..//..//appsettings.json" target="_blank">https://localhost:44339/api/GetText?name=..//..//appsettings.json</a>
        /// <br />
        ///  <a href="https://localhost:44339/api/GetText?name=..%2f..%2fappsettings.json" target="_blank">https://localhost:44339/api/GetText?name=..%2f..%2fappsettings.json</a>
        /// <br /> <br />
        /// Do not use param as part of path. Send id and get path from database
        /// <br />
        /// Sanitization is not recommended (because of possible tricks and OS specific functional)
        /// </remarks>
        /// <param name="name"></param>
        /// <response code="200">Just returns Ok</response>
        [HttpGet]
        public IActionResult GetText(string name)
        {
            var root = _env.WebRootPath;

            // and you are thinking that it is possible to get only files from texts folder
            var path = Path.Combine(root, "texts", name);

            // next sanitization: 
            // path = path.Replace(@"../", "");
            // could be pass over with trick like this:
            // https://localhost:44339/api/GetText?name=..././..././appsettings.json

            var fileContent = System.IO.File.ReadAllText(path);

            return Content(fileContent);
        }


    }
}