using System.ComponentModel.DataAnnotations;

namespace VisionShare.Models
{
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; }
        [Required(ErrorMessage ="The tite is reqired")]
        [MaxLength(400, ErrorMessage ="The title cannot exceed 200 characters")]
        public string Title { get; set; }
       
        [Required(ErrorMessage = "The description is reqired")]

        public string Description { get; set; }

        [Required(ErrorMessage = "The Author is reqired")]
        [MaxLength(100, ErrorMessage = "The Author cannot exceed 100 characters")]
        public string Author { get; set; }
        public string UserId { get; set; }

        public string FeatureImagePath { get; set; }   // for image upload
        
        [DataType(DataType.Date)]
        public DateTime DatePosted { get; set; }=DateTime.Now;

        // public List<Comment> Comments { get; set; }

        public ICollection <Comment> Comments { get; set; }
    }
}
