using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RSSReader.Data;
using RSSReader.Models;
using RSSReader.ViewModel;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSReader.Controllers
{
    public class FeedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Feed INDEX
        public ActionResult Index()
        {
            IEnumerable<FeedCategory> objFeedCategories = _context.FeedCategories.ToList();
            if (objFeedCategories.Count() == 0)
            {
                FeedCategory feedCategory1 = new();
                feedCategory1.Name = "World";
                _context.FeedCategories.Add(feedCategory1);
                _context.SaveChanges();
            }
            objFeedCategories = _context.FeedCategories.ToList();
            IEnumerable<Feed> objFeeds = _context.Feeds.ToList();
            if (objFeeds.Count() == 0)
            {
                Feed newFeed1 = new();
                newFeed1.Name = "BBC NEWS - World";
                newFeed1.Uri = "https://feeds.bbci.co.uk/news/world/rss.xml";
                newFeed1.FeedCategoryId = _context.FeedCategories.FirstOrDefault(e => e.Name == "World").Id;
                _context.Feeds.Add(newFeed1);

                Feed newFeed2 = new();
                newFeed2.Name = "The New York Times - World";
                newFeed2.Uri = "https://rss.nytimes.com/services/xml/rss/nyt/World.xml";
                newFeed2.FeedCategoryId = _context.FeedCategories.FirstOrDefault(e => e.Name == "World").Id;
                _context.Feeds.Add(newFeed2);
                _context.SaveChanges();



            }
            return View(objFeeds);
        }

        // Feed CREATE
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feed newFeed)
        {
            if (ModelState.IsValid)
            {
                if (newFeed.Uri == null)
                {
                    return View(newFeed);
                }
                newFeed.LastUpdate = DateTime.UtcNow.AddHours(1);

                XmlReader reader;
                SyndicationFeed feeds;

                try
                {                    
                    reader = XmlReader.Create(newFeed.Uri);
                    feeds = SyndicationFeed.Load(reader);
                }
                catch (Exception)
                {
                    ViewData["Error"] = "Please enter a valid RSS link!";
                    return View(newFeed);
                }


                _context.Feeds.Add(newFeed);
                _context.SaveChanges();

                foreach (var loadedArticle in feeds.Items)
                {
                    Article article = new Article
                    {
                        FeedId = newFeed.Id,
                        ArticleRssID = loadedArticle.Id,
                        Title = loadedArticle.Title.Text
                    };

                    if (loadedArticle.Summary == null)
                    {
                        article.Summary = "Summary not found.";
                    }
                    else
                    {
                        article.Summary = loadedArticle.Summary.Text;
                    }

                    article.PublishDate = loadedArticle.PublishDate.DateTime;
                    article.Uri = loadedArticle.Links[0].Uri.AbsoluteUri;

                    _context.Articles.Add(article);
                    _context.SaveChanges();
                }


                return RedirectToAction("Index");
            }
            else
            {
                return View(newFeed);
            }
        }

        // Feed RELOAD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReloadFeed(int id)
        {
            Feed? feed = _context.Feeds.Find(id);

            if (feed != null && feed.Uri != null)
            {
                XmlReader reader = reader = XmlReader.Create(feed.Uri);
                SyndicationFeed loadedFeed = SyndicationFeed.Load(reader);
                
                
                feed.LastUpdate = DateTime.UtcNow.AddHours(1);

                List<Article> articlesFromDb = _context.Articles.Where(x => x.FeedId == id).ToList();

                foreach (var loadedArticle in loadedFeed.Items)
                {
                    Article? articleFromDb = articlesFromDb.Find(x => x.ArticleRssID == loadedArticle.Id);
                    if (articleFromDb == null)
                    {
                        articleFromDb = new Article();
                        articleFromDb.FeedId = feed.Id;
                        articleFromDb.ArticleRssID = loadedArticle.Id;
                    }

                    articleFromDb.Title = loadedArticle.Title.Text;
                    articleFromDb.PublishDate = loadedArticle.PublishDate.DateTime;
                    articleFromDb.Uri = loadedArticle.Links[0].Uri.AbsoluteUri;

                    if (loadedArticle.Summary == null)
                    {
                        articleFromDb.Summary = "Summary not found.";
                    }
                    else
                    {
                        articleFromDb.Summary = loadedArticle.Summary.Text;
                    }

                    if (articleFromDb.Id == 0)
                    {
                        _context.Articles.Add(articleFromDb);
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.Update(articleFromDb);
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Read", "Articles", new { id = id });
        }

        // Feed EDIT
        public ActionResult Edit(int id)
        {
            Feed? feed = _context.Feeds.Find(id);
            if (feed == null)
            {
                return RedirectToAction("Index");
            }
            return View(feed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Feed editedFeed)
        {

            XmlReader reader;
            SyndicationFeed feeds;

            try
            {
                if (editedFeed.Uri != null)
                {
                    reader = XmlReader.Create(editedFeed.Uri);
                    feeds = SyndicationFeed.Load(reader);
                }

            }
            catch (Exception)
            {
                ViewData["Error"] = "Please enter a valid RSS link!";
                return View(editedFeed);
            }
            editedFeed.LastUpdate = DateTime.UtcNow.AddHours(1);

            _context.Update(editedFeed);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        //Feed DELETE
        [HttpPost]
        public ActionResult DeleteSelectedFeeds(List<int> selectedFeedsId)
        {
            foreach (int feedId in selectedFeedsId)
            {
                Feed? feed = _context.Feeds.Find(feedId);
                if (feed != null)
                {
                    _context.Feeds.Remove(feed);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
