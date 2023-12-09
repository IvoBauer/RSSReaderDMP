using Microsoft.AspNetCore.Mvc;
using RSSReader.Data;
using RSSReader.Models;
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
                List<FeedCategory> feedCategories = new List<FeedCategory>();
                _context.FeedCategories.Add(new FeedCategory() { Name = "Business", Color = "badge rounded-pill bg-primary" });
                _context.FeedCategories.Add(new FeedCategory() { Name = "Technology", Color = "bg-secondarybadge rounded-pill bg-secondary" });
                _context.FeedCategories.Add(new FeedCategory() { Name = "Science & Environment", Color = "badge rounded-pill bg-success" });
                _context.FeedCategories.Add(new FeedCategory() { Name = "Education & Family", Color = "badge rounded-pill bg-danger" });
                _context.FeedCategories.Add(new FeedCategory() { Name = "Entertainment & Arts", Color = "rounded-pill bg-warning" });
                _context.FeedCategories.Add(new FeedCategory() { Name = "Health", Color = "badge rounded-pill bg-info" });
                _context.FeedCategories.Add(new FeedCategory() { Name = "Politics", Color = "badge rounded-pill bg-light" });
                _context.FeedCategories.Add(new FeedCategory() { Name = "Sports", Color = "badge rounded-pill bg-dark" });
                _context.SaveChanges();
            }

            objFeedCategories = _context.FeedCategories.ToList();
            IEnumerable<Feed> objFeeds = _context.Feeds.ToList();

            if (objFeeds.Count() == 0)
            {
                List<Feed> feeds = new List<Feed>();

                _context.Feeds.Add(new Feed()
                {
                    Name = "BBC NEWS",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Business").Id,
                    Uri = "http://feeds.bbci.co.uk/news/business/rss.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "BBC NEWS",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Politics").Id,
                    Uri = "http://feeds.bbci.co.uk/news/politics/rss.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "BBC NEWS",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Health").Id,
                    Uri = "http://feeds.bbci.co.uk/news/health/rss.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "BBC NEWS",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Education & Family").Id,
                    Uri = "http://feeds.bbci.co.uk/news/education/rss.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "BBC NEWS",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Technology").Id,
                    Uri = "http://feeds.bbci.co.uk/news/technology/rss.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "BBC NEWS",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Science & Environment").Id,
                    Uri = "http://feeds.bbci.co.uk/news/science_and_environment/rss.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "BBC NEWS",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Entertainment & Arts").Id,
                    Uri = "http://feeds.bbci.co.uk/news/entertainment_and_arts/rss.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "The New York Times",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Business").Id,
                    Uri = "https://rss.nytimes.com/services/xml/rss/nyt/Business.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "The New York Times",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Technology").Id,
                    Uri = "https://rss.nytimes.com/services/xml/rss/nyt/Technology.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "The New York Times",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Health").Id,
                    Uri = "https://rss.nytimes.com/services/xml/rss/nyt/Health.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "The New York Times",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Sports").Id,
                    Uri = "https://rss.nytimes.com/services/xml/rss/nyt/Sports.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "The New York Times",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Science & Environment").Id,
                    Uri = "https://rss.nytimes.com/services/xml/rss/nyt/Science.xml"
                });

                _context.Feeds.Add(new Feed()
                {
                    Name = "The New York Times",
                    FeedCategoryId = _context.FeedCategories.First(e => e.Name == "Science & Environment").Id,
                    Uri = "https://rss.nytimes.com/services/xml/rss/nyt/Climate.xml"
                });

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

        
        [ValidateAntiForgeryToken]
        public ActionResult ReloadAllFeeds()
        {
            List<Feed>? feeds = _context.Feeds.ToList();
            foreach (var feed in feeds)
            {
                if (feed != null && feed.Uri != null)
                {
                    XmlReader reader = reader = XmlReader.Create(feed.Uri);
                    SyndicationFeed loadedFeed = SyndicationFeed.Load(reader);


                    feed.LastUpdate = DateTime.UtcNow.AddHours(1);

                    List<Article> articlesFromDb = _context.Articles.Where(x => x.FeedId == feed.Id).ToList();

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
            }
            return RedirectToAction("Personalised", "Articles");
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
                    var recordsToRemove = _context.FeedCategoryRecords.Where(e => e.FeedCategoryId == feed.FeedCategoryId);
                    _context.FeedCategoryRecords.RemoveRange(recordsToRemove);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
