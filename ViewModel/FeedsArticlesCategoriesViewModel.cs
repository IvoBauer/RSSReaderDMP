using RSSReader.Models;

namespace RSSReader.ViewModel
{
    public class FeedArticlesCategoriesViewModel
    {
        public IEnumerable<Feed>? Feeds { get; set; }
        public IEnumerable<Article>? Articles { get; set; }
        public IEnumerable<FeedCategory>? FeedCategories { get; set; }
    }
}
