using Microsoft.AspNetCore.Mvc;
using RSSReader.Data;
using RSSReader.Models;

namespace RSSReader.Controllers
{
    public class FeedCategoryRecordController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FeedCategoryRecordController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            Article article = _context.Articles.Where(e => e.Id == id).First();
            int feedCategoryId = _context.Feeds.Where(x => x.Id == article.FeedId).First().FeedCategoryId;

            FeedCategoryRecord feedCategoryRecord = new FeedCategoryRecord();            
            feedCategoryRecord.FeedCategory = _context.FeedCategories.Where(x => x.Id == feedCategoryId).First();
            feedCategoryRecord.Date = DateTime.Now;
            _context.FeedCategoryRecords.Add(feedCategoryRecord);
            _context.SaveChanges();

            return Redirect(article.ArticleRssID);
        }
    }
}
