using Microsoft.AspNetCore.Mvc;
using ParameterTampering.Entities;
using ParameterTampering.Models;
using ParameterTampering.ViewModels;
using System.Diagnostics;
using System.Linq;
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

        // It is possible to send POST or edit hidden field in HTML
        // https://localhost:44335/catalog/EditArticle?Id=1&Quantity=100&Price=1.00&Name=Lego

        [HttpPost]
        public async Task<IActionResult> EditArticle(Article article)
        {
            // Never trust to parameters
            // And better use DTO class (Dto.ArticleDto) to prevent database schema disclosure (Exposure of Sensitive System Information)

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
