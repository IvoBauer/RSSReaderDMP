﻿using System.ComponentModel.DataAnnotations;

namespace RSSReader.Models
{
    public class FeedCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
