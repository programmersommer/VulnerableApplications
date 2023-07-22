using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkshopApp.Entities;
using WorkshopApp.Models;
using WorkshopApp.ViewModels;

namespace WorkshopApp.Controllers
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

            if (article == null) return NotFound();

            _context.Entry(article).Collection(a => a.Comments).Load();

            var model = new HomeViewModel()
            {
                Article = article,
                NewComment = new Comment()
                {
                    ArticleId = article.Id
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditArticle(HomeViewModel model)
        {
            _context.Articles.Update(model.Article);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(HomeViewModel model)
        {
            model.NewComment.Created = System.DateTime.Now;

            _context.Comments.Add(model.NewComment);
            await _context.SaveChangesAsync();

            model.Article = new Article()
            {
                Id = model.NewComment.ArticleId,
                Comments = _context.Comments.Where(c => c.ArticleId == model.NewComment.ArticleId).ToList()
            };

            return PartialView("_Comments", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}