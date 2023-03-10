using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RSSReader.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? ArticleRssID { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Summary { get; set; }

        [Required]
        public string? Uri { get; set; }

        public DateTime PublishDate { get; set; }

        public int FeedId { get; set; }
        [ForeignKey("FeedId")]
        [ValidateNever]
        public Feed? Feed { get; set; }
    }
}
