using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RSSReader.Data;
using RSSReader.Models;
using RSSReader.ViewModel;

namespace RSSReader.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Articles READ
        public IActionResult Read(int id, DateTime? dateFrom, DateTime? dateTo)
        {
            FeedArticlesViewModel feedArticlesViewModel = new FeedArticlesViewModel();
            feedArticlesViewModel.Feed = _context.Feeds.Find(id);

            if (feedArticlesViewModel.Feed == null)
            {
                return RedirectToAction("Index", "Feeds");
            }

            if (dateFrom > dateTo)
            {
                DateTime? tmp = dateTo;
                dateTo = dateFrom;
                dateFrom = tmp;

            }

            if (dateFrom != null && dateTo != null)
            {
                feedArticlesViewModel.Articles = _context.Articles.Where(x => x.FeedId == id).Where(x => x.PublishDate >= dateFrom).Where(x => x.PublishDate <= dateTo)
                .ToList()
                .OrderByDescending(f => f.PublishDate);
            }
            else if (dateFrom == null && dateTo != null)
            {
                feedArticlesViewModel.Articles = _context.Articles.Where(x => x.FeedId == id).Where(x => x.PublishDate <= dateTo)
                .ToList()
                .OrderByDescending(f => f.PublishDate);
            }
            else if (dateFrom != null && dateTo == null)
            {
                feedArticlesViewModel.Articles = _context.Articles.Where(x => x.FeedId == id).Where(x => x.PublishDate >= dateFrom)
                .ToList()
                .OrderByDescending(f => f.PublishDate);
            }
            else
            {
                feedArticlesViewModel.Articles = _context.Articles.Where(x => x.FeedId == id)
                .ToList()
                .OrderByDescending(f => f.PublishDate);
            }
            return View(feedArticlesViewModel);
        }
    }
}
