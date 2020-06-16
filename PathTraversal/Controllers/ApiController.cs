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

        // Path Traversal example
        // in case if possible to guess path or get it with Full Path Disclosure vulnerability
        // https://owasp.org/www-community/attacks/Path_Traversal
        // https://localhost:44339/api/GetText?name=privacy.txt
        // https://localhost:44339/api/GetText?name=..//..//appsettings.json
        // or https://localhost:44339/api/GetText?name=..%2f..%2fappsettings.json
        // simple solution - do not use param as part of path. send id and get path from database
        // or sanitize input very carefuly becase of possible tricks (and don't forget to be OS specific)
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