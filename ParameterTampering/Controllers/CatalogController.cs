using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParameterTampering.Entities;


namespace ParameterTampering.Controllers
{
    public class CatalogController : Controller
    {
        private readonly StoreDBContext _context;

        public CatalogController(StoreDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int id = 0)
        {
            //await _context.Articles.AddAsync(new Entities.Article()
            //{
            //    Id = 1,
            //    Name = "Lego constructor",
            //    Price = 55,
            //    Quantity = 100
            //});
            //await _context.SaveChangesAsync();

            var article = await _context.Articles.FindAsync(id);

            return View(article);
        }


        // https://localhost:44335/catalog/EditArticle?Id=1&Quantity=100&Price=1.00&Name=Lego
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticle(/*[Bind("Name","Quantity")]*/ Article article)
        {
            // Never trust to parameters
            // Get object from db and map preperties

            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
