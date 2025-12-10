using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VisionShare.Models
{
    public class Idea
    {
        // Primary Key
        [Key]
        public int IdeaId { get; set; }

        // Title
        [Required]
        public string Title { get; set; }

        // Description
        [Required]
        public string Description { get; set; }

        // Optional: Author name
        public string Author { get; set; }

        // Image path stored in wwwroot/images
        public string FeatureImagePath { get; set; }

        // Creation date
        public DateTime DatePosted { get; set; }

        // User who uploaded (Identity Name)
        public string UserId { get; set; }

        // Count stored for fast access
        public int UpvoteCount { get; set; } = 0;

        // NEW: View count
        public int ViewCount { get; set; } = 0;

        // Navigation property
        public virtual ICollection<IdeaUpvote> Upvotes { get; set; } = new List<IdeaUpvote>();

        // Computed property
        public int LikeCount => Upvotes?.Count ?? 0;
    }
}
