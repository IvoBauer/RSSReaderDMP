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
        public IActionResult Read(int? id, DateTime? dateFrom, DateTime? dateTo)
        {
            if (dateTo != null)
            {
                dateTo = dateTo.Value.AddDays(1);

            }
            
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

            //IEnumerable<Article> articles;
            //IEnumerable<Article> articles2;
            //if (dateFrom != null && dateTo != null)
            //{
            //    articles = _context.Articles.Where(x => x.FeedId == id).Where(x => x.PublishDate >= dateFrom).Where(x => x.PublishDate <= dateTo);
            //}
            //else if (dateFrom == null && dateTo != null)
            //{
            //    articles = _context.Articles.Where(x => x.FeedId == id).Where(x => x.PublishDate <= dateTo);
            //}
            //else if (dateFrom != null && dateTo == null)
            //{
            //    articles = _context.Articles.Where(x => x.FeedId == id).Where(x => x.PublishDate >= dateFrom);
            //}
            //else
            //{
            //    articles = _context.Articles.Where(x => x.FeedId == id);
            //}


            IEnumerable<Article> articles = _context.Articles.Where(x => x.FeedId == id);
            if (dateFrom != null)
            {
                articles = articles.Where(x => x.PublishDate >= dateFrom);
            }

            if (dateTo != null)
            {
                articles = articles.Where(x => x.PublishDate <= dateTo);
            }

            if (articles != null)
            {
                feedArticlesViewModel.Articles = articles.ToList()
                .OrderByDescending(f => f.PublishDate);
            }

            return View(feedArticlesViewModel);
        }
    }
}
