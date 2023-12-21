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

        public IActionResult Personalised()
        {
            FeedArticlesCategoriesViewModel feedArticlesCategoriesViewModel = new FeedArticlesCategoriesViewModel();    
            feedArticlesCategoriesViewModel.Feeds = _context.Feeds.ToList();
            List<FeedCategory> feedCategories = _context.FeedCategories.ToList();
            List<FeedCategoryRecord> feedCategoriesRecord = _context.FeedCategoryRecords.ToList();
            List<FeedCategoryRecord> feedCategoryRecordWeek = feedCategoriesRecord.Where(x => x.Date > DateTime.Now.AddDays(-7)).ToList();
            feedArticlesCategoriesViewModel.FeedCategories = feedCategories;
            List<Article> articles = _context.Articles.OrderByDescending(e => e.PublishDate).ToList();

            double businessScore = 0;
            int bussinessId = feedCategories.First(e => e.Name == "Business").Id;
            double technologyScore = 0;
            int technologyId = feedCategories.First(e => e.Name == "Technology").Id;
            double scienceAEnviromentScore = 0;
            int scienceAEnviromentId = feedCategories.First(e => e.Name == "Science & Environment").Id;
            double educationAFamilyScore = 0;
            int educationAFamilyId = feedCategories.First(e => e.Name == "Education & Family").Id;
            double entertainmentAArtsScore = 0;
            int entertainmentAArtsId = feedCategories.First(e => e.Name == "Entertainment & Arts").Id;
            double healthScore = 0;
            int healthId = feedCategories.First(e => e.Name == "Health").Id;
            double politicsScore = 0;
            int politicsId = feedCategories.First(e => e.Name == "Politics").Id;
            double sportsScore = 0;
            int sportsId = feedCategories.First(e => e.Name == "Sports").Id;

            //Last week. Coefficient: 0.5
            int readedArticlesCount = feedCategoryRecordWeek.Count();
            businessScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == bussinessId).Count() / readedArticlesCount;
            technologyScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == technologyId).Count() / readedArticlesCount;
            scienceAEnviromentScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == scienceAEnviromentId).Count() / readedArticlesCount;
            educationAFamilyScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == educationAFamilyId).Count() / readedArticlesCount;
            entertainmentAArtsScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == entertainmentAArtsId).Count() / readedArticlesCount;
            healthScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == healthId).Count() / readedArticlesCount;
            politicsScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == politicsId).Count() / readedArticlesCount;
            sportsScore += 0.5 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == sportsId).Count() / readedArticlesCount;

            //1 to 2 weeks ago. Coefficient: 0.25
            feedCategoryRecordWeek = feedCategoriesRecord.Where(x => x.Date > DateTime.Now.AddDays(-14) && x.Date < DateTime.Now.AddDays(-7)).ToList();
            businessScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == bussinessId).Count() / readedArticlesCount;
            technologyScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == technologyId).Count() / readedArticlesCount;
            scienceAEnviromentScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == scienceAEnviromentId).Count() / readedArticlesCount;
            educationAFamilyScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == educationAFamilyId).Count() / readedArticlesCount;
            entertainmentAArtsScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == entertainmentAArtsId).Count() / readedArticlesCount;
            healthScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == healthId).Count() / readedArticlesCount;
            politicsScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == politicsId).Count() / readedArticlesCount;
            sportsScore += 0.25 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == sportsId).Count() / readedArticlesCount;            

            //2 to 3 weeks ago. Coefficient: 0.125
            feedCategoryRecordWeek = feedCategoriesRecord.Where(x => x.Date > DateTime.Now.AddDays(-21) && x.Date < DateTime.Now.AddDays(-14)).ToList();
            businessScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == bussinessId).Count() / readedArticlesCount;
            technologyScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == technologyId).Count() / readedArticlesCount;
            scienceAEnviromentScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == scienceAEnviromentId).Count() / readedArticlesCount;
            educationAFamilyScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == educationAFamilyId).Count() / readedArticlesCount;
            entertainmentAArtsScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == entertainmentAArtsId).Count() / readedArticlesCount;
            healthScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == healthId).Count() / readedArticlesCount;
            politicsScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == politicsId).Count() / readedArticlesCount;
            sportsScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == sportsId).Count() / readedArticlesCount;

            //Older than 3 weeks. Coefficient: 0.125
            feedCategoryRecordWeek = feedCategoriesRecord.Where(x => x.Date < DateTime.Now.AddDays(-21)).ToList();
            businessScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == bussinessId).Count() / readedArticlesCount;
            technologyScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == technologyId).Count() / readedArticlesCount;
            scienceAEnviromentScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == scienceAEnviromentId).Count() / readedArticlesCount;
            educationAFamilyScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == educationAFamilyId).Count() / readedArticlesCount;
            entertainmentAArtsScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == entertainmentAArtsId).Count() / readedArticlesCount;
            healthScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == healthId).Count() / readedArticlesCount;
            politicsScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == politicsId).Count() / readedArticlesCount;
            sportsScore += 0.125 * feedCategoryRecordWeek.Where(x => x.FeedCategoryId == sportsId).Count() / readedArticlesCount;
            double sumOfScore = 0;
            sumOfScore = businessScore + technologyScore + scienceAEnviromentScore + educationAFamilyScore + entertainmentAArtsScore + healthScore + politicsScore + sportsScore;

            if (sumOfScore == 0 || Double.IsNaN(sumOfScore))
            {
                businessScore = 1 / 8.0;
                technologyScore = 1 / 8.0;
                scienceAEnviromentScore = 1 / 8.0;
                educationAFamilyScore = 1 / 8.0;
                entertainmentAArtsScore = 1 / 8.0;
                healthScore = 1 / 8.0;
                politicsScore = 1 / 8.0;
                sportsScore = 1 / 8.0;
                sumOfScore = 1.0;
            }
            List<Article> selectedArticles = new List<Article>();
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == bussinessId).Take((int)((businessScore / sumOfScore) * 60) + 5).ToList());
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == technologyId).Take((int)((technologyScore / sumOfScore) * 60) + 5).ToList());
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == scienceAEnviromentId).Take((int)((scienceAEnviromentScore / sumOfScore) * 60) + 5).ToList());
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == educationAFamilyId).Take((int)((educationAFamilyScore / sumOfScore) * 60) + 5).ToList());
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == entertainmentAArtsId).Take((int)((entertainmentAArtsScore / sumOfScore) * 60) + 5).ToList());
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == healthId).Take((int)((healthScore / sumOfScore) * 60) + 6).ToList());
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == politicsId).Take((int)((politicsScore / sumOfScore) * 60) + 6).ToList());
            selectedArticles.AddRange(articles.Where(e => e.Feed.FeedCategoryId == sportsId).Take((int)((sportsScore / sumOfScore) * 60) + 5).ToList());

            Random random = new Random();
            feedArticlesCategoriesViewModel.Articles = selectedArticles.OrderBy((x => random.Next()));
            return View(feedArticlesCategoriesViewModel);
        }
    }
}
