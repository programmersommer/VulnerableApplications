using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using ZipBombs.Models;

namespace ZipBombs.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostEnvironment _hostEnvironment;

        public HomeController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.Length > 0)
            {
                string zipFilePath = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", file.FileName);
                using (Stream fileStream = new FileStream(zipFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                //var file = ZipFile.OpenRead(filePath);
                //var size = file.Entries.Sum(entry => entry.Length);
                //if (size > 1_000_000_000) return BadRequest("Zip file is too large");

                string contentPath = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", Path.GetFileNameWithoutExtension(zipFilePath));

                ZipFile.ExtractToDirectory(zipFilePath, contentPath); // safe for recursive r.zip
            }

            return Ok("Upload completed!");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}