using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSSReader.Models
{
    public class FeedCategoryRecord
    {

        [Key]
        public int Id { get; set; }

        [Required]        
        public int FeedCategoryId { get; set; }
        [ForeignKey("FeedCategoryId")]
        public FeedCategory FeedCategory { get; set; }

        [ValidateNever]
        public DateTime Date { get; set; }
    }
}
