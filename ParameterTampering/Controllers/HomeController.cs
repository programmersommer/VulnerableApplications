using Microsoft.AspNetCore.Mvc;
using ParameterTampering.Entities;
using ParameterTampering.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParameterTampering.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreDBContext _context;

        public HomeController(StoreDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int id = 1;
            var article = await _context.Articles.FindAsync(id);

            return View(article);
        }

        // It is possible to send POST or edit hidden field in HTML
        // https://localhost:44335/catalog/EditArticle?Id=1&Quantity=100&Price=1.00&Name=Lego

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticle(/*[Bind("Id", "Name", "Quantity")]*/ Article article)
        {
            // Never trust to parameters
            //var existingArticle = _context.Articles.FirstOrDefault(a => a.Id == article.Id);
            //if (existingArticle != default)
            //{
            //    existingArticle.Name = article.Name;
            //    existingArticle.Quantity = article.Quantity;
            //    _context.Articles.Update(existingArticle);
            //    await _context.SaveChangesAsync();
            //}

            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
