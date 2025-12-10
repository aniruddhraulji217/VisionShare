using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisionShare.Models
{
    public class IdeaUpvote
    {
        [Key]
        public int IdeaUpvoteId { get; set; }

        [Required]
        public int IdeaId { get; set; }

        [ForeignKey(nameof(IdeaId))]
        public Idea Idea { get; set; }

        [Required]
        public string UserId { get; set; }

        // Optional: link to IdentityUser if you have a custom ApplicationUser:
        // public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
