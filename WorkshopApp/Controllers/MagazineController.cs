using Microsoft.AspNetCore.Mvc;
using WorkshopApp.ViewModels;

namespace WorkshopApp.Controllers
{
    public class MagazineController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public MagazineController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index(string fileName)
        {
            var root = _env.WebRootPath;

            // it should be possible to get only files from MagazineArticles folder, right?
            var path = Path.Combine(root, "MagazineArticles", fileName);

            var fileContent = System.IO.File.ReadAllText(path);

            var model = new MagazineViewModel() { Text = fileContent };

            return View(model);
        }
    }
}
