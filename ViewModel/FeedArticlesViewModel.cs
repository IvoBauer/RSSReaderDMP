using RSSReader.Models;

namespace RSSReader.ViewModel
{
    public class FeedArticlesViewModel
    {
        public Feed Feed { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
}
