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
            FeedCategoryRecord feedCategoryRecord = new FeedCategoryRecord();
            int feedCategoryId = _context.Feeds.Where(x => x.Id == id).First().FeedCategoryId;
            feedCategoryRecord.FeedCategory = _context.FeedCategories.Where(x => x.Id == feedCategoryId).First();
            feedCategoryRecord.Date = DateTime.Now;
            _context.FeedCategoryRecords.Add(feedCategoryRecord);
            _context.SaveChanges();

            return Redirect("https://stackoverflow.com/questions/52308563/redirect-to-url-in-asp-net-core");
        }
    }
}
